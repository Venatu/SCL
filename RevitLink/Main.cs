using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Venatu.SCL.RevitLink
{
    class Main : IExternalApplication
    {
        public static string AssemblyDirectory
        {
            get
            {                
                return Assembly.GetExecutingAssembly().Location;
                /*string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path + "RevitLink.dll");*/
            }
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            String tabName = "Structure Compute Library";
            application.CreateRibbonTab(tabName);

            // Create two push buttons
            PushButtonData button1 = new PushButtonData("SyncButton", "Sync Model",
                AssemblyDirectory, "Venatu.SCL.RevitLink.SyncModel");
            PushButtonData button2 = new PushButtonData("Button2", "My Button #2",
                AssemblyDirectory, "Venatu.SCL.RevitLink.SyncModel");

            // Create a ribbon panel
            RibbonPanel m_projectPanel = application.CreateRibbonPanel(tabName, "This Panel Name");

            // Add the buttons to the panel
            List<RibbonItem> projectButtons = new List<RibbonItem>();
            projectButtons.AddRange(m_projectPanel.AddStackedItems(button1, button2));

            return Result.Succeeded;
        }

        
    }
}
