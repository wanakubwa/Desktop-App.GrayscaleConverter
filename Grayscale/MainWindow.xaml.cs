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

namespace Grayscale
{
    /// <summary>
    /// App logic class.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Conatins user img used to display img and to convert to gray scale.
        /// </summary>
        public Image ImageToEdit { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();
            SetDefaultStatement();
        }

        /// <summary>
        /// Set default parameters on main window.
        /// </summary>
        private void SetDefaultStatement()
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new AboutUserControl());
        }

        /// <summary>
        /// Reaction when the user presses one of the available side menu options.
        /// </summary>
        /// <param name="sender">Object that call event</param>
        /// <param name="e"></param>
        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var menuIndex = MenuListView.SelectedIndex;

            MenuSliderTransition.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, 70 + (60 * menuIndex), 0, 0);

            switch (menuIndex)
            {
                case 0:
                    ContentGrid.Children.Clear();
                    ContentGrid.Children.Add(new AboutUserControl());
                    break;
                case 1:
                    ContentGrid.Children.Clear();
                    ContentGrid.Children.Add(new SetImageControl(this));
                    break;
                case 2:
                    ContentGrid.Children.Clear();
                    ContentGrid.Children.Add(new ConvertControl(this));
                    break;
                default:
                    break;
            }
        }
    }
}
