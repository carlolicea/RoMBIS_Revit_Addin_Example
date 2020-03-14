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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow _mainWindow;
        internal static UIApplication _uiApp;
        internal static List<Element> _cropElements;

        public MainWindow(UIApplication uiApp)
        {
            InitializeComponent();
            _uiApp = uiApp;
            _mainWindow = this;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _mainWindow = null;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            List<Element> selectedElements = _uiApp.ActiveUIDocument.Selection.PickObjects(ObjectType.Element)
                .Cast<Reference>()
                .Select(r=>_uiApp.ActiveUIDocument.Document.GetElement(r.ElementId))
                .ToList();
            _mainWindow.SelectionListView.ItemsSource = selectedElements.Cast<Element>()
                .Select(el => el.Id.ToString()+" : "+el.Category.Name+" - "+el.Name.ToString())
                .ToList();
            _cropElements = selectedElements;
        }

        private void CropButton_Click(object sender, RoutedEventArgs e)
        {
            RequestHandler._exEvent.Raise();
            RequestHandler.MakeRequest(RequestId.CropViewToElements);
        }
    }
}
