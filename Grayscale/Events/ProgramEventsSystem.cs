using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.Events
{
    static class ProgramEventsSystem
    {
        public delegate void EndImageProcessing();
        public static event EndImageProcessing EndImageProcessingHandler;

        public static void CallEndImageProcessingEvent()
        {
            if(EndImageProcessingHandler != null)
            {
                EndImageProcessingHandler.Invoke();
            }
        }
    }
}