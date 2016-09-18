using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
using Venatu.SCL.AnalysisEngine;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Example2();
        }

        public void Example1()
        {
            Model m = new Model(6, 3, 2, 3, 10, 3);

            m.SetSupport(0, 0, 1, 1);
            m.SetSupport(1, 2, 0, 1);
            m.SetSupport(2, 3, 0, 1);

            m.SetJointCoordinate(0, 0, 0);
            m.SetJointCoordinate(1, 288, 0);
            m.SetJointCoordinate(2, 576, 0);
            m.SetJointCoordinate(3, 864, 0);
            m.SetJointCoordinate(4, 288, 216);
            m.SetJointCoordinate(5, 576, 216);

            m.SetMaterial(0, 29000);
            m.SetMaterial(1, 10000);

            m.SetSection(0, 8);
            m.SetSection(1, 12);
            m.SetSection(2, 16);

            m.SetMember(0, 0, 1, 0, 0);
            m.SetMember(1, 1, 2, 0, 0);
            m.SetMember(2, 2, 3, 1, 2);
            m.SetMember(3, 4, 5, 0, 0);
            m.SetMember(4, 1, 4, 0, 0);
            m.SetMember(5, 2, 5, 0, 0);
            m.SetMember(6, 0, 4, 0, 1);
            m.SetMember(7, 1, 5, 0, 1);
            m.SetMember(8, 2, 4, 0, 1);
            m.SetMember(9, 3, 5, 1, 2);

            m.SetLoad(0, 1, 0, -75);
            m.SetLoad(1, 4, 25, 0);
            m.SetLoad(2, 5, 0, -60);

            m.Calculate();
        }

        public void Example2()
        {
            Model m = new Model(4, 3, 1, 2, 3, 1);

            m.SetJointCoordinate(0, 12 * 12, 16 * 12);
            m.SetJointCoordinate(1, 0, 0);
            m.SetJointCoordinate(2, 12 * 12, 0);
            m.SetJointCoordinate(3, 24 * 12, 0);

            m.SetSupport(0, 1, 1, 1);
            m.SetSupport(1, 2, 1, 1);
            m.SetSupport(2, 3, 1, 1);

            m.SetMaterial(0, 29000);

            m.SetSection(0, 8);
            m.SetSection(1, 6);

            m.SetMember(0, 1, 0, 0, 0);
            m.SetMember(1, 2, 0, 0, 1);
            m.SetMember(2, 3, 0, 0, 0);

            m.SetLoad(0, 0, 150, -300);

            m.Calculate();
        }
    }
}
