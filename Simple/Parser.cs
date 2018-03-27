using System;
using System.Collections.Generic;
using System.Text;

namespace Simple
{
    class Parser
    {
        readonly Scanner scanner;

        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
        }

        public Expression ReadProgram()
        {
            if (!scanner.MoveNext())
                return Expression.None;

            var t = scanner.Current;
            if (t.Type == Token.Types.If)
                return ReadIf(t);

            if (t.Type == Token.Types.Integer && IsNumericOperator(scanner.Next))
                return NumericExpression(t);
        }

        private bool IsNumericOperator(Token next)
        {
            return next?.Type == Token.Types.Plus || next?.Type == Token.Types.Minus;
        }

        private Expression ReadIf(Token t)
        {
            throw new NotImplementedException();
        }

        private Expression NumericExpression(Token t)
        {
            throw new NotImplementedException();
        }
    }

    class Expression
    {
        public static Expression None = new Expression(Token.None);
        public Token Token { get; }
        public Token Left { get; set; } // set by the parser for some tokens
        public Token Right { get; set; } // set by the parser for some tokens
        public Token.Types Type => Token.Type;
        public bool HasValue => Token.HasValue;

        public Expression(Token token)
        {
            Token = token;
        }
    }
}
