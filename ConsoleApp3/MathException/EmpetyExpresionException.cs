using System;

namespace REPL.MathException
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
