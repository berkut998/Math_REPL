using System;
using System.Collections.Generic;
using REPL.MathException;
using System.Globalization; //need for correct decimal separator (dot "." instead comma ",")
namespace REPL
{

    public class MathEvaluator
    {
        private Parser parser;
        private Token token;
        private Dictionary<string, double?> variables_dictionary = new Dictionary<string, double?>();
        //tmp storage global variables when enter in function
        private Dictionary<string, double?> GlobalVariables = new Dictionary<string, double?>();
        private Dictionary<string, Function> functions_dictionary = new Dictionary<string, Function>();

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
                stack_parsers.Clear();
                stack_variables.Clear();
                GlobalVariables.Clear();
                if (token.type == Token.tokenType.end || token.type == Token.tokenType.error)
                {
                    error(new EmpetyExpresionException("No expression"));
                    return null;
                }
                evaluator_setFunction(ref result);
                if (token.type != Token.tokenType.end)
                {
                    error(new SyntaxException("Syntax error in position :" + parser.ptrCurrentElement + " token: " + token.content));
                    return null;
                }
                return result;
            }
            catch
            {
                return null;
            }

        }
 
        private void evaluator_setFunction(ref double? result)
        {
            if (token.type == Token.tokenType.command)
            {
                token = parser.getToken();
                //try find name function
                if (isDeclarated(token.content))
                    error(new SyntaxException("Variable with such name already exist. Position :" + parser.ptrCurrentElement + " token: " + token.content));
                string functionName = token.content;
                Function currentFunction = new Function();
                token = parser.getToken();
                while (token.type != Token.tokenType.function_sign)
                {
                    if (token.type != Token.tokenType.variable)
                        error(new SyntaxException("Excpected variable. Position: " + parser.ptrCurrentElement));
                    currentFunction.arguments.Add(token);
                    token = parser.getToken();
                }
                while (token.type != Token.tokenType.end && token.type != Token.tokenType.error)
                {
                    token = parser.getToken();
                    currentFunction.instructions.Add(token);
                }
                if (functions_dictionary.ContainsKey(functionName))
                    functions_dictionary[functionName] = currentFunction;
                else
                    functions_dictionary.Add(functionName, currentFunction);
                result = null;
                return;
            }
            evaluate_assigment(ref result);
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
                    if (functions_dictionary.ContainsKey(tmpToken.content))
                        error(new SyntaxException("Function with such name \""+ tmpToken.content + "\" already declared"));
                    token = parser.getToken();
                    evaluate_assigment (ref result); //added here assigment because of A = B = 7
                    addVariable(tmpToken.content, result);
                    return;
                }
            }
            evaluate_add_sub(ref result);
        }
        private void evaluate_function(ref double? result)
        {
            if (functions_dictionary.TryGetValue(token.content, out Function current_function))
            {
                copyGlobalVariable();
                copyParser();
                for (int i = 0; i < current_function.arguments.Count; i++)
                {
                    token = parser.getToken();
                    if (functions_dictionary.TryGetValue(token.content, out Function nextFunction))
                    {
                        evaluate_function(ref result);
                        token.content = Convert.ToString(result, CultureInfo.InvariantCulture);
                        token.type = Token.tokenType.number;
                        result = 0;
                    }
                    if (!Double.TryParse(token.content,NumberStyles.Float, CultureInfo.InvariantCulture, out double varValue))
                        varValue = (double)GlobalVariables[token.content];

                    variables_dictionary.Add(current_function.arguments[i].content, varValue);
                }
                parser = new Parser(current_function.instructions);
                token = parser.getToken();
                evaluate_assigment(ref result);

                getParserBack();
                getVariablesBack();
                token = parser.getToken();
                if (stack_parsers.Count == 0 && token.type == Token.tokenType.end)
                    isSoloFunction = true;
                if (stack_parsers.Count == 0 && token.type == Token.tokenType.number)
                    error(new SyntaxException("Syntax error in position :" + parser.ptrCurrentElement + " token: " + token.content));
                parser.getBack();
                token = new Token(Convert.ToString(result,CultureInfo.InvariantCulture), Token.tokenType.number);
            }
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
        private void evaluate_pow(ref double? result)
        {
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
                evaluate_assigment(ref result); //assigment because of   x = 13 + (y = 3)
                if (token.content != ")")
                    error(new NotClosedParentheseException("Not closed parentheses. Position: " + token.content  ));
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
                    //if (double.TryParse(token.content, out double _result)) //need for different decimal separator -> ,  .
                    //    result = _result;
                    //else 
                        result = double.Parse(token.content, System.Globalization.CultureInfo.InvariantCulture);
                    token = parser.getToken();
                    break;
                default:
                    if (isSoloFunction == true)
                        return;
                    error(new SyntaxException("Syntax error in position :" + parser.ptrCurrentElement + " token: " + token.content));
                    break;
            }
        }
        private void error(Exception exception)
        {
            Console.WriteLine(exception.Message);
            throw exception;
        }

        private void addVariable(string name, double? value)
        {
            if (variables_dictionary.ContainsKey(name))
                variables_dictionary[name] = value;
            else
                variables_dictionary.Add(name, value);
        }
        private double? findVariable(string nameVariable)
        {
            if (!variables_dictionary.TryGetValue(nameVariable, out double? result))
                error(new SyntaxException("Syntax error in position :" + parser.ptrCurrentElement + " token: " + token.content));
            return result;
        }
        private bool isDeclarated(string name)
        {
            if (variables_dictionary.ContainsKey(name))
                return true;
            return false;
        }

        private void copyGlobalVariable()
        {
            Dictionary<string, double?> tmpVariables = new Dictionary<string, double?>();
            foreach (KeyValuePair<string, double?> pair in variables_dictionary)
            {
                tmpVariables.Add(pair.Key, pair.Value);
                if (stack_variables.Count == 0)
                {
                    GlobalVariables.Add(pair.Key, pair.Value);
                }
            }
            stack_variables.Push(tmpVariables);
            variables_dictionary.Clear();

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
                variables_dictionary = stack_variables.Pop();
            }
            if (stack_variables.Count == 0)
                GlobalVariables.Clear();

        }
    }

}

