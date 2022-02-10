using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obem_vibr_sten
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Edge, "Выберите элемент");
            var wallList=new List<Element>();
            foreach (var selectedElement in selectedElementRefList)
            {
                Element element = doc.GetElement(selectedElement);
                if (element is Wall)
                {
                    Wall oWall = (Wall)element;
                    wallList.Add(oWall);
                }
            }
            double obemVibrannihSten=0;
            foreach (Wall wall in wallList)
            {
                Parameter volumeParametr = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                if (volumeParametr.StorageType == StorageType.Double)
                {
                    obemVibrannihSten += volumeParametr.AsDouble();
                }
            }
            double obemVibrannihStenMeter = UnitUtils.ConvertFromInternalUnits(obemVibrannihSten, DisplayUnitType.DUT_CUBIC_METERS);

            TaskDialog.Show ("Объем стен",$"Oбъем выбранных стен равен {obemVibrannihStenMeter}м3");
            return Result.Succeeded;
        }
    }
}
