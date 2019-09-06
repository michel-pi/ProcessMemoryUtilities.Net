using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProcessMemoryUtilities.Native;
using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Tests
{
    [TestClass]
    public class WriteVirtualMemoryTests
    {
        private struct TestStruct
        {
            public int Field1;
            public int Field2;
            public int Field3;
            public int Field4;
        }

        private readonly int _processId;

        private readonly IntPtr _memory;
        private readonly IntPtr _buffer;

        private int _fieldBuffer;
        private TestStruct _fieldStruct;

        public WriteVirtualMemoryTests()
        {
            _processId = Process.GetCurrentProcess().Id;

            _memory = Marshal.AllocHGlobal(Environment.SystemPageSize);

            for (int i = 0; i < Environment.SystemPageSize; i += 4)
            {
                Marshal.WriteInt32(_memory + i, 0);
            }

            _buffer = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(_buffer, 1337);

            _fieldBuffer = 1337;
            _fieldStruct = new TestStruct
            {
                Field1 = 1,
                Field2 = 2,
                Field3 = 3,
                Field4 = 4
            };
        }

        private bool VerifyInt()
        {
            return Marshal.ReadInt32(_memory) == 1337;
        }

        private bool VerifyTestStruct()
        {
            return Marshal.ReadInt32(_memory) == 1
                && Marshal.ReadInt32(_memory, 4) == 2
                && Marshal.ReadInt32(_memory, 8) == 3
                && Marshal.ReadInt32(_memory, 12) == 4;
        }

        private void ClearMemory()
        {
            Marshal.WriteInt32(_memory, 0);
            Marshal.WriteInt32(_memory, 4, 0);
            Marshal.WriteInt32(_memory, 8, 0);
            Marshal.WriteInt32(_memory, 12, 0);
        }

        [TestMethod]
        public void TestWriteVirtualMemory()
        {
            ClearMemory();

            NtOpenProcess(ProcessAccessFlags.Write, _processId, out var handle);

            Assert.IsTrue(NtSuccess(NtWriteVirtualMemory(handle, _memory, _buffer, (IntPtr)4, out var numberOfBytes)));

            Assert.IsTrue(numberOfBytes == (IntPtr)4);
            Assert.IsTrue(VerifyInt());

            NtClose(handle);

            ClearMemory();
        }

        [TestMethod]
        public void TestWriteVirtualMemoryGeneric()
        {
            ClearMemory();

            int stackBuffer = 1337;
            var stackStruct = new TestStruct
            {
                Field1 = 1,
                Field2 = 2,
                Field3 = 3,
                Field4 = 4
            };

            NtOpenProcess(ProcessAccessFlags.Write, _processId, out var handle);

            Assert.IsTrue(NtSuccess(NtWriteVirtualMemory(handle, _memory, ref stackBuffer, out var numberOfBytes)));

            Assert.IsTrue(numberOfBytes == (IntPtr)4);
            Assert.IsTrue(VerifyInt());

            ClearMemory();
            numberOfBytes = IntPtr.Zero;

            Assert.IsTrue(NtSuccess(NtWriteVirtualMemory(handle, _memory, ref stackStruct, out numberOfBytes)));

            Assert.IsTrue(numberOfBytes == (IntPtr)16);
            Assert.IsTrue(VerifyTestStruct());

            ClearMemory();
            numberOfBytes = IntPtr.Zero;

            Assert.IsTrue(NtSuccess(NtWriteVirtualMemory(handle, _memory, ref _fieldBuffer, out numberOfBytes)));

            Assert.IsTrue(numberOfBytes == (IntPtr)4);
            Assert.IsTrue(VerifyInt());

            ClearMemory();
            numberOfBytes = IntPtr.Zero;

            Assert.IsTrue(NtSuccess(NtWriteVirtualMemory(handle, _memory, ref _fieldStruct, out numberOfBytes)));

            Assert.IsTrue(numberOfBytes == (IntPtr)16);
            Assert.IsTrue(VerifyTestStruct());

            NtClose(handle);

            ClearMemory();
        }

        [TestMethod]
        public void TestWriteVirtualMemoryArray()
        {
            var array = new int[]
            {
                1, 2, 3, 4
            };

            ClearMemory();

            NtOpenProcess(ProcessAccessFlags.Write, _processId, out var handle);

            Assert.IsTrue(NtSuccess(NtWriteVirtualMemoryArray(handle, _memory, array, 0, array.Length, out var numberOfBytes)));

            Assert.IsTrue(numberOfBytes == (IntPtr)16);
            Assert.IsTrue(VerifyTestStruct());

            NtClose(handle);

            ClearMemory();
        }

        [TestMethod]
        public void TestWriteVirtualMemoryPartial()
        {
            var stackStruct = new TestStruct
            {
                Field1 = 1,
                Field2 = 2,
                Field3 = 3,
                Field4 = 4
            };

            ClearMemory();

            NtOpenProcess(ProcessAccessFlags.Write, _processId, out var handle);

            Assert.IsTrue(NtSuccess(NtWriteVirtualMemoryPartial(handle, _memory, ref stackStruct, 4, 4, out var numberOfBytes)));

            Assert.IsTrue(numberOfBytes == (IntPtr)4);
            Assert.IsTrue(Marshal.ReadInt32(_memory) == 2);
            Assert.IsTrue(Marshal.ReadInt32(_memory, 4) == 0);
            Assert.IsTrue(Marshal.ReadInt32(_memory, 8) == 0);
            Assert.IsTrue(Marshal.ReadInt32(_memory, 12) == 0);

            ClearMemory();

            Assert.IsTrue(NtSuccess(NtWriteVirtualMemoryPartial(handle, _memory, ref _fieldStruct, 4, 4, out numberOfBytes)));

            Assert.IsTrue(numberOfBytes == (IntPtr)4);
            Assert.IsTrue(Marshal.ReadInt32(_memory) == 2);
            Assert.IsTrue(Marshal.ReadInt32(_memory, 4) == 0);
            Assert.IsTrue(Marshal.ReadInt32(_memory, 8) == 0);
            Assert.IsTrue(Marshal.ReadInt32(_memory, 12) == 0);

            NtClose(handle);

            ClearMemory();
        }
    }
}
