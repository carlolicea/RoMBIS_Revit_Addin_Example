using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RevitAddinExample
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class MainWindow : Window
    {
        /// <summary> The instance of MainWindow to be recalled </summary>
        internal static MainWindow _mainWindow;
        /// <summary> UIApplication object for use in Revit API operations </summary>
        internal static UIApplication _uiApp;
        /// <summary> List of Revit elements from the user's selection. The storage is static so it can be recalled from another class. </summary>
        internal static List<Element> _cropElements = new List<Element>();
        /// <summary> Color used for button backgrounds when there is an input error. </summary>
        private static readonly SolidColorBrush errorColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(127, 255, 0, 0));
        /// <summary> Color used for button backgrounds when the input is accepted. </summary>
        private static readonly SolidColorBrush okayColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(127, 0, 255, 0));


        /// <summary> Constructor for MainWindow </summary>
        /// <param name="uiApp"> UIApplication from ExCommand </param>
        public MainWindow(UIApplication uiApp)
        {
            // Create an instance of MainWindow
            InitializeComponent();
            // Set the local UIApplication variable for MainWindow
            _uiApp = uiApp;

            // Check if the list of elements selected has no elements.
            if (_cropElements.Count == 0)
            {
                // If so, set the background to the error color
                _mainWindow.CropButton.Background = errorColor;
            }
        }

        /// <summary> On closed event that occurs when the _mainWindow instance is closed. </summary>
        /// <param name="sender"> The sender of the close event. </param>
        /// <param name="e"> The close EventArgs. </param>
        private void Window_Closed(object sender, EventArgs e)
        {
            //Reset _mainWindow to null;
            _mainWindow = null;
        }

        /// <summary> Performs operations when the SelectButton is clicked. </summary>
        /// <param name="sender"> The sender of the click event. </param>
        /// <param name="e"> The click RoutedEventArgs. </param>
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Notes
                /* Invoke the selection of elements in the Revit UI. 
                 * Note that ? is used after PickElementsByRectangle(). 
                 * This indicates that if PickElementsByRectangle() does not return null, convert it to a list, else return the null value.
                 */
                #endregion Notes

                List<Element> selectedElements = _uiApp.ActiveUIDocument.Selection.PickElementsByRectangle()?.ToList();

                // If the user didn't select anything, selectedElements will be null
                if (selectedElements != null)
                {
                    #region Notes
                    /* Set the data source of the SelectionListView to the selected elements.
                     * Here, LINQ is used to select the elements in the list using a lambda expression to obtain their Id, Category Name, and Type Name to return as a list.
                     */
                    #endregion Notes

                    _mainWindow.SelectionListView.ItemsSource = selectedElements.Cast<Element>()
                                        .Select(el => el.Id.ToString() + " : " + el.Category.Name + " - " + el.Name.ToString())
                                        .ToList();

                    // Set _crop elements to the selected elements for retrieval in the CropViewToElements class
                    _cropElements = selectedElements;
                    
                    // Set the background color of the CropButton to the color indicating an accepted selection.
                    _mainWindow.CropButton.Background = okayColor;

                }
                else
                {
                    //If the selectedElements is null, set the CropButton background to the error color.
                    _mainWindow.CropButton.Background = errorColor;
                }
            }
            catch
            {
                // If something went wrong, clear the _cropElements list.
                _cropElements.Clear();
                // Set the SelectiongListView to _cropElements to clear it out.
                _mainWindow.SelectionListView.ItemsSource = _cropElements;
                // Set the CropButton background to the error color.
                _mainWindow.CropButton.Background = errorColor;
            }
        }

        /// <summary> Performs operations when the CropButton is clicked. </summary>
        /// <param name="sender"> The sender of the click event. </param>
        /// <param name="e"> The click RoutedEventArgs. </param>
        private void CropButton_Click(object sender, RoutedEventArgs e)
        {
            //Check again to ensure the user is in a plan view by checking the view type and seeing if it contains "plan";
            if (!_uiApp.ActiveUIDocument.ActiveView.ViewType.ToString().ToLower().Contains("plan"))
            {
                //If the view is not a plan view, let the user know and return.
                TaskDialog.Show("Notice", "This add-in only works in a plan view");
                return;
            }

            //Otherwise, if the number of elements in the _cropElements list is greater than 0, raise and external event and make a request.
            if (_cropElements.Count > 0)
            {
                RequestHandler._exEvent.Raise();
                RequestHandler.MakeRequest(RequestId.CropViewToElements);
            }
            else
            {
                // If the number of elements in the _cropElements list was not greater than 0, notify the user.
                TaskDialog.Show("Notice", "Select one or more elements prior to cropping.");
            }
        }
    }
}
