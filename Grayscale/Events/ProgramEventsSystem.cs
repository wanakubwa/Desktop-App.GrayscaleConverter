using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.Events
{
    static class ProgramEventsSystem
    {
        public delegate void EndImageProcessingHandler();
        public static event EndImageProcessingHandler ImageProcessingClosed;

        public delegate void ThreadCompleatedHandler();
        public static event ThreadCompleatedHandler ThreadCompleated;

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
