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
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(string), typeof(NamedTextBox));
        public static readonly DependencyProperty TextBoxTextProperty = DependencyProperty.Register("TextBoxText", typeof(string), typeof(NamedTextBox));

        public string LabelText
        {
            get
            {
                return (string)this.GetValue(LabelTextProperty);
            }
            set
            {
                this.SetValue(LabelTextProperty, value + ":");
            }
        }

        public string TextBoxText
        {
            get
            {
                return (string)this.GetValue(TextBoxTextProperty);
            }
            set
            {
                this.SetValue(TextBoxTextProperty, value);
            }
        }

        public NamedTextBox()
        {
            InitializeComponent();
        }
    }
}
