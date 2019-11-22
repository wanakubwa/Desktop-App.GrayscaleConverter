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

namespace Grayscale.ThreadsMenager
{
    class ThreadsMenager
    {
        public int ThreatsNum { get; set; } = 0;
        int _arraySize;
        int _addedElements;
        List<Thread> _threads = new List<Thread>();
        List<Vector<byte>> _pixelsList = new List<Vector<byte>>();

        [DllImport("Grayscale_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void AddNumbers(int a, int b, int* summ);

        public void SplitByteArrayToVectors(byte[] array)
        {
            _arraySize = array.Length;

            // Max vector size in bits. (128-bit register)
            var vectSize = Vector<byte>.Count;

            // Spliting oryginal vector to chunks by 128-bit each.
            int i = 0;
            for(i = 0; i< array.Length - vectSize; i += vectSize)
            {
                var vTmp = new Vector<byte>(array, i);
                _pixelsList.Add(vTmp);
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

                Vector<byte> vRest = new Vector<byte>(subArray);
                _pixelsList.Add(vRest);
            }
        }
        public byte[] ConvertVectorToByteArray()
        {
            var vectSize = Vector<byte>.Count;
            byte[] returnData = new byte[_arraySize];

            // Setting last element.
            var lastEl = _pixelsList.Last();

            int i = 0;
            foreach(var element in _pixelsList)
            {
                if(i < _arraySize - vectSize)
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
        public unsafe void CreateThreadsArray()
        {
            int* dupsko = (int*)GetDupsko();
            AddNumbers(5, 7, dupsko);
            int check = (*dupsko);

            for (int i = 0; i < _pixelsList.Count; i++)
            {
                _pixelsList[i] = MakeGrayScale(_pixelsList[i]);
            }
        }

        private static unsafe int GetDupsko()
        {
            return new int();
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
