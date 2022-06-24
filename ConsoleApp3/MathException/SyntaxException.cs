using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.MathException
{
    public class SyntaxException:Exception
    {
        public SyntaxException()
        {

        }
        public SyntaxException(string message) : base(message)
        {

        }
        public SyntaxException(string message, Exception inner) : base(message, inner)
        {

        }

    }
}
