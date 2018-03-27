using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Simple
{
    class Scanner : IEnumerator<Token>
    {
        readonly TextReader reader;
        readonly StringBuilder sb;
        bool atStart;
        int position;

        public Scanner(TextReader reader)
        {
            this.reader = reader;
            atStart = true;
            sb = new StringBuilder(80);
        }

        object IEnumerator.Current => Current;
        public Token Current { get; private set; }
        public Token Next { get; private set; }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (atStart)
            {
                Current = Read();
                Next = Read();
                atStart = false;
            }
            else
            {
                Current = Next;
                Next = Read();
            }
            return Current.HasValue;
        }

        private Token Read()
        {
            for (;;)
            {
                char ch;
                if (!reader.TryRead(out ch))
                    return Token.None;
                position++;

                switch (ch)
                {
                    case ' ':
                    case '\t':
                    case '\f':
                    case '\r':
                    case '\n':
                        break; // skip white space
                    case '"':
                        return ReadString();
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '0':
                        return ReadInteger(ch);
                    case '-':
                        return ReadMinus();
                    case '+':
                        return ReadPlus();
                    default:
                        if (char.IsLetter(ch))
                            return ReadIdentifier(ch);
                        throw new NotSupportedException();
                }
            }
        }

        private Token ReadPlus()
        {
            return new Token(position, "+", Token.Types.Plus);
        }

        private Token ReadMinus()
        {
            if (NegativeNumber())
                return ReadInteger('-');
            return new Token(position, "-", Token.Types.Minus);
        }

        bool NegativeNumber()
        {
            var next = reader.Peek();
            return next > 0 && char.IsNumber((char)next);
        }

        private Token ReadIdentifier(char first)
        {
            var start = position;
            sb.Clear();
            sb.Append(first);
            for (;;)
            {
                char ch;
                reader.TryPeek(out ch);
                if (char.IsLetterOrDigit(ch) || ch == '_')
                {
                    sb.Append(ch);
                    reader.Read(); // we peeked above
                    position++;
                }
                else
                    break;
            }
            return new Token(start, sb.ToString(), Token.Types.Identifier);
        }

        public Token ReadString()
        {
            var start = position;
            char ch;
            sb.Clear();
            do
            {
                if (!reader.TryRead(out ch))
                    return Token.None;
                position++;
            } while (ch != '"');
            return new Token(start, sb.ToString(), Token.Types.String);
        }

        public Token ReadInteger(char first)
        {
            var start = position;
            sb.Clear();
            sb.Append(first);
            for(;;)
            {
                char ch;
                if (!reader.TryPeek(out ch))
                    break;

                switch (ch)
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '0':
                        sb.Append(ch);
                        reader.Read(); // we peeked
                        position++;
                        break;
                    default:
                        // TODO: check what the next token is to prevent "123hello" being read as two tokens "123" and "hello"
                        return new Token(start, sb.ToString(), Token.Types.Integer);

                }
            }
            if (first == '-' && sb.Length == 1)
                throw new ParseException("'-' is not a valid number", start);

            return new Token(start, sb.ToString(), Token.Types.Integer);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
