using System;
using System.Text;
namespace REPL
{
    class Program
    {	   

      //Simple Interactive Interpreter
	  //https://www.codewars.com/kata/52ffcfa4aff455b3c2000750
	   
        static void Main(string[] args)
        {
            MathEvaluator mathEvaluator = new MathEvaluator();
            while (true)
                Console.WriteLine("Result: " + mathEvaluator.evaluate(Console.ReadLine()));
            

            //all work
            //Parser parser = new Parser("fn functionName => 1 + 5");
            //Token token;
            //do
            //{
            //    token = parser.getToken();
            //}
            //while (token.type != Token.tokenType.end && token.type != Token.tokenType.error);


            //all work
            //MathEvaluator mathEvaluator = new MathEvaluator();
            //double? result = mathEvaluator.evaluate("A=5");
            //double? result1 = mathEvaluator.evaluate("B=5*1");
            //double? result2 = mathEvaluator.evaluate("A*B");
            //double? result3 = mathEvaluator.evaluate("fn functionName x => 2 + 5 + x");
            //double? result4 = mathEvaluator.evaluate("functionName functionName 1");

            //all work
            //mathEvaluator.evaluate("A = B = 7");
            //mathEvaluator.evaluate("fn ZeroValue input => input = 0");
            //mathEvaluator.evaluate("fn sum x y => x + y");
            //mathEvaluator.evaluate("A = 5");
            //mathEvaluator.evaluate("ZeroValue A + 5")



            //all work
            //mathEvaluator.evaluate("fn ZeroValue input => input = 0");
            //mathEvaluator.evaluate("fn sum x y => x + y");
            //mathEvaluator.evaluate("A = 5");
            //mathEvaluator.evaluate("ZeroValue A + 5");
            //mathEvaluator.evaluate("sum 1 ZeroValue 0");

            //all work
            //mathEvaluator.evaluate("fn one => 1");
            //mathEvaluator.evaluate("fn avg x y => (x + y) / 2");
            //mathEvaluator.evaluate("fn echo x => x");
            //mathEvaluator.evaluate("fn add x y => x + z");
            //mathEvaluator.evaluate("fn add x x => x + x");
            //mathEvaluator.evaluate("(fn f => 1)");
            //mathEvaluator.evaluate("one");
            //mathEvaluator.evaluate("avg 4 2");
            //mathEvaluator.evaluate("avg 7");
            //mathEvaluator.evaluate("avg 7 2 4");
            //mathEvaluator.evaluate("avg echo 4 echo 2");
            //mathEvaluator.evaluate("avg echo 7");
            //mathEvaluator.evaluate("avg echo 7 echo 2 echo 4");
            //mathEvaluator.evaluate("fn f a b => a * b");
            //mathEvaluator.evaluate("fn g a b c => a * b * c");
            //mathEvaluator.evaluate("g g 1 2 3 f 4 5 f 6 7") ;//5040


        }
        

        
    }
}
