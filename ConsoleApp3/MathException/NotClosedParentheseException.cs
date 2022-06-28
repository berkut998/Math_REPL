using System;

namespace REPL.MathException
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
