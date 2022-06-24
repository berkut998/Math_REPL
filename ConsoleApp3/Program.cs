using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Cache;
using System.IO;
namespace ConsoleApp3
{
    class Program
    {
       //TODO problem with global variables when give them to method
       //TODO overwiting function (should work but beacuse of global variable still not tested)
        static void Main(string[] args)
        {
            Parser parser = new Parser("fn functionName => 1 + 5");
            Token token;
            do
            {
                token = parser.getToken();
            }
            while (token.type != Token.tokenType.end && token.type != Token.tokenType.error);



            MathEvaluator mathEvaluator = new MathEvaluator();
            //double? result = mathEvaluator.evaluate("A=5");
            //double? result1 = mathEvaluator.evaluate("B=5*1");
            //double? result2 = mathEvaluator.evaluate("A*B");
            //double? result3 = mathEvaluator.evaluate("fn functionName x => 2 + 5 + x");
            //double? result4 = mathEvaluator.evaluate("functionName functionName 1");


            //mathEvaluator.evaluate("A = B = 7");
            //mathEvaluator.evaluate("fn ZeroValue input => input = 0");
            //mathEvaluator.evaluate("fn sum x y => x + y");
            //mathEvaluator.evaluate("A = 5");
            //mathEvaluator.evaluate("ZeroValue A + 5");    

            mathEvaluator.evaluate("ZZ = 1");
            mathEvaluator.evaluate("ZZ     ");
            
            mathEvaluator.evaluate("a = 1");
            mathEvaluator.evaluate("a");
            mathEvaluator.evaluate("fn inc x => x + 1");
            mathEvaluator.evaluate("a = inc a");
            mathEvaluator.evaluate("fn inc x => x + 2");
            mathEvaluator.evaluate("a = inc a");
        }
    }
}
