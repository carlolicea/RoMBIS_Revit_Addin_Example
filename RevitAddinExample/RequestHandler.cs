using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddinExample
{
    /// <summary> Implements an IExternalEventHandler for handling events. </summary>
    class RequestHandler : IExternalEventHandler
    {
        /// <summary> A RequestHandler object to be recalled. </summary>
        internal static RequestHandler _handler;
        /// <summary> An ExternalEvent object to be recalled. </summary>
        internal static ExternalEvent _exEvent;
        /// <summary> A Request object to be recalled. </summary>
        internal Request _request = new Request();


        internal RequestHandler()
        {
            //Empty constructor. Stuff does not need initialized.
        }

        /// <summary> Returns the current Request </summary>
        internal Request Request
        {
            get { return _request; }
        }

        /// <summary> Gets the name of the RequestHandler. </summary>
        /// <remarks> Required by the Revit API for implementing IExternalEventHandler. </remarks>
        /// <returns> An name used to track what called the external event. Functionally not important for most development but useful in debugging/logging. </returns>
        public string GetName()
        {
            return "RevitAddinExample";
        }

        /// <summary> Performs a specified operation while making a request to the Revit thread. </summary>
        /// <param name="uiApp"> Required UIApplication object, which may or may not be used. </param>
        /// <remarks> Required by the Revit API for implementing IExternalEventHandler. </remarks>
        public void Execute(UIApplication uiApp)
        {
            // Try to perform the supplied request operation.
            try
            {
                switch(Request.Take())
                {
                    case RequestId.None:
                        break;
                    case RequestId.CropViewToElements:
                        //Call the method that will interact with the Revit db.
                        CropViewToElements.PerformOperations();
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                // Catch the exception and show a TaskDialog with the error if something goes wrong.
                TaskDialog.Show("ERROR", ex.Message);
            }
            finally
            {
                // Since the _mainWindow is disabled when a MakeRequest() is called, ensure it is re-enabled here.
                if(MainWindow._mainWindow != null)
                {
                    MainWindow._mainWindow.IsEnabled = true;
                }
            }
        }

        /// <summary> Makes the request to the Revit thread to perform an operation. </summary>
        /// <param name="requestId"> The enum RequestId defined in Request.cs </param>
        internal static void MakeRequest(RequestId requestId)
        {
            // Make a request and raise an external event.
            _handler.Request.Make(requestId);
            _exEvent.Raise();

            // Disable the _mainWindow if it exists so the user cannot interact with it and call operations while an operation is being performed.
            if(MainWindow._mainWindow !=null)
            {
                MainWindow._mainWindow.IsEnabled = false;
            }
        }
    }
}
