using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DrawItFast.View.Controls
{
    /// <summary>
    /// Interaction logic for NamedTextBox.xaml
    /// </summary>
    public partial class NamedTextBox : UserControl
    {
        string LocalLabel = "";

        public string Label
        {
            get { return LocalLabel; }
            set
            {
                LocalLabel = value;
                BaseLabel.Content = value;
            }
        }

        public string TextBox
        {
            get { return BaseTextBox.Text; }
            set
            {
                BaseTextBox.Text = value;
            }
        }

        public NamedTextBox()
        {
            InitializeComponent();
        }
    }
}
