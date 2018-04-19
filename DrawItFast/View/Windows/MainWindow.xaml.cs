using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DrawItFast.Model.Drawing;
using DrawItFast.Model.Drawing.Drawables;
using DrawItFast.Model.Tools;
using DrawItFast.View.Controls;

namespace DrawItFast.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Data binding
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(int), typeof(MainWindow), new UIPropertyMetadata(0));
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(int), typeof(MainWindow), new UIPropertyMetadata(0));

        public int ImageWidth
        {
            get { return (int)this.GetValue(ImageWidthProperty); }
            set { this.SetValue(ImageWidthProperty, value); }
        }

        public int ImageHeight
        {
            get { return (int)this.GetValue(ImageHeightProperty); }
            set { this.SetValue(ImageHeightProperty, value); }
        }
        #endregion

        private int grabbedIndex;
        private int selectedShape;

        public float PointSize;

        public int InterpolationOffset;

        private Color colorLine;
        private Color colorFill;
        private Color colorGuide;
        private Color colorPoint;
        private int lineThickness;

        public static MainWindow Instance { get; private set; }

        private List<IDrawable> drawables;
        private List<ITool> tools;

        private ITool selectedTool;

        public MainWindow()
        {
            InitializeComponent();

            this.ImageWidth = 800;
            this.ImageHeight = 600;

            this.drawables = new List<IDrawable>();
            this.tools = new List<ITool>();

            this.CreateTools();

            this.colorPoint = Colors.Pink;
            this.colorLine = Colors.Black;
            this.colorGuide = Colors.Pink;
            this.colorFill = Colors.Gray;

            this.selectedShape = -1;
            this.lineThickness = 1;

            this.InterpolationOffset = 5;

            this.PointSize = 5;

            this.D2DControl.BindList(this.drawables);

            Instance = this;
        }

        private void CreateTools()
        {
            Type type = typeof(Tools);
            foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                var tool = fieldInfo.GetValue(null);

                RadioButton toolButton = new RadioButton();

                Image icon = new Image();
                icon.Source = new BitmapImage(new Uri(String.Format("/Resources/Icons/Tools/{0}.png", tool.GetType().Name), UriKind.Relative));

                toolButton.Content = icon;
                toolButton.GroupName = "Tools";
                toolButton.Checked += (obj, args) =>
                {
                    this.selectedTool = tool as ITool;
                    this.UpdateToolColors();
                };

                this.CurveToolBox.Items.Add(toolButton);

                this.tools.Add(tool as ITool);
            }
        }

        private void UpdateToolColors()
        {
            if(this.selectedTool != null && this.selectedTool is IDrawTool)
            {
                IDrawTool tool = this.selectedTool as IDrawTool;
                tool.Color1 = this.colorLine.ToRawColor4();
                tool.Color2 = this.colorFill.ToRawColor4();
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            NewImageWindow window = new NewImageWindow();
            window.Show();
        }

        private void DrawCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(this.DrawCanvas);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if(this.selectedTool != null && this.selectedTool is IMoveTool)
                {
                    for (int i = 0; i < this.drawables.Count; i++)
                    {
                        if (this.drawables[i].IsHovering(mousePosition) && (this.selectedTool as IMoveTool).TrySelectShape(this.drawables[i]))
                        {
                            break;
                        }
                    }
                }
            }

            if (this.selectedTool != null)
            {
                this.selectedTool.MouseDown(mousePosition, e);
            }
        }

        private void DrawCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(this.selectedTool != null)
            {
                this.selectedTool.MouseMove(e.GetPosition(this.DrawCanvas), e);
            }
        }

        private void DrawCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(this.selectedTool != null)
            {
                this.selectedTool.MouseUp(e.GetPosition(this.DrawCanvas), e);
            }
        }

        internal void AddShape(IDrawable shape)
        {
            if(!this.drawables.Contains(shape))
            {
                this.drawables.Add(shape);
            }
        }

        internal void RemoveShape(IDrawable shape)
        {
            if (this.drawables.Contains(shape))
            {
                this.drawables.Remove(shape);
            }
        }
    }
}
