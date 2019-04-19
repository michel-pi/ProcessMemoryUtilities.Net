using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProcessMemoryUtilities.Memory;

namespace ProcessMemoryUtilities.Test
{
    [TestClass]
    public class StaticArrayPoolTests
    {
        [TestMethod]
        public void TestAll()
        {
            int[] array;

            StaticArrayPool<int>.Fill(new int[4]);

            for (int i = 0; i < 4; i++)
            {
                array = StaticArrayPool<int>.Rent(4);

                Assert.IsFalse(array == null);
                Assert.IsTrue(array.Length == 4);
            }

            StaticArrayPool<int>.Fill(new int[4], true);

            for (int i = 0; i < 4; i++)
            {
                array = StaticArrayPool<int>.Rent(4);

                foreach (var element in array)
                {
                    Assert.IsTrue(element == 0);
                }

                Assert.IsFalse(array == null);
                Assert.IsTrue(array.Length == 4);
            }

            array = StaticArrayPool<int>.Rent(4);

            Assert.IsFalse(array == null);
            Assert.IsTrue(array.Length == 4);

            StaticArrayPool<int>.Return(array, true);

            array = StaticArrayPool<int>.Rent(4);

            Assert.IsFalse(array == null);
            Assert.IsTrue(array.Length == 4);

            foreach (var element in array)
            {
                Assert.IsTrue(element == 0);
            }
        }
    }
}
