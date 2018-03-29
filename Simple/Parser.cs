using System;
using System.Collections.Generic;

namespace Simple
{
    // expression = SimpleExpresison [ relation SimpleExpresison ]
    // SimpleExpresison = [+|-] term {AddOperator term}
    // term = factor { MulOperator factor }
    // factor = number | string | null | designator [ ActualParameters ] | "(" expression ")" 
    // number = integer | real
    // designator = identifier
    // ExpList  =  expression {"," expression }. 
    // ActualParameters  =  "(" [ExpList] ")". 
    // MulOperator  =  "*" | "/" | DIV | MOD | "&". 
    // AddOperator  =  "+" | "-" | OR. 
    // relation  =  "=" | "#" | "<" | "<=" | ">" | ">=" | IN | IS.
    // statement = assignment | ProcCall | If | Case | While | Return
    // assignment = designator := expression
    // StatementSequence  =  statement {";" statement}.
    class Parser
    {
        readonly Scanner scn;

        public Parser(Scanner scanner)
        {
            this.scn = scanner;
        }

        public Expression ReadProgram()
        {
            if (scn.Accept(Token.Types.If))
            {

            }
        }

        Statement Statement()
        {
            if (scn.Accept(Token.Types.If))
            {
                return If();
            }
            if ()
            throw new ParseException($"Expected a statement but got '{scn.Current.Text}' at {scn.Current.Start}");
        }

        // IfStatement  =  IF expression THEN StatementSequence 
        // {ELSIF expression THEN StatementSequence}
        // [ELSE StatementSequence]
        // END.
        private IfStatement If()
        {
            var result = new IfStatement();
            do
            {
                var cond = Expression();
                scn.Expect(Token.Types.Then);
                var then = StatementSequence();
                result.Add(cond, then);
                if (scn.Next.Type == Token.Types.Else)
                {
                    result.Else = StatementSequence();
                    scn.Expect(Token.Types.End);
                    break;
                }
            } while (scn.Expect(Token.Types.Elsif));
            return result;
        }

        Expression Expression()
        {

        }

        private List<Statement> StatementSequence()
        {
            var results = new List<Statement>();
            do
            {
                results.Add(Statement());
            } while (scn.Expect(Token.Types.SemiColon));
            return results;
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

    class Statement
    {
        public Token Token { get; }
    }

    class IfStatement : Statement
    {
        public List<IfThen> IfThen = new List<IfThen>();
        public List<Statement> Else;

        public void Add(Expression cond, List<Statement> then) => IfThen.Add(new IfThen { If = cond, Then = then });
    }

    class IfThen
    {
        public Expression If;
        public List<Statement> Then;
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
