using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddinExample
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExCommand : IExternalCommand
    {
        internal static MainWindow _mainWindow;

        internal static UIApplication uIApplication;
        public Result Execute(ExternalCommandData externalCommandData, ref string message, ElementSet elementSet)
        {
             uIApplication = externalCommandData.Application;
            try
            {
                if(_mainWindow!=null)
                {
                    _mainWindow.BringIntoView();
                }
                else
                {
                    _mainWindow = new MainWindow(uIApplication);
                    _mainWindow.Show();
                }
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
        }
    }
}
