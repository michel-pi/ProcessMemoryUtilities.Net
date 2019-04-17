using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProcessMemoryUtilities.Memory;

namespace ProcessMemoryUtilities.Test
{
    [TestClass]
    public class ProcessMemoryTests
    {
        private int _processId;

        public ProcessMemoryTests()
        {
            _processId = Process.GetCurrentProcess().Id;
        }

        [TestMethod]
        public void Close()
        {
            IntPtr handle = ProcessMemory.OpenProcess(ProcessAccessFlags.All, _processId);

            Assert.IsFalse(handle == IntPtr.Zero);

            Assert.IsTrue(ProcessMemory.Close(handle));
        }

        [TestMethod]
        public void OpenProcess()
        {
            IntPtr handle = ProcessMemory.OpenProcess(ProcessAccessFlags.All, _processId);

            Assert.IsFalse(handle == IntPtr.Zero);

            ProcessMemory.Close(handle);
        }

        [TestMethod]
        public void ReadProcessMemory_Pointer()
        {
            IntPtr baseAddress = Marshal.AllocHGlobal(4);
            IntPtr buffer = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(baseAddress, 1337);
            Marshal.WriteInt32(buffer, 0);

            IntPtr handle = ProcessMemory.OpenProcess(ProcessAccessFlags.All, _processId);

            Assert.IsFalse(handle == IntPtr.Zero);

            Assert.IsTrue(ProcessMemory.ReadProcessMemory(handle, baseAddress, buffer, (IntPtr)4));
            Assert.IsTrue(Marshal.ReadInt32(baseAddress) == Marshal.ReadInt32(buffer));

            Marshal.WriteInt32(buffer, 0);

            IntPtr bytesRead = IntPtr.Zero;

            Assert.IsTrue(ProcessMemory.ReadProcessMemory(handle, baseAddress, buffer, (IntPtr)4, ref bytesRead));
            Assert.IsTrue(Marshal.ReadInt32(baseAddress) == Marshal.ReadInt32(buffer));
            Assert.IsTrue(bytesRead == (IntPtr)4);

            Assert.IsTrue(ProcessMemory.Close(handle));

            Marshal.FreeHGlobal(baseAddress);
            Marshal.FreeHGlobal(buffer);
        }

        [TestMethod]
        public void ReadProcessMemory_T()
        {
            IntPtr baseAddress = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(baseAddress, 1337);

            IntPtr handle = ProcessMemory.OpenProcess(ProcessAccessFlags.All, _processId);

            Assert.IsFalse(handle == IntPtr.Zero);

            int buffer = 0;

            Assert.IsTrue(ProcessMemory.ReadProcessMemory(handle, baseAddress, ref buffer));
            Assert.IsTrue(buffer == Marshal.ReadInt32(baseAddress));

            buffer = 0;

            IntPtr bytesRead = IntPtr.Zero;

            Assert.IsTrue(ProcessMemory.ReadProcessMemory(handle, baseAddress, ref buffer, ref bytesRead));
            Assert.IsTrue(buffer == Marshal.ReadInt32(baseAddress));
            Assert.IsTrue(bytesRead == (IntPtr)4);

            Assert.IsTrue(ProcessMemory.Close(handle));

            Marshal.FreeHGlobal(baseAddress);
        }

        [TestMethod]
        public void ReadProcessMemory_TArray()
        {
            IntPtr baseAddress = Marshal.AllocHGlobal(16);

            for (int i = 0; i < 16; i += 4)
            {
                Marshal.WriteInt32(baseAddress, i, 1337);
            }

            IntPtr handle = ProcessMemory.OpenProcess(ProcessAccessFlags.All, _processId);

            Assert.IsFalse(handle == IntPtr.Zero);

            int[] buffer = new int[4];

            Assert.IsTrue(ProcessMemory.ReadProcessMemory(handle, baseAddress, buffer));

            for (int i = 0; i < buffer.Length; i++)
            {
                Assert.IsTrue(buffer[i] == Marshal.ReadInt32(baseAddress, i * 4));

                buffer[i] = 0;
            }

            IntPtr bytesRead = IntPtr.Zero;

            Assert.IsTrue(ProcessMemory.ReadProcessMemory(handle, baseAddress, buffer, ref bytesRead));
            Assert.IsTrue(bytesRead == (IntPtr)(buffer.Length * 4));

            for (int i = 0; i < buffer.Length; i++)
            {
                Assert.IsTrue(buffer[i] == Marshal.ReadInt32(baseAddress, i * 4));
            }

            Assert.IsTrue(ProcessMemory.Close(handle));

            Marshal.FreeHGlobal(baseAddress);
        }

        [TestMethod]
        public void WriteProcessMemory_Pointer()
        {
            IntPtr baseAddress = Marshal.AllocHGlobal(4);
            IntPtr buffer = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(baseAddress, 0);
            Marshal.WriteInt32(buffer, 1337);

            IntPtr handle = ProcessMemory.OpenProcess(ProcessAccessFlags.All, _processId);

            Assert.IsFalse(handle == IntPtr.Zero);

            Assert.IsTrue(ProcessMemory.WriteProcessMemory(handle, baseAddress, buffer, (IntPtr)4));
            Assert.IsTrue(Marshal.ReadInt32(baseAddress) == Marshal.ReadInt32(buffer));

            Marshal.WriteInt32(baseAddress, 0);

            IntPtr bytesWritten = IntPtr.Zero;

            Assert.IsTrue(ProcessMemory.WriteProcessMemory(handle, baseAddress, buffer, (IntPtr)4, ref bytesWritten));
            Assert.IsTrue(Marshal.ReadInt32(baseAddress) == Marshal.ReadInt32(buffer));
            Assert.IsTrue(bytesWritten == (IntPtr)4);

            Assert.IsTrue(ProcessMemory.Close(handle));

            Marshal.FreeHGlobal(baseAddress);
            Marshal.FreeHGlobal(buffer);
        }

        [TestMethod]
        public void WriteProcessMemory_T()
        {
            IntPtr baseAddress = Marshal.AllocHGlobal(4);
            int buffer = 1337;
            Marshal.WriteInt32(baseAddress, 0);

            IntPtr handle = ProcessMemory.OpenProcess(ProcessAccessFlags.All, _processId);

            Assert.IsFalse(handle == IntPtr.Zero);

            Assert.IsTrue(ProcessMemory.WriteProcessMemory(handle, baseAddress, buffer));
            Assert.IsTrue(Marshal.ReadInt32(baseAddress) == buffer);

            Marshal.WriteInt32(baseAddress, 0);

            IntPtr bytesWritten = IntPtr.Zero;

            Assert.IsTrue(ProcessMemory.WriteProcessMemory(handle, baseAddress, buffer, ref bytesWritten));
            Assert.IsTrue(Marshal.ReadInt32(baseAddress) == buffer);
            Assert.IsTrue(bytesWritten == (IntPtr)4);

            Assert.IsTrue(ProcessMemory.Close(handle));

            Marshal.FreeHGlobal(baseAddress);
        }

        [TestMethod]
        public void WriteProcessMemory_TArray()
        {
            IntPtr baseAddress = Marshal.AllocHGlobal(16);
            int[] buffer = new int[4];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 1337;
                Marshal.WriteInt32(baseAddress, i * 4, 0);
            }

            IntPtr handle = ProcessMemory.OpenProcess(ProcessAccessFlags.All, _processId);

            Assert.IsFalse(handle == IntPtr.Zero);

            Assert.IsTrue(ProcessMemory.WriteProcessMemory(handle, baseAddress, buffer));

            for (int i = 0; i < buffer.Length; i++)
            {
                Assert.IsTrue(Marshal.ReadInt32(baseAddress, i * 4) == 1337);
                Marshal.WriteInt32(baseAddress, i * 4, 0);
            }

            IntPtr bytesWritten = IntPtr.Zero;

            Assert.IsTrue(ProcessMemory.WriteProcessMemory(handle, baseAddress, buffer, ref bytesWritten));
            Assert.IsTrue(bytesWritten == (IntPtr)16);

            for (int i = 0; i < buffer.Length; i++)
            {
                Assert.IsTrue(Marshal.ReadInt32(baseAddress, i * 4) == 1337);
            }

            Assert.IsTrue(ProcessMemory.Close(handle));

            Marshal.FreeHGlobal(baseAddress);
        }
    }
}
