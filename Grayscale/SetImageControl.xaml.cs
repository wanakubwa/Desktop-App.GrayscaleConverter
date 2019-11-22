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
using System.Data;
using Microsoft.Win32;

namespace Grayscale
{
    /// <summary>
    /// imageControl allows user to choose and upload image to conver.
    /// </summary>
    public partial class SetImageControl : UserControl
    {
        private Image img = null;
        private string imgPath = null;
        private MainWindow mainWindow;

        public SetImageControl(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            SetDefaultStatement();
        }

        private void SetDefaultStatement()
        {
            if (mainWindow.ImageToEdit != null)
                userImage.Source = mainWindow.ImageToEdit.Source;
        }

        private void ImageSetButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog();

            if(imgPath != null)
            {
                ConvertFilePathToImg();

                // Setting choosed img in mainWindow.
                mainWindow.ImageToEdit = img;
                userImage.Source = img.Source;
            }
        }

        /// <summary>
        /// Converting file path to img source.
        /// </summary>
        private void ConvertFilePathToImg()
        {
            // Creating new image.
            img = new Image();

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imgPath, UriKind.Absolute);
            bitmapImage.EndInit();

            // Replace by real source img prv source.
            img.Source = bitmapImage;
        }

        /// <summary>
        /// Opening file dialog that allows user to choose image.
        /// </summary>
        private void OpenFileDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Title = "Browse Image Files",
                CheckFileExists = true,
                CheckPathExists = true,
                RestoreDirectory = true,
                Multiselect = false,
                Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG",
                DefaultExt = ".JPG"
            };

            // Contains information that dialog was opened == true | failed == null
            Nullable<bool> dialogOK = fileDialog.ShowDialog();

            if(dialogOK == true)
            {
                // Set file path of selected file.
                imgPath = fileDialog.FileName;
            }
        }
    }
}
