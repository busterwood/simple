using System.IO;

namespace Simple
{
    static class Extensions
    {
        public static bool TryRead(this TextReader reader, out char ch)
        {
            var val = reader.Read();
            ch = val < 0 ? '\0' : (char)val;
            return val >= 0;
        }

        public static bool TryPeek(this TextReader reader, out char ch)
        {
            var val = reader.Peek();
            ch = val < 0 ? '\0' : (char)val;
            return val >= 0;
        }

    }
}
