using System;
using System.Text;
using System.Runtime.CompilerServices;

namespace ProcessMemoryUtilities.Memory
{
    public static class StringMarshal
    {
        private const char _nullChar = '0';
        private const string _nullString = "\0";

        public static bool AppendNullCharacters { get; set; } = true;
        public static Encoding DefaultEncoding { get; set; } = Encoding.Default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string BytesToString(byte[] bytes, Encoding encoding = null)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length == 0) throw new ArgumentOutOfRangeException(nameof(bytes));

            if (encoding == null) encoding = DefaultEncoding;

            var result = encoding.GetString(bytes);

            int index = result.IndexOf(_nullChar);

            if (index == -1)
            {
                return result;
            }
            else
            {
                return result.Substring(0, index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] StringToBytes(string value, Encoding encoding = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (encoding == null) encoding = DefaultEncoding;

            if (value == string.Empty)
            {
                return encoding.GetBytes(_nullString);
            }
            else
            {
                return encoding.GetBytes(AppendNullCharacters ? value + _nullString : value);
            }
        }
    }
}
