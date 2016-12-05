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

namespace Venatu.SCL.UI
{
    /// <summary>
    /// Interaction logic for ProjectControl.xaml
    /// </summary>
    public partial class OptionControl : UserControl
    {
        public OptionControl()
        {
            InitializeComponent();

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var designElement = (IDesignElement)((ComboBox)sender).SelectedItem;

            if(designElement is SteelDesignElement)
            {
                placeholder.Children.Clear();
                SteelElement se = new SteelElement(designElement as SteelDesignElement);
                placeholder.Children.Add(se);
            }
        }

        private void comboBox_DropDownOpened(object sender, EventArgs e)
        {
            var bindingExpression = options.GetBindingExpression(ComboBox.ItemsSourceProperty);
            //bindingExpression.UpdateSource();
            var obj = this.DataContext;
            this.DataContext = null;
            this.DataContext = obj;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ((Option)this.DataContext).Elements.Add(new SteelDesignElement());
        }
    }
}
