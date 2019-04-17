using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProcessMemoryUtilities.Extensions;

namespace ProcessMemoryUtilities.Test
{
    [TestClass]
    public class IntPtrExtensionsTests
    {
        private readonly Random _random;

        public IntPtrExtensionsTests()
        {
            _random = new Random();
        }

        [TestMethod]
        public void Add()
        {
            IntPtr pointer = new IntPtr(1);

            for (int i = 0; i < 4; i++)
            {
                pointer = pointer.Add((IntPtr)i);
            }

            Assert.IsTrue(pointer == (IntPtr)7);
        }

        [TestMethod]
        public void Divide()
        {
            IntPtr pointer = new IntPtr(16).Divide((IntPtr)2);

            Assert.IsTrue(pointer == (IntPtr)8);
        }

        public void GetValue()
        {
            int value = _random.Next();

            IntPtr pointer = (IntPtr)value;

            Assert.IsTrue(pointer.GetValue() == (ulong)value, "IntPtr.GetValue returned " + pointer.GetValue() + " for a value of " + value.ToString());
        }

        [TestMethod]
        public void IsAligned()
        {
            for (int i = 0; i < 4; i++)
            {
                int number = _random.Next();

                IntPtr pointer = new IntPtr(number);

                bool isAligned = pointer.IsAligned();

                if (number == 1 || number % 2 == 0)
                {
                    Assert.IsTrue(isAligned, "IntPtr.IsAligned returned false for: " + number.ToString());
                }
                else
                {
                    Assert.IsFalse(isAligned, "IntPtr.IsAligned returned true for: " + number.ToString());
                }
            }
        }

        [TestMethod]
        public void IsNotZero()
        {
            Assert.IsFalse(IntPtr.Zero.IsNotZero());
            Assert.IsFalse(new IntPtr(0).IsNotZero());
            Assert.IsTrue(new IntPtr(1).IsNotZero());
        }

        [TestMethod]
        public void IsValid()
        {
            Assert.IsFalse(IntPtr.Zero.IsValid());

            if (IntPtr.Size == 4)
            {
                Assert.IsFalse(new IntPtr(0x05000).IsValid());
            }
            else
            {
                Assert.IsFalse(new IntPtr(0x05000).IsValid());
            }

            Assert.IsTrue(new IntPtr(0x400000).IsValid());
        }

        [TestMethod]
        public void IsZero()
        {
            Assert.IsTrue(IntPtr.Zero.IsZero());
            Assert.IsTrue(new IntPtr(0).IsZero());
            Assert.IsFalse(new IntPtr(1).IsZero());
        }

        [TestMethod]
        public void Multiply()
        {
            Assert.IsTrue(new IntPtr(2).Multiply((IntPtr)2) == (IntPtr)4);
        }

        [TestMethod]
        public void Subtract()
        {
            IntPtr pointer = new IntPtr(7);

            for (int i = 0; i < 4; i++)
            {
                pointer = pointer.Subtract((IntPtr)i);
            }

            Assert.IsTrue(pointer == (IntPtr)1);
        }
    }
}
