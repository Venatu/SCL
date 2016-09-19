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
            Document doc = commandData.Application.ActiveUIDocument.Document;

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

            

            foreach(Element e in filteredElements)
            {
                /*
                var analytical = e.GetAnalyticalModel();                
                var materials = e.GetMaterialIds(false);
                var mat = doc.GetElement(materials.ElementAt(0));
                var yield = mat.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_MINIMUM_YIELD_STRESS);
                var temp = yield.AsValueString();
                var youngs = mat.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_YOUNG_MOD);
                temp = youngs.AsValueString();*/

                var structID = e.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM);
                var material = doc.GetElement(structID.AsElementId()) as Material;
                var properties = doc.GetElement(material.StructuralAssetId);

                var yield = properties.get_Parameter(BuiltInParameter.PHY_MATERIAL_PARAM_MINIMUM_YIELD_STRESS).AsValueString();              
                

            }

            return Result.Succeeded;
        }
    }
}
