using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.CustomExceptions
{
    /// <summary>
    /// Exception to handle that in current using system CPU 
    /// is not avaible SSE instructions to use.
    /// </summary>
    class NotSSEAvaibleException : Exception 
    {
        public NotSSEAvaibleException(string message) : base(message)
        {

        }
    }
}
