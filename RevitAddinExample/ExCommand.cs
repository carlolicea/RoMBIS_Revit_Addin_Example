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
    /// <summary> Implements IExternalCommand to perform an operation from the Revit thread. </summary>
    /// <remarks> The Transaction and Regeneration attributes are required per the Revit API. Both are set to Manual (preferable) so they can be handled as needed in the API calls. </remarks>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExCommand : IExternalCommand
    {
        /// <summary> A UIApplication object to store information about the UIAppliction for later use. </summary>
        internal static UIApplication uIApplication;

        /// <summary> Executes an external command </summary>
        /// <param name="externalCommandData"> ExternalCommandData supplied by Revit. User does not provide this. </param>
        /// <param name="message"> A message string. User does not provide this. </param>
        /// <param name="elementSet"> An ElementSet. User does not provide this. </param>
        /// <remarks> Required per the Revit API for implementing IExternalCommand. </remarks>
        /// <returns> Result.Succeeded if successful, else Result.Failed. </returns>
        public Result Execute(ExternalCommandData externalCommandData, ref string message, ElementSet elementSet)
        {
            // From the externalCommandData, get the active UIApplication.
            uIApplication = externalCommandData.Application;

            // Try to perform the commands enclosed. 
            try
            {
                // If the MainWindow._mainWindow object is not null, bring it into view when the button is clicked.
                if(MainWindow._mainWindow!=null)
                {
                    MainWindow._mainWindow.BringIntoView();
                }
                else
                {
                    //Otherwise, the _mainWindow has not been displayed/created, so make one and show it.
                    MainWindow._mainWindow = new MainWindow(uIApplication);
                    MainWindow._mainWindow.Show();
                }
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                // If an exception is thrown, collect it and display a TaskDialog with the error message.
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }
        }
    }
}
