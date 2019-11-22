using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Grayscale.Domain
{
    class GrayscaleConverter
    {
        public Image Image { get; set; } = null;
        private int _imageStride;

        /// <summary>
        /// Converting to bitmap and dividing to vectors.
        /// </summary>
        /// <param name="imageToEdit"></param>
        public BitmapImage ConvertToGrayscale()
        {
            // TODO throw exception if image== null

            var bitmapImage = ConvertBitmapSourceToBitmapImage();
            var bitmap = new WriteableBitmap(bitmapImage);
            BitmapImage returnData;

            CalculateImageStrideLength(bitmapImage);
            int arraySize = _imageStride * bitmap.PixelHeight;
            byte[] pixels = new byte[arraySize];
            bitmap.CopyPixels(pixels, _imageStride, 0);

            //// Converting to grayscale algorithm.
            //int j = 0;
            //for (int i = 0; i < pixels.Length / 4; i++)
            //{
            //    var grayValue = MakeGrayValueForPixels(pixels[j], pixels[j + 1], pixels[j + 2]);
            //    pixels[j] = grayValue;
            //    pixels[j + 1] = grayValue;
            //    pixels[j + 2] = grayValue;
            //    pixels[j + 3] = pixels[j + 3];
            //    j += 4;
            //}

            ThreadsMenager.ThreadsMenager threadsMenager = new ThreadsMenager.ThreadsMenager();
            threadsMenager.ThreatsNum = 12;
            threadsMenager.SplitByteArrayToVectors(pixels);
            threadsMenager.CreateThreadsArray();

            byte[] test = threadsMenager.ConvertVectorToByteArray();

            //// Converting byte array to bitmapImage and return.
            //Int32Rect rect = new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
            //bitmap.WritePixels(rect, pixels, _imageStride, 0);
            //returnData = ConvertWriteableBitmapToBitmapImage(bitmap);

            Int32Rect rect = new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
            bitmap.WritePixels(rect, test, _imageStride, 0);
            returnData = ConvertWriteableBitmapToBitmapImage(bitmap);

            return returnData;
        }
        public byte MakeGrayValueForPixels(byte blue, byte green, byte red)
        {
            byte grayValue = (byte)((red + green + blue) / 3);
            return grayValue;
        }
        private void CalculateImageStrideLength(BitmapImage bitmap)
        {
            var width = bitmap.PixelWidth;
            var height = bitmap.PixelHeight;

            // Stride length for current image.
            _imageStride = width * ((bitmap.Format.BitsPerPixel + 7) / 8);
        }
        private BitmapImage ConvertWriteableBitmapToBitmapImage(WriteableBitmap wbm)
        {
            BitmapImage bmImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(wbm));
                encoder.Save(stream);
                bmImage.BeginInit();
                bmImage.CacheOption = BitmapCacheOption.OnLoad;
                bmImage.StreamSource = stream;

                // Rewind the stream its require for ie. JpegConverter
                stream.Seek(0, SeekOrigin.Begin);
                bmImage.EndInit();
                bmImage.Freeze();
            }
            return bmImage;
        }
        private BitmapImage ConvertBitmapSourceToBitmapImage()
        {
            BitmapSource bitmapSource = Image.Source as BitmapSource;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bitmapImage = new BitmapImage();

            // Changing format if its not 4x4bits per single pixel.
            if (bitmapSource.Format != PixelFormats.Rgba64)
                bitmapSource = new FormatConvertedBitmap(bitmapSource, PixelFormats.Rgba64, null, 0);

            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            // Setting memory stream to start postion.
            memoryStream.Position = 0;
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());
            bitmapImage.EndInit();

            bitmapImage.Freeze();
            return bitmapImage;
        }
    }
}
