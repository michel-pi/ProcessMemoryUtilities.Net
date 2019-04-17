using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProcessMemoryUtilities.Memory;

namespace ProcessMemoryUtilities.Test
{
    [TestClass]
    public class StringMarshalTests
    {
        private List<Encoding> _encodings;

        public StringMarshalTests()
        {
            _encodings = new List<Encoding>()
            {
                Encoding.ASCII,
                Encoding.BigEndianUnicode,
                Encoding.Default,
                Encoding.Unicode,
                Encoding.UTF32,
                Encoding.UTF7,
                Encoding.UTF8
            };
        }

        private static bool ByteArrayEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null) return false;
            if (left.Length != right.Length) return false;

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i]) return false;
            }

            return true;
        }

        private static bool SmallerByteArrayEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null) return false;

            if (left.Length <= right.Length)
            {
                for (int i = 0; i < left.Length; i++)
                {
                    if (left[i] != right[i]) return false;
                }
            }
            else
            {
                for (int i = 0; i < right.Length; i++)
                {
                    if (left[i] != right[i]) return false;
                }
            }

            return true;
        }

        [TestMethod]
        public void BytesToString()
        {
            string str = "Test";

            foreach (var encoding in _encodings)
            {
                byte[] bytes = encoding.GetBytes(str);

                Assert.IsTrue(StringMarshal.BytesToString(bytes, encoding) == str);
            }
        }

        [TestMethod]
        public void StringToBytes()
        {
            StringMarshal.AppendNullCharacters = false;

            foreach (var encoding in _encodings)
            {
                byte[] original = encoding.GetBytes("Test");
                byte[] marshal = StringMarshal.StringToBytes("Test", encoding);

                Assert.IsTrue(ByteArrayEquals(original, marshal));
            }

            StringMarshal.AppendNullCharacters = true;

            foreach (var encoding in _encodings)
            {
                byte[] original = encoding.GetBytes("Test");
                byte[] marshal = StringMarshal.StringToBytes("Test", encoding);

                Assert.IsTrue(SmallerByteArrayEquals(original, marshal));
            }
        }
    }
}
