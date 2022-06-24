using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp3.MathException;
namespace ConsoleApp3
{

    public class MathEvaluator
    {
        private Parser parser;
        private Token token;
        private Dictionary<string, double?> variables = new Dictionary<string, double?>();
        private Dictionary<string, double?> Globalvariables = new Dictionary<string, double?>();
        private Dictionary<string, Function> functions = new Dictionary<string, Function>();

        private Stack<Parser> stack_parsers = new Stack<Parser>();
        private Stack<Dictionary<string, double?>> stack_variables = new Stack<Dictionary<string, double?>>();

        bool isSoloFunction;
        class Function
        {
            public List<Token> arguments;
            public List<Token> instructions;
            public Function()
            {
                arguments = new List<Token>();
                instructions = new List<Token>();
            }
        }


        public double? evaluate(string mathExpression)
        {
            try
            {
                isSoloFunction = false;
                parser = new Parser(mathExpression);
                double? result = 0;
                token = parser.getToken();
                if (token.type == Token.tokenType.end || token.type == Token.tokenType.error)
                {
                    error(new SyntaxException("Нет выражения для разбора"));
                    return null;
                }
                evaluator_setFunction(ref result);
                if (token.type != Token.tokenType.end)
                {
                    error(new SyntaxException("Синтаксическая ошибка"));
                    return null;
                }
                return result;
            }
            catch
            {
                return null;
            }

        }
        /*
             fn avg => (x + y) / 2
             ERROR: Unknown identifier 'x'
             > fn avg x y => (x + y) / 2
         */
        private void evaluator_setFunction(ref double? result)
        {
            if (token.type == Token.tokenType.command)
            {
                token = parser.getToken();
                //try find name function
                if (isDeclarated(token.content))
                    error(new SyntaxException("Variable with such name already exist"));
                string functionName = token.content;
                Function function = new Function();
                token = parser.getToken();
                while (token.type != Token.tokenType.function_sign)
                {
                    if (token.type != Token.tokenType.variable)
                        error(new SyntaxException("Excpected variable"));
                    function.arguments.Add(token);
                    token = parser.getToken();
                }
                while (token.type != Token.tokenType.end && token.type != Token.tokenType.error)
                {
                    token = parser.getToken();
                    function.instructions.Add(token);
                }
                functions.Add(functionName, function);
                result = null;
                return;
            }
            evaluate_assigment(ref result);
        }

        private void evaluate_function(ref double? result)
        {
            if (functions.TryGetValue(token.content, out Function current_function))
            {
                copyGlobalVariable();
                copyParser();
                for (int i = 0; i < current_function.arguments.Count; i++)
                {
                    token = parser.getToken();
                    if (functions.TryGetValue(token.content, out Function nextFunction))
                    {
                        evaluate_function(ref result);
                        token.content = result.ToString();
                        token.type = Token.tokenType.number;
                        result = 0;
                    }
                    if (!Double.TryParse(token.content, out double varValue))
                        varValue = (double)Globalvariables[token.content];

                    variables.Add(current_function.arguments[i].content, varValue);
                }
                parser = new Parser(current_function.instructions);
                token = parser.getToken();
                evaluate_assigment(ref result);
                getParserBack();
                getVariablesBack();
                if (stack_parsers.Count == 0 && token.type == Token.tokenType.end)
                    isSoloFunction = true;
            }
        }
        private void evaluate_assigment(ref double? result)
        {
            Token tmpToken;
            if (token.type == Token.tokenType.variable)
            {
                tmpToken = token;
                token = parser.getToken();
                if (token.content != "=")
                {
                    parser.getBack();
                    token = tmpToken;
                    evaluate_function(ref result);
                }
                else
                {
                    if (functions.ContainsKey(tmpToken.content))
                        error(new SyntaxException("function with such name already declared"));
                    token = parser.getToken();
                    evaluate_assigment (ref result); //added here assigment because of A = B = 7
                    addVariable(tmpToken.content, result);
                    return;
                }
            }
            evaluate_add_sub(ref result);
        }
        private void evaluate_add_sub(ref double? result)
        {

            double? tmp_result = 0;
            evaluate_mul_div(ref result);
            char oper;
            while ((oper = token.content[0]) == '+' || oper == '-')
            {
                token = parser.getToken();
                evaluate_mul_div(ref tmp_result);
                switch (oper)
                {
                    case '+':
                        result = result + tmp_result;
                        break;
                    case '-':
                        result = result - tmp_result;
                        break;
                }
            }
        }
        //3
        private void evaluate_mul_div(ref double? result)
        {
            double? tmp_result = 0;
            evaluate_pow(ref result);
            char oper = token.content[0];
            while ((oper = token.content[0]) == '*' || oper == '/' || oper == '%')
            {
                token = parser.getToken();
                evaluate_pow(ref tmp_result);
                switch (oper)
                {
                    case '*':
                        result = result * tmp_result;
                        break;
                    case '/':
                        result = result / tmp_result;
                        break;
                    case '%':
                        result = result % tmp_result;
                        break;
                }
            }
        }

        //4
        private void evaluate_pow(ref double? result)
        {
            int i;
            double? tmp_result = 0, exp = 0;
            evaluate_unarMinusOrPlus(ref result);
            if (token.content == "^")
            {
                token = parser.getToken();
                evaluate_pow(ref tmp_result);
                exp = result;
                if (tmp_result == 0)
                    result = 1;
                for (int t = (int)tmp_result - 1; t > 0; t--)
                    result = result * exp;

            }
        }

        private void evaluate_unarMinusOrPlus(ref double? result)
        {
            char oper = '0';
            if (token.type == Token.tokenType.delimetr && (token.content == "+" || token.content == "-"))
            {
                oper = token.content[0];
                token = parser.getToken();
            }
            evaluate_parentheses(ref result);
            if (oper == '-')
                result = -result;
        }

        private void evaluate_parentheses(ref double? result)
        {
            if (token.content == "(")
            {
                token = parser.getToken();
                evaluate_assigment(ref result); //here was add 23.06 change because of   x = 13 + (y = 3)
                if (token.content != ")")
                    error(new SyntaxException("Незакрытая скобка"));
                token = parser.getToken();
            }
            else
            {
                getAtom(ref result);
            }
        }

        private void getAtom(ref double? result)
        {
            switch (token.type)
            {
                case Token.tokenType.variable:
                    result = findVariable(token.content);
                    token = parser.getToken();
                    break;
                case Token.tokenType.number:
                    result = double.Parse(token.content);
                    token = parser.getToken();
                    break;
                default:
                    if (isSoloFunction == true)
                        return;
                    error(new SyntaxException("Синтаксическая ошибка"));
                    break;
            }
        }
        private void error(int exception)
        {
            string[] errors = new string[]
            {
            "Синтаксическая ошибка",
            "Незакрытая скобка",
            "Нет выражения для разбора"
            };
            Console.WriteLine(errors[exception]);
        }
        private void error(Exception exception)
        {
            Console.WriteLine(exception.Message);
            throw exception;
        }

        private void addVariable(string name, double? value)
        {
            if (variables.ContainsKey(name))
                variables[name] = value;
            else
                variables.Add(name, value);
        }
        private double? findVariable(string nameVariable)
        {
            if (!variables.TryGetValue(nameVariable, out double? result))
                error(new SyntaxException("Синтаксическая ошибка"));
            return result;
        }
        private bool isDeclarated(string name)
        {
            if (variables.ContainsKey(name))
                return true;
            return false;
        }

        private void copyGlobalVariable()
        {
            Dictionary<string, double?> tmpVariables = new Dictionary<string, double?>();
            foreach (KeyValuePair<string, double?> pair in variables)
            {
                tmpVariables.Add(pair.Key, pair.Value);
            }
            if (stack_variables.Count == 0)
            {
                Globalvariables = tmpVariables;
            }
            stack_variables.Push(tmpVariables);
            variables.Clear();

        }
        private void copyParser()
        {
            stack_parsers.Push(parser);
        }
        private void getParserBack()
        {
            if (stack_parsers.Count > 0)
                parser = stack_parsers.Pop();
        }
        private void getVariablesBack()
        {
            if (stack_variables.Count > 0)
            {
                variables.Clear();
                variables =  stack_variables.Pop();
            }
            if (stack_variables.Count == 0)
                Globalvariables.Clear();

        }
    }

}

