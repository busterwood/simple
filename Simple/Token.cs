using System;

namespace Simple
{
    class Token
    {
        public static readonly Token None = new Token(0, null, 0);
        
        public int Start { get; }
        public string Text { get; }
        public Types Type { get; }
        public bool HasValue => Start > 0;

        public Token(int start, string text, Types type)
        {
            Start = start;
            Text = text;
            Type = type == Types.Identifier ? DetectKeyword(type, text) : type;
        }

        static Types DetectKeyword(Types type, string text)
        {
            switch (text)
            {
                case "if": return Types.If;
                case "then": return Types.Then;
                case "else": return Types.Else;
                default: return type;
            }
        }

        public enum Types
        {
            Identifier = 1,
            String,
            Integer,
            Minus,
            Plus,
            If,
            Then,
            Else,
        }
    }
}
