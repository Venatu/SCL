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
    /// Interaction logic for SteelElement.xaml
    /// </summary>
    public partial class SteelElement : UserControl
    {
        public SteelElement(SteelDesignElement sde)
        {
            InitializeComponent();
            sectionSize.ItemsSource = scip363.GetUKB().Keys;
            this.DataContext = sde;
            sde.AnalysisComplete += Sde_AnalysisComplete;
        }

        private void Sde_AnalysisComplete(SteelDesignElement source, EventArgs e)
        {
            this.DataContext = null;
            this.DataContext = source;
        }
    }
}
