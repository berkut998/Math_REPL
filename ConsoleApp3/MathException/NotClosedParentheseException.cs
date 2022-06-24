using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.MathException
{
    class NotClosedParentheseException : Exception
    {

        public NotClosedParentheseException()
        {

        }
        public NotClosedParentheseException(string message) : base(message)
        {

        }
        public NotClosedParentheseException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
