
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RevitAddinExample
{
    /// <summary> Implements IExternalApplication </summary>
    /// <remarks> Part of the Revit API for external applications to implement external commands </remarks>
    public class ExApplication : IExternalApplication
    {
        /// <summary> Static UIControlledApplication to be used in successive calls. </summary>
        internal static UIControlledApplication _uiCtrlApp;

        /// <summary> Runs processes when Revit starts. </summary>
        /// <param name="uIControlledApplication"> A UIControlledApplication object. </param>
        /// <remarks> Part of the Revit API to setup processes when Revit starts. </remarks>
        /// <returns> Result.Succeeded if successful, else Result.Failed. </returns>
        public Result OnStartup(UIControlledApplication uIControlledApplication)
        { 
            _uiCtrlApp = uIControlledApplication;

            #region Notes
            /* Implement UIControlledApplication event handlers here, such as: 
             * ControlledApplicatoin.ApplicationInitialized, ViewActivated, and ApplicationClosing. 
             * ApplicationInitalized is one of the most important since you can add event handlers for: 
             * DocumentOpened, DocumentCreated, DocumentSaved, DocumentClosed, etc.*/
            #endregion Notes

            // Create the Ribbon buttons for the add-in on startup.
            this.CreateRibbonButtons();

            return Result.Succeeded;
        }

        /// <summary> Runs processes when Revit shuts down. </summary>
        /// <param name="uIControlledApplication"> A UIControlledApplication object </param>
        /// <remarks> Part of the Revit API to finish processes when Revit shuts down. </remarks>
        /// <returns> Result.Succeeded if successful, else Result.Failed. </returns>
        public Result OnShutdown(UIControlledApplication uIControlledApplication)
        {
            // Add operations here if processes need performed.
            return Result.Succeeded;
        }

        /// <summary> Creates the Revit Ribbon buttons for the add-in </summary>
        private void CreateRibbonButtons()
        {
            // ImageSource objects created from images used for the Ribbon button graphics.
            System.Windows.Media.ImageSource iconLarge = IconSource(Properties.Resources.RoMBIS_large);
            System.Windows.Media.ImageSource iconSmall = IconSource(Properties.Resources.RoMBIS_small);

            #region Notes
            /* With the UIControlledApplication, call CreateRibbonTab to give the add-in its own tab on the Ribbon.
             * Note: Below at "RibbonPanel ribbonPanel = _uiCtrlApp.CreateRibbonPanel(tabName: "RoMBIS", panelName:"Example")", 
             * if the tabName parameter is excluded, the panel and button will be added to the Add-ins tab instead of creating a new tab.
             * If you want to add the button to the Add-in tab, exclude the tabName parameter from CreateRibbonPanel,
             * and delete the line following this note block that calls CreateRibbonTab().*/
            #endregion Notes

            _uiCtrlApp.CreateRibbonTab("RoMBIS");

            #region Notes
            /* Create a PushButtonData object that defines the add-in's button. 
             * assemblyName uses reflection to identify from where the add-in is being called, and from which dll.
             * className must match the namespace and class that implements IExternalCommand. 
             * Note that if you change the name of the class that implements IExternalCommand, you have to manually change it here too. */
            #endregion Notes

            PushButtonData pushButtonData = new PushButtonData(name: "MyRevitAddin",
                text: "Run Addin",
                assemblyName: System.Reflection.Assembly.GetExecutingAssembly().Location,
                className: "RevitAddinExample.ExCommand");
            pushButtonData.Image = iconSmall;
            pushButtonData.LargeImage = iconLarge;
            pushButtonData.ToolTip = "This is the tooltip for MyRevitAddin";

            // Create a RibbonPanel object that will add itself to the tab named "RoMBIS" defined above, and call the panel "Example".
            RibbonPanel ribbonPanel = _uiCtrlApp.CreateRibbonPanel(tabName: "RoMBIS", panelName: "Example");

            // Add the button defined above to the panel.
            ribbonPanel.AddItem(pushButtonData);
            
        }

        /// <summary> Creates an ImageSource object by reading an image into a MemoryStream. </summary>
        /// <param name="bitmap"> The image to convert to an ImageSource. </param>
        /// <returns> An ImageSource object </returns>
        private System.Windows.Media.ImageSource IconSource(System.Drawing.Bitmap bitmap)
        {
            //Create a new memory stream.
            MemoryStream memoryStream = new MemoryStream();

            #region Notes
            /* Save() loads the image into the MemoryStream.
            * System.Drawing.Imaging.ImageFormat.Png is required for using PNG transparency.*/
            #endregion Notes

            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

            //Create a new BitMapImage to write the MemoryStream to.
            BitmapImage bitmapImage = new BitmapImage();
            
            //Initialize the BitmapImage
            bitmapImage.BeginInit();
            //Set the MemoryStream to begin reading from the beginning.
            memoryStream.Seek(0, SeekOrigin.Begin);
            //Set the StreamSource of the BitmapImage to the MemoryStream.
            bitmapImage.StreamSource = memoryStream;
            //End initialization of the BitmapImage
            bitmapImage.EndInit();

            //Return the BitmapImage.
            return bitmapImage;
        }
    }
}
