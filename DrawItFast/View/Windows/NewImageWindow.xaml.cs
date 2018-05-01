using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DrawItFast.View.Windows
{
    /// <summary>
    /// Interaction logic for NewImageWindow.xaml
    /// </summary>
    public partial class NewImageWindow : Window
    {
        public NewImageWindow()
        {
            InitializeComponent();

            this.WidthInput.PreviewTextInput += ValidateDimensions;
            this.HeightInput.PreviewTextInput += ValidateDimensions;
        }

        private void ValidateDimensions(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            int width;
            int height;
            if(int.TryParse(this.WidthInput.TextBox, out width) && int.TryParse(this.HeightInput.TextBox, out height))
            {
                MainWindow.Instance.ImageWidth = width;
                MainWindow.Instance.ImageHeight = height;
                this.Close();
            }
            else
            {
                MessageBox.Show("Only numbers are allowed for dimensions.");
            }
        }
    }
}
