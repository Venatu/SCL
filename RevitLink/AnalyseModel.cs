using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;

namespace Venatu.SCL.RevitLink
{
    [TransactionAttribute(TransactionMode.Automatic)]
    class AnalyseModel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the current document
            Document doc = SyncModel.BuildModel(commandData.Application.ActiveUIDocument.Document);

            //Analyse the document
            doc.Calculate(AnalysisType.Gravity);

            //Sync the results back


            return Result.Succeeded;
        }
    }
}
