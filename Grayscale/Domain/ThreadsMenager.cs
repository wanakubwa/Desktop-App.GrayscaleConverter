using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using GrayscaleCppManager;

namespace Grayscale.ThreadsMenager
{
    class ThreadsMenager
    {
        public int ThreatsNum { get; set; } = 0;
        int _arraySize;
        int _addedElements;
        List<Thread> _threads = new List<Thread>();
        List<byte[]> _pixelsList = new List<byte[]>();

        public void SplitByteArrayToRegisters(byte[] array)
        {
            // Size of oryginal byte array.
            _arraySize = array.Length;

            // Max vector size in bits. (128-bit register)
            //var vectSize = Vector<byte>.Count;
            var regSize = 16;

            // Spliting oryginal vector to chunks by 128-bit each.
            int i = 0;
            for(i = 0; i< array.Length - regSize; i += regSize)
            {
                var rTmp = new byte[regSize];
                Array.Copy(array, i, rTmp, 0, regSize);
                _pixelsList.Add(rTmp);

                //var vTmp = new Vector<byte>(array, i);
                //_pixelsList.Add(vTmp);
            }

            // Last pixels add to special 128-bit register filled by 0.
            if(i != array.Length)
            {
                _addedElements = array.Length - i;
                byte[] subArray = new byte[16];
                for (int x = 0; x < subArray.Length; x++)
                {
                    subArray[x] = 0;
                }
                for(int x = 0; (x+i)<array.Length; x++)
                {
                    subArray[x] = array[i + x];
                }

                //Vector<byte> vRest = new Vector<byte>(subArray);
                //_pixelsList.Add(vRest);

                _pixelsList.Add(subArray);
            }
        }
        public byte[] ConvertListToOneByteArray()
        {
            var vectSize = Vector<byte>.Count;

            var regSize = 16;
            byte[] returnData = new byte[_arraySize];

            // Setting last element.
            var lastEl = _pixelsList.Last();

            int i = 0;
            foreach(var element in _pixelsList)
            {
                if(i < _arraySize - regSize)
                {
                    element.CopyTo(returnData, i);
                    i += vectSize;
                }
            }

            // Erasing '0' from end of last vector from list
            // This '0''s was added manualy to fill last 128-bit register correctly.
            for(int x = 0; x < _addedElements; x++)
            {
                returnData[x + i] = lastEl[vectSize -1 - x];
            }

            return returnData;
        }

        /// <summary>
        /// Unsafe because using pointers are necessary.
        /// </summary>
        public unsafe void CreateThreadsArray()
        {
            // Prototype testing the DLL caling function and return.
            GrayscaleConverterCpp converterCpp = new GrayscaleConverterCpp();

            for (int i = 0; i < _pixelsList.Count; i++)
            {
                converterCpp.MakeGrayScaleAtOneRegisterCpp(_pixelsList[i], 16);
                //_pixelsList[i] = MakeGrayScale(_pixelsList[i]);
            }
        }

        /// <summary>
        /// Spliting 128-bit register to R.G.B channels and return new vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private unsafe Vector<byte> MakeGrayScale(Vector<byte> vector)
        {
            byte[] ar = new byte[16] { 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 };
            byte[] ag = new byte[16] { 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0 };
            byte[] ab = new byte[16] { 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0 };

            // Consts arrays usign to multiply.
            float rConst = 0.29891f;
            float gConst = 0.58661f;
            float bConst = 0.11448f;

            // Creating registers from byte arrays.
            Vector<byte> vr = new Vector<byte>(ar, 0);
            Vector<byte> vg = new Vector<byte>(ag, 0);
            Vector<byte> vb = new Vector<byte>(ab, 0);

            // Writing R,G,B values to registers.
            vr = vr * vector;
            vg = vg * vector;
            vb = vb * vector;

            Vector<byte> ret = (vr + vg + vb);
            return ret;
        }
        private byte MakeGrayValueForPixels(byte blue, byte green, byte red)
        {
            byte grayValue = (byte)((red + green + blue) / 3);
            return grayValue;
        }
    }
}
