
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
    public class ExApplication : IExternalApplication
    {
        internal static UIControlledApplication _uiCtrlApp;
        public Result OnStartup(UIControlledApplication uIControlledApplication)
        { 
            _uiCtrlApp = uIControlledApplication;

            #region Notes
            /* Implement UIControlledApplication event handlers here, such as: 
             * ControlledApplicatoin.ApplicationInitialized, ViewActivated, and ApplicationClosing. 
             * ApplicationInitalized is one of the most important since you can add event handlers for: 
             * DocumentOpened, DocumentCreated, DocumentSaved, DocumentClosed, etc.*/
            #endregion Notes

            this.CreateRibbonButtons();

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication uIControlledApplication)
        {
            return Result.Succeeded;
        }

        private void CreateRibbonButtons()
        {
            System.Windows.Media.ImageSource iconLarge = IconSource(Properties.Resources.RoMBIS_large);
            System.Windows.Media.ImageSource iconSmall = IconSource(Properties.Resources.RoMBIS_small);

            _uiCtrlApp.CreateRibbonTab("RoMBIS");

            PushButtonData pushButtonData = new PushButtonData(name: "MyRevitAddin",
                text: "Run Addin",
                assemblyName: System.Reflection.Assembly.GetExecutingAssembly().Location,
                className: "RevitAddinExample.ExCommand");
            pushButtonData.Image = iconSmall;
            pushButtonData.LargeImage = iconLarge;
            pushButtonData.ToolTip = "This is the tooltip for MyRevitAddin";

            RibbonPanel ribbonPanel = _uiCtrlApp.CreateRibbonPanel(tabName: "RoMBIS", panelName: "Example");
            ribbonPanel.AddItem(pushButtonData);
            
        }

        private System.Windows.Media.ImageSource IconSource(System.Drawing.Bitmap bitmap)
        {
            MemoryStream memoryStream = new MemoryStream();
            //System.Drawing.Imaging.ImageFormat.Png is required for using PNG transparency
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();

            memoryStream.Seek(0, SeekOrigin.Begin);
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();

            return bitmapImage;
        }

    }
}
