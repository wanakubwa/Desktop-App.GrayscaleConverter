using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.Events
{
    static class ProgramEventsSystem
    {
        public delegate void OnImageProcessingClosed();
        public static event OnImageProcessingClosed ImageProcessingClosed;

        public delegate void OnThreadCompleated();
        public static event OnThreadCompleated ThreadCompleated;

        public static void CallImageProcessingClosed()
        {
            if(ImageProcessingClosed != null)
            {
                ImageProcessingClosed.Invoke();
            }
        }

        public static void CallThreadCompleated()
        {
            if (ThreadCompleated != null)
            {
                ThreadCompleated.Invoke();
            }
        }
    }
}
