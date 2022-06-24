using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.MathException
{
    class EmpetyExpresionException:Exception
    {
        public EmpetyExpresionException()
        {
             
        }
        public EmpetyExpresionException(string message) : base(message)
        {

        }
        public EmpetyExpresionException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
