using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GrayscaleCppManager;
using Grayscale.Events;

namespace Grayscale.Processing
{
    class ThreadsManager
    {
        bool _isAsm;
        List<Thread> _threads = new List<Thread>();
        static Queue<CppWorker> _cppWorkers = new Queue<CppWorker>();
        public int ThreadsNum { get; set; }
        static readonly object _object = new object();

        public ThreadsManager(bool isAsm)
        {
            this._isAsm = isAsm;
            _cppWorkers.Clear();
        }

        /// <summary>
        /// initializing queue stack with workers.
        /// </summary>
        public void InitializeCppWorkersStack()
        {
            for(int i = 0; i< ThreadsNum; i++)
            {
                CppWorker cppWorker = new CppWorker(i);
                cppWorker.IsFree = true;
                _cppWorkers.Enqueue(cppWorker);
            }
        }

        /// <summary>
        /// Adding to queue free worker to enable using it againg.
        /// </summary>
        /// <param name="worker"> Worker to add to stack.</param>
        static void  AddToQueue(CppWorker worker)
        {
            lock (_object)
            {
                _cppWorkers.Enqueue(worker);
            }
        }

        public void RunThreadProcess(ref List<byte[]> pixelsListToDo)
        {
            for (int i = 0; i < pixelsListToDo.Count; i++)
            {
                CppWorker freeWorker = null;
                while (freeWorker == null)
                {
                    if (_cppWorkers.Count > 0)
                    {
                        freeWorker = _cppWorkers.Dequeue();
                    }
                }

                freeWorker.SetSingleRegister(pixelsListToDo[i]);
                ThreadPool.QueueUserWorkItem((DoCppJob), freeWorker);
            }

            // TODO Make events system class to call back when end.
            ProgramEventsSystem.CallEndImageProcessingEvent();
        }

        private static void DoCppJob(object parameter)
        {
            CppWorker threadObject = parameter as CppWorker;
            threadObject.DoCppJob();
            AddToQueue(threadObject);
        }
    }

    class CppWorker
    {
        private byte[] _singleRegister;
        public bool IsFree { get; set; }
        public int Index { get; private set; }
        private GrayscaleConverterCpp grayscaleConverter = new GrayscaleConverterCpp();

        public CppWorker(int index)
        {
            Index = index;
        }

        public void SetSingleRegister(byte[] reg)
        {
            _singleRegister = reg;
        }

        public void DoCppJob()
        {
            grayscaleConverter.MakeGrayScaleAtOneRegisterCpp(_singleRegister, 16);
        }
    }
}
