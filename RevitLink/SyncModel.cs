using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.Attributes;

namespace Venatu.SCL.RevitLink
{
    [TransactionAttribute(TransactionMode.Automatic)]
    class SyncModel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get the current document
            Document doc = BuildModel(commandData.Application.ActiveUIDocument.Document);
            
            //Sync the results back

            return Result.Succeeded;
        }

        public static Document BuildModel(Autodesk.Revit.DB.Document doc)
        {
            BuiltInCategory[] bics = new BuiltInCategory[] {
    BuiltInCategory.OST_StructuralColumns,
    BuiltInCategory.OST_StructuralFraming,
    BuiltInCategory.OST_StructuralFoundation
            };

            IList<ElementFilter> a = new List<ElementFilter>(bics.Count());

            foreach (BuiltInCategory bic in bics)
            {
                a.Add(new ElementCategoryFilter(bic));
            }

            LogicalOrFilter categoryFilter = new LogicalOrFilter(a);

            LogicalAndFilter familyInstanceFilter = new LogicalAndFilter(categoryFilter, new ElementClassFilter(typeof(FamilyInstance)));

            IList<ElementFilter> b = new List<ElementFilter>(6);

            /*b.Add(new ElementClassFilter(
              typeof(Wall)));

            b.Add(new ElementClassFilter(
              typeof(Floor)));

            b.Add(new ElementClassFilter(
              typeof(ContFooting)));

            b.Add(new ElementClassFilter(
              typeof(PointLoad)));

            b.Add(new ElementClassFilter(
              typeof(LineLoad)));

            b.Add(new ElementClassFilter(
              typeof(AreaLoad)));*/

            b.Add(familyInstanceFilter);

            LogicalOrFilter classFilter = new LogicalOrFilter(b);

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.WherePasses(classFilter);
            var filteredElements = collector.ToElements();

            //Build the document ready for analysis
            Venatu.SCL.Document analysisDoc = new Document();

            foreach (Element e in filteredElements)
            {
                StructuralMember member = new StructuralMember(e.UniqueId);

                Curve analyticalCurve = e.GetAnalyticalModel().GetCurve();
                var start = analyticalCurve.GetEndPoint(0);
                var end = analyticalCurve.GetEndPoint(1);

                Joint startJoint = new Joint();
                startJoint.X = start.X;
                startJoint.Y = start.Y;
                startJoint.Z = start.Z;

                Joint endJoint = new Joint();
                endJoint.X = end.X;
                endJoint.Y = end.Y;
                endJoint.Z = end.Z;

                analysisDoc.AddMember(member, startJoint, endJoint);

                /*var structID = e.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM);
                var material = doc.GetElement(structID.AsElementId()) as Material;
                var properties = doc.GetElement(material.StructuralAssetId);                

                var yield = properties.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_MINIMUM_YIELD_STRESS).AsValueString();

                var type = doc.GetElement(e.GetTypeId());
                var webThickness = type.get_Parameter(BuiltInParameter.STRUCTURAL_SECTION_ISHAPE_FLANGETHICKNESS).AsValueString();  */
            }

            return analysisDoc;
        }
    }
}
