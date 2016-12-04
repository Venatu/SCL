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
using Venatu.SCL;
using Venatu.SCL.AnalysisEngine;
using Venatu.SCL.UI;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Projects = new List<Project>();

            InitializeComponent();

            BuildTree();     
        }

        public List<Project> Projects;

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            Project p = new Project();
            CreateTabItem(p);

            Projects.Add(p);

            BuildTree();
        }

        private void BuildTree()
        {
            while (treeOutline.Items.Count > 0)
            {
                treeOutline.Items.RemoveAt(0);
            }

            foreach(Project p in Projects)
            {
                //Bind to the porjects properties
                TreeViewItem treeItem = new TreeViewItem();
                Binding projectBinding = new Binding();
                projectBinding.Source = p;
                projectBinding.Mode = BindingMode.TwoWay;
                projectBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                projectBinding.Path = new PropertyPath("ProjectName");
                BindingOperations.SetBinding(treeItem, TreeViewItem.HeaderProperty, projectBinding);

                treeItem.MouseDoubleClick += new MouseButtonEventHandler(delegate (Object o, MouseButtonEventArgs a)
                {
                    if (((TreeViewItem)o).IsSelected)
                    {
                        CreateTabItem(p);
                    }
                });

                //Iterate through its revisions
                foreach (Revision r in p.Revisions)
                {
                    TreeViewItem revisionItem = new TreeViewItem();
                    revisionItem.MouseDoubleClick += new MouseButtonEventHandler(delegate (Object o, MouseButtonEventArgs a)
                    {
                        if (((TreeViewItem)o).IsSelected)
                        {
                            CreateTabItem(r);
                        }
                    });
                    Binding revisionBinding = new Binding();
                    revisionBinding.Source = r;
                    revisionBinding.Mode = BindingMode.TwoWay;
                    revisionBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    revisionBinding.Path = new PropertyPath("Name");
                    BindingOperations.SetBinding(revisionItem, TreeViewItem.HeaderProperty, revisionBinding);

                    foreach (Option o in r.AnalysisObjects)
                    {
                        TreeViewItem optionItem = new TreeViewItem();
                        Binding optionBinding = new Binding();
                        optionBinding.Source = p;
                        optionBinding.Mode = BindingMode.TwoWay;
                        optionBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                        optionBinding.Path = new PropertyPath("Name");
                        BindingOperations.SetBinding(optionItem, TreeViewItem.HeaderProperty, optionBinding);

                        revisionItem.Items.Add(optionItem);
                    }

                    treeItem.Items.Add(revisionItem);
                }

                treeOutline.Items.Add(treeItem);
            }
        }

        private void CreateTabItem(object item)
        {
            TabItem tb = new TabItem();

            Binding headerBinding = new Binding();
            headerBinding.Source = item;
            headerBinding.Mode = BindingMode.TwoWay;
            headerBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;            

            if (item is Project)
            {
                headerBinding.Path = new PropertyPath("ProjectName");
                ProjectControl pc = new ProjectControl();
                pc.DataContext = item;
                tb.Content = pc;
            }
            if (item is Revision)
            {
                headerBinding.Path = new PropertyPath("Name");
                RevisionControl pc = new RevisionControl();
                pc.DataContext = item;
                tb.Content = pc;
            }
            if (item is Option)
            {
                headerBinding.Path = new PropertyPath("Name");
                OptionControl pc = new OptionControl();
                pc.DataContext = item;
                tb.Content = pc;
            }

            BindingOperations.SetBinding(tb, TabItem.HeaderProperty, headerBinding);
            detailView.Items.Add(tb);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Save on exit
        }
    }
}
