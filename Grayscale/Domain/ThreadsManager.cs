using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GrayscaleCppManager;

namespace Grayscale.Processing
{
    class ThreadsManager
    {
        bool _isAsm;
        List<Thread> _threads = new List<Thread>();
        List<CppWorker> _cppWorkers = new List<CppWorker>();
        public int ThreadsNum { get; set; }

        public ThreadsManager(bool isAsm)
        {
            this._isAsm = isAsm;
        }

        public void InitializeTasks()
        {
            for(int i = 0; i< ThreadsNum; i++)
            {
                Thread thread = new Thread(DoCppJob);
                _threads.Add(thread);
            }
        }

        public void InitializeCppWorker()
        {
            for(int i = 0; i< ThreadsNum; i++)
            {
                CppWorker cppWorker = new CppWorker(i);
                cppWorker.IsFree = true;
                _cppWorkers.Add(cppWorker);
            }
        }

        public void RunThreadProcess(ref List<byte[]> pixelsListToDo)
        {
            InitializeTasks();

            for (int i = 0; i < pixelsListToDo.Count; i++)
            {
                CppWorker freeWorker = null;
                while (freeWorker == null)
                {
                    freeWorker = _cppWorkers.Find(x => x.IsFree == true);
                }
                freeWorker.SetSingleRegister(pixelsListToDo[i]);
                freeWorker.IsFree = false;
                ThreadPool.QueueUserWorkItem((DoCppJob), freeWorker);
                Console.WriteLine(i);
                //_threads[freeWorker.Index].Start(freeWorker);
            }

            Thread.Sleep(2000);
        }

        private static void DoCppJob(object parameter)
        {
            CppWorker threadObject = parameter as CppWorker;
            threadObject.DoCppJob();
            Console.WriteLine("Threed num = {0}", Thread.CurrentThread.ManagedThreadId);
            threadObject.IsFree = true;
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
