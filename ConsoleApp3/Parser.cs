using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public struct Token
    {
        public Token (string content, tokenType type)
        {
            this.content = content;
            this.type = type;
        }
        public enum tokenType
        { 
            delimetr,
            number,
            end,
            error,
            variable,
            command,
            function_sign
        }
        public string content;
        public tokenType type;
        
    }


    public class Parser
    {
        private string input;
        private int ptrLastElement;
        private int ptrCurrentElement;
        private List<Token> tokens;
        public Parser (string input)
        {
            this.input = input;
        }
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        private Token getTokenFromString ()
        {
            StringBuilder strBuilder = new StringBuilder();
            if (ptrCurrentElement > input.Length - 1)
            {
                ptrLastElement = ptrCurrentElement;
                return new Token(" ", Token.tokenType.end);
            }
            while (char.IsWhiteSpace(input[ptrCurrentElement]))
            { 
                ptrCurrentElement++;
                if (ptrCurrentElement > input.Length - 1)
                {
                    ptrLastElement = ptrCurrentElement;
                    return new Token(" ", Token.tokenType.end);
                }
            }
            if (isDelimetr(input[ptrCurrentElement]))
            {
                strBuilder.Append(input[ptrCurrentElement]);
                ptrLastElement = ptrCurrentElement;
                if (ptrCurrentElement+1 < input.Length-1 && (input[ptrCurrentElement] == '=' && input[ptrCurrentElement + 1] == '>') )
                {
                    strBuilder.Append(input[ptrCurrentElement + 1]);
                    ptrCurrentElement += 2;
                    return new Token(strBuilder.ToString(), Token.tokenType.function_sign);
                }
                else
                {
                    ptrCurrentElement++;
                    return new Token(strBuilder.ToString(), Token.tokenType.delimetr);
                }
            }
            if (char.IsDigit(input[ptrCurrentElement]))
            {
                ptrLastElement = ptrCurrentElement;
                while (ptrCurrentElement < input.Length && char.IsDigit(input[ptrCurrentElement]))
                {
                    strBuilder.Append(input[ptrCurrentElement]);
                    ptrCurrentElement++;
                }
                return new Token(strBuilder.ToString(), Token.tokenType.number);
            }
            if (char.IsLetter(input[ptrCurrentElement]))
            {
                ptrLastElement = ptrCurrentElement;
                while (ptrCurrentElement < input.Length && char.IsLetter(input[ptrCurrentElement]))
                {
                    strBuilder.Append(input[ptrCurrentElement]);
                    ptrCurrentElement++;
                }
                if (tokenIsCommand(strBuilder.ToString()))
                    return new Token(strBuilder.ToString(), Token.tokenType.command);
                return new Token(strBuilder.ToString(), Token.tokenType.variable);
            }
            return new Token(" ", Token.tokenType.error);
        }
        private Token getTokenFromList()
        {
            if (ptrCurrentElement > tokens.Count)
                return new Token(" ", Token.tokenType.end);
            ptrLastElement = ptrCurrentElement;
            ptrCurrentElement++;
            return tokens[ptrLastElement];
        }
        public Token getToken ()
        {
            if (tokens == null)
                return getTokenFromString();
            return getTokenFromList();
        }
        /// <summary>
        /// return on 1 token back
        /// </summary>
        public void getBack ()
        {
            ptrCurrentElement = ptrLastElement;
        }

        private bool tokenIsCommand (string token)
        {
            if (token == "fn")
                return true;
            return false;
        }



        private bool isDelimetr (char input)
        {
            if (input == '+' ||
                input == '-' ||
                input == '*' ||
                input == '/' ||
                input == '=' ||
                input == '(' ||
                input == '^' ||
                input == '%' ||
                input == ')')
                return true;
            return false;
        }
    }
}
