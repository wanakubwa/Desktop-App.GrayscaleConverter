﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grayscale.Events;
using System.Diagnostics;
using System.Runtime.InteropServices;

// Include cpp dll functions.
using GrayscaleCppManager;

namespace Grayscale.Processing
{
    class ThreadsManager
    {
        [DllImport("ASMDLL.dll")]
        public static unsafe extern void doRegisterGrayASM(IntPtr ptr);

        bool _isAsm;
        static int _threadsCompleated = 0;
        List<Thread> _threads = new List<Thread>();

        public int ThreadsNum { get; set; }
        static readonly object _padLockIndex = new object();
        static readonly object _padLockCounter = new object();
        static int _nextIndex = 0;

        public ThreadsManager(bool isAsm)
        {
            this._isAsm = isAsm;
        }

        public void SetDefaultState()
        {
            _nextIndex = 0;
            _threadsCompleated = 0;
        }

        private static void IncrementEndThreads()
        {
            lock (_padLockCounter)
            {
                _threadsCompleated++;
            }
        }

        private static int GetNextIndex()
        {
            lock (_padLockIndex)
            {
                _nextIndex++;
                return _nextIndex;
            }
        }

        public void RunThreadProcess(ref List<byte[]> pixelsListToDo)
        {
            if (_isAsm)
            {
                // TODO: split into two functions for ASM and CPP.
                for (int i = 0; i < ThreadsNum; i++)
                {
                    var tmp = new Thread(DoCppJob);
                    _threads.Add(tmp);
                }
            }
            else
            {
                // TODO: split into two functions for ASM and CPP.
                for (int i = 0; i < ThreadsNum; i++)
                {
                    var tmp = new Thread(DoAsmJob);
                    _threads.Add(tmp);
                }
            }


            foreach (var element in _threads)
            {
                element.Start(pixelsListToDo);
                element.Join();
            }

            while (_threadsCompleated != ThreadsNum)
            {
                
            }

            ProgramEventsSystem.CallImageProcessingClosed();
        }

        private static void DoCppJob(object parameter)
        {
            List<byte[]> pixelList = parameter as List<byte[]>;
            var index = GetNextIndex();

            GrayscaleConverterCpp grayscaleConverter = new GrayscaleConverterCpp();
            while (index < pixelList.Count)
            {
                // Cpp call function from dll.
                grayscaleConverter.MakeGrayScaleAtOneRegisterCpp(pixelList[index]);
                index = GetNextIndex();
            }
            IncrementEndThreads();
        }

        private static void DoAsmJob(object parameter)
        {
            List<byte[]> pixelList = parameter as List<byte[]>;
            var index = GetNextIndex();

            while (index < pixelList.Count)
            {
                // ASM call function from dll.
                unsafe
                {
                    byte[] srcArray = pixelList[index];
                    fixed (byte* p = srcArray)
                    {
                        IntPtr ptr = (IntPtr)p;
                        doRegisterGrayASM(ptr);
                    }
                }
                index = GetNextIndex();
            }
            IncrementEndThreads();
        }

        private void ThreadCompleatedHandler()
        {
            _threadsCompleated++;
            
            if (_threadsCompleated == ThreadsNum)
            {
                ProgramEventsSystem.CallImageProcessingClosed();
            }
        }
    }
}