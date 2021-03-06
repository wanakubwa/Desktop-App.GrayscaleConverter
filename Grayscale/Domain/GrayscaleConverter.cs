﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Grayscale.Processing;

namespace Grayscale.Domain
{
    class GrayscaleConverter
    {
        public Image Image { get; set; } = null;
        public bool IsAsm { set; private get; }
        public int ThreadsNum { get; set; }
        public byte[] EditedImgBytes { get; private set; }

        private int _imageStride;
        private WriteableBitmap _bitmap;
        private MyProcessingData _myProcessingData = new MyProcessingData();

        /// <summary>
        /// Converting to bitmap and dividing to vectors.
        /// </summary>
        /// <param name="imageToEdit"></param>
        public void ConvertToGrayscale()
        {
            var bitmapImage = ConvertBitmapSourceToBitmapImage();
            _bitmap = new WriteableBitmap(bitmapImage);

            CalculateImageStrideLength(bitmapImage);
            int arraySize = _imageStride * _bitmap.PixelHeight;
            byte[] pixels = new byte[arraySize];
            _bitmap.CopyPixels(pixels, _imageStride, 0);

            // Creating an instance of DLL menager and setting params.
            _myProcessingData.ThreatsNum = ThreadsNum;
            _myProcessingData.IsAsm = IsAsm;
            _myProcessingData.SplitByteArrayToRegisters(pixels);

            _myProcessingData.RunConversionProcess();
        }

        public BitmapImage GetImageBitmap()
        {
            BitmapImage returnData;

            EditedImgBytes = _myProcessingData.ConvertListToOneByteArray();

            Int32Rect rect = new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
            _bitmap.WritePixels(rect, EditedImgBytes, _imageStride, 0);
            returnData = ConvertWriteableBitmapToBitmapImage(_bitmap);

            return returnData;
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
