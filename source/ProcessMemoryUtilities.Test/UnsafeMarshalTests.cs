using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProcessMemoryUtilities.Memory;

namespace ProcessMemoryUtilities.Test
{
    [TestClass]
    public class UnsafeMarshalTests
    {
        [TestMethod]
        public void Allocate()
        {
            Assert.IsTrue(UnsafeMarshal.Allocate<int>() == 0);
        }

        [TestMethod]
        public void AllocateArray()
        {
            var array = UnsafeMarshal.AllocateArray<int>(4);

            Assert.IsFalse(array == null);
            Assert.IsTrue(array.Length == 4);

            foreach (var value in array)
            {
                Assert.IsTrue(value == 0);
            }
        }

        [TestMethod]
        public void AllocateNonZero()
        {
            Assert.IsInstanceOfType(UnsafeMarshal.AllocateNonZero<int>(), typeof(int));
        }

        [TestMethod]
        public void AsPointer()
        {
            int value = 1337;

            IntPtr address = UnsafeMarshal.AsPointer<int>(ref value);

            Assert.IsFalse(address == IntPtr.Zero);
            Assert.IsTrue(Marshal.ReadInt32(address) == 1337);
        }

        [TestMethod]
        public void Copy_Pointer()
        {
            IntPtr source = Marshal.AllocHGlobal(4);
            IntPtr dest = Marshal.AllocHGlobal(4);

            Marshal.WriteInt32(source, 1337);
            Marshal.WriteInt32(dest, 0);

            UnsafeMarshal.Copy(source, dest, 4);

            Assert.IsTrue(Marshal.ReadInt32(dest) == 1337);

            Marshal.FreeHGlobal(source);
            Marshal.FreeHGlobal(dest);
        }

        [TestMethod]
        public void Copy_TDest()
        {
            IntPtr source = Marshal.AllocHGlobal(4);
            int dest = 0;

            Marshal.WriteInt32(source, 1337);

            UnsafeMarshal.Copy<int>(source, ref dest);

            Assert.IsTrue(dest == 1337);

            Marshal.FreeHGlobal(source);
        }

        [TestMethod]
        public void Copy_TSource()
        {
            IntPtr dest = Marshal.AllocHGlobal(4);
            int source = 1337;

            Marshal.WriteInt32(dest, 0);

            UnsafeMarshal.Copy<int>(ref source, dest);

            Assert.IsTrue(Marshal.ReadInt32(dest) == 1337);

            Marshal.FreeHGlobal(dest);
        }

        [TestMethod]
        public void Read_Bytes()
        {
            byte[] array = new byte[8];

            var bytes = BitConverter.GetBytes(1337);

            for (int i = 0; i < bytes.Length; i++)
            {
                array[i] = bytes[i];
            }

            bytes = BitConverter.GetBytes(7331);

            for (int i = 0; i < bytes.Length; i++)
            {
                array[i + 4] = bytes[i];
            }

            Assert.IsTrue(UnsafeMarshal.Read<int>(array) == 1337);
            Assert.IsTrue(UnsafeMarshal.Read<int>(array, 4) == 7331);
        }

        [TestMethod]
        public void Read_Pointer()
        {
            IntPtr address = Marshal.AllocHGlobal(8);
            Marshal.WriteInt32(address, 1337);
            Marshal.WriteInt32(address, 4, 7331);

            Assert.IsTrue(UnsafeMarshal.Read<int>(address) == 1337);
            Assert.IsTrue(UnsafeMarshal.Read<int>(address, 4) == 7331);

            Marshal.FreeHGlobal(address);
        }

        [TestMethod]
        public void ReinterpretCast()
        {
            int value = 1337;
            float flt = BitConverter.ToSingle(BitConverter.GetBytes(value), 0);

            Assert.IsTrue(UnsafeMarshal.ReinterpretCast<int, float>(ref value) == flt);
        }

        [TestMethod]
        public void SizeOf()
        {
            Assert.IsTrue(UnsafeMarshal.SizeOf<bool>() == 1);
            Assert.IsTrue(UnsafeMarshal.SizeOf<int>() == 4);
            Assert.IsTrue(UnsafeMarshal.SizeOf<double>() == 8);
        }

        [TestMethod]
        public void Write_Bytes()
        {
            byte[] buffer = new byte[8];

            UnsafeMarshal.Write<int>(buffer, 1337);
            UnsafeMarshal.Write<int>(buffer, 4, 7331);

            Assert.IsTrue(BitConverter.ToInt32(buffer, 0) == 1337);
            Assert.IsTrue(BitConverter.ToInt32(buffer, 4) == 7331);
        }

        [TestMethod]
        public void Write_Pointer()
        {
            IntPtr address = Marshal.AllocHGlobal(8);

            UnsafeMarshal.Write<int>(address, 1337);
            UnsafeMarshal.Write<int>(address, 4, 7331);

            Assert.IsTrue(Marshal.ReadInt32(address) == 1337);
            Assert.IsTrue(Marshal.ReadInt32(address, 4) == 7331);

            Marshal.FreeHGlobal(address);
        }

        [TestMethod]
        public void ZeroArray()
        {
            int[] array = new int[4] { 1337, 1337, 1337, 1337 };

            UnsafeMarshal.ZeroArray<int>(array);

            foreach (var value in array)
            {
                Assert.IsTrue(value == 0);
            }
        }

        [TestMethod]
        public void ZeroMemory()
        {
            IntPtr address = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(address, 1337);

            UnsafeMarshal.ZeroMemory(address, 4);

            Assert.IsTrue(Marshal.ReadInt32(address) == 0);

            Marshal.FreeHGlobal(address);
        }

        [TestMethod]
        public void ZeroMemory_T()
        {
            int value = 1337;

            UnsafeMarshal.ZeroMemory(ref value);

            Assert.IsTrue(value == 0);
        }
    }
}
