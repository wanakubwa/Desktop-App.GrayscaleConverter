using Grayscale.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using Grayscale.Events;

namespace Grayscale
{
    /// <summary>
    /// Clas converting input source to grayscale img.
    /// </summary>
    public partial class ConvertControl : UserControl
    {
        private int _choosedThrNum;
        private bool _isAsm = false;
        MainWindow _mainWindow;
        GrayscaleConverter _grayscaleConverter;
        DispatcherTimer _timer;
        System.Diagnostics.Stopwatch _watch;

        public ConvertControl(MainWindow mainWindow)
        {
            this._mainWindow = mainWindow;
            InitializeComponent();
            SetDefaultStatement();

            _grayscaleConverter = new GrayscaleConverter();
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        private void SetDefaultStatement()
        {
            //Checking how much logical threats are avaible in current platform.
            _choosedThrNum = Environment.ProcessorCount;
            threadsSlider.Minimum = 1;
            threadsSlider.Maximum = 64;
            threadsSlider.Value = _choosedThrNum;
            sliderText.Text = _choosedThrNum.ToString();

            if (_mainWindow.ImageToEdit != null)
                originalImg.Source = _mainWindow.ImageToEdit.Source;

            ProgramEventsSystem.EndImageProcessingHandler += EndImageProcessing_Handler;
        }

        private void CheckBoxC_Checked(object sender, RoutedEventArgs e)
        {
            if(checkBoxASM !=null)
                checkBoxASM.IsChecked = false;
            _isAsm = false;
        }

        private void CheckBoxASM_Checked(object sender, RoutedEventArgs e)
        {
            if(checkBoxC != null)
                checkBoxC.IsChecked = false;
            _isAsm = true;
        }

        private void ThreadsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(sliderText != null && threadsSlider != null)
            {
                _choosedThrNum = (int)threadsSlider.Value;
                sliderText.Text = _choosedThrNum.ToString();
            }
        }

        /// <summary>
        /// Converting choosed by user normal source to grayscale source.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            if(_mainWindow.ImageToEdit != null)
            {
                _grayscaleConverter.Image = _mainWindow.ImageToEdit;

                _watch = System.Diagnostics.Stopwatch.StartNew();

                _grayscaleConverter.IsAsm = _isAsm;
                _grayscaleConverter.ConvertToGrayscale();
            }
        }

        private void EndImageProcessing_Handler()
        {
            var tmp = _grayscaleConverter.GetImageBitmap();
            _watch.Stop();
            var elapsedMs = _watch.ElapsedMilliseconds;
            editedImg.Source = tmp;
            timeLabel.Text = elapsedMs.ToString() + " ms";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
