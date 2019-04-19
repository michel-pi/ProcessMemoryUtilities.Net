using System;
using System.Text;
using System.Runtime.CompilerServices;

namespace ProcessMemoryUtilities.Memory
{
    /// <summary>
    /// Provides methods to convert between byte arrays and strings using different encodings.
    /// </summary>
    public static class StringMarshal
    {
        private const char _nullChar = '0';
        private const string _nullString = "\0";

        /// <summary>
        /// Determines whether null chars should be added to the end of the output of StringToBytes.
        /// </summary>
        public static bool AppendNullCharacters { get; set; } = true;

        /// <summary>
        /// Gets or sets the default encoding used by the methods in this class if no encoding got specified.
        /// </summary>
        public static Encoding DefaultEncoding { get; set; } = Encoding.Default;

        /// <summary>
        /// Converts a byte array into a string using the default encoding if none is specified.
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <param name="encoding">The encoding to use. null if the default encoding should be used.</param>
        /// <returns>The string resulting by decoding the bytes in the given byte array.</returns>
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

        /// <summary>
        /// Converts a string into a byte array using the default encoding if none is specified.
        /// </summary>
        /// <param name="value">The string to encode.</param>
        /// <param name="encoding">The encoding to use. null if the default encoding should be used.</param>
        /// <returns>A byte array representing the encoded characters of the given string.</returns>
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
