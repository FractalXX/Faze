﻿using System;
using System.Collections.Generic;
using System.IO;
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
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace DrawItFast.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Data binding
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(int), typeof(MainWindow), new UIPropertyMetadata(0));
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(int), typeof(MainWindow), new UIPropertyMetadata(0));
        public static readonly DependencyProperty SelectedToolProperty = DependencyProperty.Register("SelectedTool", typeof(ITool), typeof(MainWindow), new UIPropertyMetadata(Tools.BasicMoveTool));

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

        public ITool SelectedTool
        {
            get { return (ITool)this.GetValue(SelectedToolProperty); }
            set { this.SetValue(SelectedToolProperty, value); }
        }
        #endregion

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
                    this.SelectedTool = tool as ITool;
                    this.UpdateToolColors();
                };

                this.BasicToolBox.Items.Add(toolButton);

                this.tools.Add(tool as ITool);
            }
        }

        private void UpdateToolColors()
        {
            if (this.SelectedTool != null && this.SelectedTool is IDrawTool)
            {
                IDrawTool tool = this.SelectedTool as IDrawTool;
                tool.Color1 = this.colorLine;
                tool.Color2 = this.colorFill;
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
            bool justSelected = false;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.SelectedTool != null && this.SelectedTool is IMoveTool && (this.SelectedTool as IMoveTool).SelectedShape == null)
                {
                    for (int i = 0; i < this.drawables.Count; i++)
                    {
                        if (this.drawables[i].IsMouseHovering(mousePosition) && (this.SelectedTool as IMoveTool).TrySelectShape(this.drawables[i]))
                        {
                            justSelected = true;
                            break;
                        }
                    }
                }
            }

            if (this.SelectedTool != null && !justSelected)
            {
                this.SelectedTool.MouseDown(mousePosition, e);
            }
        }

        private void DrawCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.SelectedTool != null)
            {
                this.SelectedTool.MouseMove(e.GetPosition(this.DrawCanvas), e);
            }
        }

        private void DrawCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.SelectedTool != null)
            {
                this.SelectedTool.MouseUp(e.GetPosition(this.DrawCanvas), e);
            }
        }

        internal void ClearCanvas()
        {
            this.drawables.Clear();
            // delete brush drawn stuff
        }

        internal void AddShape(IDrawable shape)
        {
            if (!this.drawables.Contains(shape))
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

        private void CurrentToolData_PreparePropertyItem(object sender, Xceed.Wpf.Toolkit.PropertyGrid.PropertyItemEventArgs e)
        {
            if (e.PropertyItem.DisplayName.Equals("SelectedShape"))
            {
                e.Handled = false;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //string fileText = "Your output text";

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Png files (*.png)|*.png|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)this.D2DControl.RenderSize.Width, (int)this.D2DControl.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                rtb.Render(this.D2DControl);

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

                using (var fs = File.OpenWrite(dialog.FileName))
                {
                    pngEncoder.Save(fs);
                }
            }
        }

        private void ThemeItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if ((string)item.Header == "_Dark")
            {
                this.Resources["ToolBarBackgroundColor"] = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
                this.Resources["BorderBarColor"] = new SolidColorBrush(Color.FromArgb(255, 34, 34, 34));
                this.Resources["CanvasBackground"] = new SolidColorBrush(Color.FromArgb(255, 68, 68, 68));
                this.Resources["ToolBarButtonPressed"] = new SolidColorBrush(Color.FromArgb(255, 102, 102, 102));
                this.ResetDictionariesForTheme("BaseDark");
            }
            else if ((string)item.Header == "_Light")
            {
                this.Resources["ToolBarBackgroundColor"] = new SolidColorBrush(Color.FromArgb(255, 208, 208, 208));
                this.Resources["BorderBarColor"] = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                this.Resources["CanvasBackground"] = new SolidColorBrush(Color.FromArgb(255, 208, 218, 232));
                this.Resources["ToolBarButtonPressed"] = new SolidColorBrush(Color.FromArgb(255, 224, 224, 224));
                this.ResetDictionariesForTheme("Blue");
            }
        }

        private void ResetDictionariesForTheme(string theme)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.AnimatedSingleRowTabControl.xaml", UriKind.RelativeOrAbsolute)
            });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/" + theme + ".xaml", UriKind.RelativeOrAbsolute)
            });
        }
    }
}
