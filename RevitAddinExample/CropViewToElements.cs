using Autodesk.Revit.DB;
//Because Document is a class found in some other libraries, it is recommended to assign it to a variable.
using RvtDoc = Autodesk.Revit.DB.Document;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddinExample
{
    /// <summary> Class to perform the CropViewToElements operations. </summary>
    class CropViewToElements
    {
        /// <summary> Performs the operations that affect the Revit db. </summary>
        internal static void PerformOperations()
        {
            // Declare the Document object from the beginning because it's going to be used anyways in a transaction.
            RvtDoc rvtDoc = ExCommand.uiApplication.ActiveUIDocument.Document;
            // Get the current view active in Revit.
            View currentView = rvtDoc.ActiveView as View;

            #region Notes
            /* Getting the maximum and minimum value of all selected elements' bounding boxes.
            * Again, LINQ is being used to first get all items in the _cropElements list and cast them as elements to use in further LINQ operations in the chain.
            * Using the double maxX as a reference:
            * From each element, get the BoundingBox relative to the current view, and then get the Max point's coordinate property.
            * From all of the Max point coordinates, get the Max value using LINQ.
             */
            #endregion Notes

            double maxX = MainWindow._cropElements.Cast<Element>().Select(e => e.get_BoundingBox(currentView).Max.X).Max();
            double maxY = MainWindow._cropElements.Cast<Element>().Select(e => e.get_BoundingBox(currentView).Max.Y).Max();
            double maxZ = MainWindow._cropElements.Cast<Element>().Select(e => e.get_BoundingBox(currentView).Max.Z).Max();
            double minX = MainWindow._cropElements.Cast<Element>().Select(e => e.get_BoundingBox(currentView).Min.X).Min();
            double minY = MainWindow._cropElements.Cast<Element>().Select(e => e.get_BoundingBox(currentView).Min.Y).Min();
            double minZ = MainWindow._cropElements.Cast<Element>().Select(e => e.get_BoundingBox(currentView).Min.Z).Min();

            #region Notes
            /* When performing any operation on the Revit db, a transaction must be performed.
             * Transactions do not have to be placed in a using declarative as shown below, and can simply be declared.
             * The using declarative is often used for organization of the code going on in the transaction, and the transaction is automatically disposed when done.
             * If you want to perform multiple transactions with one undo operation, use TransactionGroup. See https://thebuildingcoder.typepad.com/blog/2015/02/using-transaction-groups.html
             */
            #endregion Notes

            using (Transaction transaction = new Transaction(rvtDoc, "Crop Elevation"))
            {
                // Start the transaction
                transaction.Start();
                // Use a try/catch to ensure a failed transaction gets rolled back if it cannot reach the commit.
                try
                {
                    // NOTE: In plan views, the Z values are ignored in cropping the view, but are required for creation of a bounding box.
                    XYZ maxPoint = new XYZ(maxX, maxY, maxZ);
                    XYZ minPoint = new XYZ(minX, minY, minZ);
                    // Make a new BoundingBox from the min and max points.
                    BoundingBoxXYZ boundingBoxXYZ = new BoundingBoxXYZ() { Max = maxPoint, Min = minPoint };
                    
                    // Set the CropBox of the current view to the new BoundingBox
                    currentView.CropBox = boundingBoxXYZ;
                    // Commit the transaction to finish it off.
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    //If there was an error, let the user know, then roll back the transaction.
                    TaskDialog.Show("Error", ex.Message);
                    transaction.RollBack();
                }
            };            
        }
    }
}
