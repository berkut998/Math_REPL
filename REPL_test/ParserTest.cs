using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleApp3;
namespace REPL_test
{
    [TestClass]
    public class ParserTest
    {
        private bool listIsEqual(List<Token> list1, List<Token> list2)
        {
            if (list1.Count == list2.Count)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    if (list1[i].content != list2[i].content || list1[i].type != list2[i].type)
                        return false;
                    
                }
            }
            else
                return false;
            return true;
        }
        [TestMethod]
        public void parserSum()
        {
            Parser parser = new Parser("1+ 2   +3   +4   +5");
            List < Token > excpected_tokensList = new List<Token>() { new Token("1", Token.tokenType.number),
                new Token("+", Token.tokenType.delimetr),
            new Token("2", Token.tokenType.number),
            new Token("+", Token.tokenType.delimetr),
            new Token("3", Token.tokenType.number),
            new Token("+", Token.tokenType.delimetr),
            new Token("4", Token.tokenType.number),
            new Token("+", Token.tokenType.delimetr),
            new Token("5", Token.tokenType.number),
             new Token(" ", Token.tokenType.end)};
            List<Token> result_tokensList = new List<Token>();
            do
            {
                result_tokensList.Add(parser.getToken());
            } while (result_tokensList[result_tokensList.Count - 1].type != Token.tokenType.end);

            Assert.IsTrue(listIsEqual(excpected_tokensList, result_tokensList));
           
        }



        [TestMethod]
        public void parserMinus()
        {
            Parser parser = new Parser("-1- 2   -3   -4   -5");
            List<Token> excpected_tokensList = new List<Token>() {new Token("-", Token.tokenType.delimetr),
                new Token("1", Token.tokenType.number),
                new Token("-", Token.tokenType.delimetr),
            new Token("2", Token.tokenType.number),
            new Token("-", Token.tokenType.delimetr),
            new Token("3", Token.tokenType.number),
            new Token("-", Token.tokenType.delimetr),
            new Token("4", Token.tokenType.number),
            new Token("-", Token.tokenType.delimetr),
            new Token("5", Token.tokenType.number),
             new Token(" ", Token.tokenType.end)};
            List<Token> result_tokensList = new List<Token>();
            do
            {
                result_tokensList.Add(parser.getToken());
            } while (result_tokensList[result_tokensList.Count - 1].type != Token.tokenType.end);

            Assert.IsTrue(listIsEqual(excpected_tokensList, result_tokensList));

        }


        [TestMethod]
        public void parserMultiply()
        {
            Parser parser = new Parser("-1* 2   *3   *4   *5");
            List<Token> excpected_tokensList = new List<Token>() {new Token("-", Token.tokenType.delimetr),
                new Token("1", Token.tokenType.number),
                new Token("*", Token.tokenType.delimetr),
            new Token("2", Token.tokenType.number),
            new Token("*", Token.tokenType.delimetr),
            new Token("3", Token.tokenType.number),
            new Token("*", Token.tokenType.delimetr),
            new Token("4", Token.tokenType.number),
            new Token("*", Token.tokenType.delimetr),
            new Token("5", Token.tokenType.number),
             new Token(" ", Token.tokenType.end)};
            List<Token> result_tokensList = new List<Token>();
            do
            {
                result_tokensList.Add(parser.getToken());
            } while (result_tokensList[result_tokensList.Count - 1].type != Token.tokenType.end);

            Assert.IsTrue(listIsEqual(excpected_tokensList, result_tokensList));

        }
        [TestMethod]
        public void parserDivide()
        {
            Parser parser = new Parser("-1/ 2   /3   /4   /5");
            List<Token> excpected_tokensList = new List<Token>() {new Token("-", Token.tokenType.delimetr),
                new Token("1", Token.tokenType.number),
                new Token("/", Token.tokenType.delimetr),
            new Token("2", Token.tokenType.number),
            new Token("/", Token.tokenType.delimetr),
            new Token("3", Token.tokenType.number),
            new Token("/", Token.tokenType.delimetr),
            new Token("4", Token.tokenType.number),
            new Token("/", Token.tokenType.delimetr),
            new Token("5", Token.tokenType.number),
             new Token(" ", Token.tokenType.end)};
            List<Token> result_tokensList = new List<Token>();
            do
            {
                result_tokensList.Add(parser.getToken());
            } while (result_tokensList[result_tokensList.Count - 1].type != Token.tokenType.end);

            Assert.IsTrue(listIsEqual(excpected_tokensList, result_tokensList));

        }

        [TestMethod]
        public void parserPow()
        {
            Parser parser = new Parser("5^2^2");
            List<Token> excpected_tokensList = new List<Token>() {new Token("5", Token.tokenType.number),
                new Token("^", Token.tokenType.delimetr),
                new Token("2", Token.tokenType.number),
            new Token("^", Token.tokenType.delimetr),
            new Token("2", Token.tokenType.number), 
             new Token(" ", Token.tokenType.end)};
            List<Token> result_tokensList = new List<Token>();
            do
            {
                result_tokensList.Add(parser.getToken());
            } while (result_tokensList[result_tokensList.Count - 1].type != Token.tokenType.end);
            Assert.IsTrue(listIsEqual(excpected_tokensList, result_tokensList));
        }


        [TestMethod]
        public void parserParenthless()
        {
            Parser parser = new Parser("-1/( 2   /3   /4 )  /5");
            List<Token> excpected_tokensList = new List<Token>() {new Token("-", Token.tokenType.delimetr),
                new Token("1", Token.tokenType.number),
                new Token("/", Token.tokenType.delimetr),
            new Token("(", Token.tokenType.delimetr),
            new Token("2", Token.tokenType.number),
            new Token("/", Token.tokenType.delimetr),
            new Token("3", Token.tokenType.number),
            new Token("/", Token.tokenType.delimetr),
            new Token("4", Token.tokenType.number),
             new Token(")", Token.tokenType.delimetr),
            new Token("/", Token.tokenType.delimetr),
            new Token("5", Token.tokenType.number),
             new Token(" ", Token.tokenType.end)};
            List<Token> result_tokensList = new List<Token>();
            do
            {
                result_tokensList.Add(parser.getToken());
            } while (result_tokensList[result_tokensList.Count - 1].type != Token.tokenType.end);
            Assert.IsTrue(listIsEqual(excpected_tokensList, result_tokensList));
        }

        [TestMethod]
        public void parser_sum_sub_mul_div_parenth()
        {
            Parser parser = new Parser("-1/( 2   +3   *4 )  /5");
            List<Token> excpected_tokensList = new List<Token>() {new Token("-", Token.tokenType.delimetr),
                new Token("1", Token.tokenType.number),
                new Token("/", Token.tokenType.delimetr),
            new Token("(", Token.tokenType.delimetr),
            new Token("2", Token.tokenType.number),
            new Token("+", Token.tokenType.delimetr),
            new Token("3", Token.tokenType.number),
            new Token("*", Token.tokenType.delimetr),
            new Token("4", Token.tokenType.number),
             new Token(")", Token.tokenType.delimetr),
            new Token("/", Token.tokenType.delimetr),
            new Token("5", Token.tokenType.number),
             new Token(" ", Token.tokenType.end)};
            List<Token> result_tokensList = new List<Token>();
            do
            {
                result_tokensList.Add(parser.getToken());
            } while (result_tokensList[result_tokensList.Count - 1].type != Token.tokenType.end);
            Assert.IsTrue(listIsEqual(excpected_tokensList, result_tokensList));
        }


        [TestMethod]
        public void empety()
        {
            Parser parser = new Parser("");
            List<Token> excpected_tokensList = new List<Token>() {new Token(" ", Token.tokenType.end)};
            List<Token> result_tokensList = new List<Token>();
            do
            {
                result_tokensList.Add(parser.getToken());
            } while (result_tokensList[result_tokensList.Count - 1].type != Token.tokenType.end);
            Assert.IsTrue(listIsEqual(excpected_tokensList, result_tokensList));
        }


        [TestMethod]
        public void error()
        {
            Parser parser = new Parser("$$$$ $#@");
            List<Token> excpected_tokensList = new List<Token>() { new Token(" ", Token.tokenType.error) };
            List<Token> result_tokensList = new List<Token>();
            do
            {
                result_tokensList.Add(parser.getToken());
            } while (result_tokensList[result_tokensList.Count - 1].type != Token.tokenType.error);
            Assert.IsTrue(listIsEqual(excpected_tokensList, result_tokensList));
        }
    }
}
