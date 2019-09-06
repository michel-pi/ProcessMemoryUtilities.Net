using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProcessMemoryUtilities.Native;
using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Tests
{
    [TestClass]
    public class ReadVirtualMemoryTests
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

        public ReadVirtualMemoryTests()
        {
            _processId = Process.GetCurrentProcess().Id;

            _memory = Marshal.AllocHGlobal(Environment.SystemPageSize);

            for (int i = 0; i < Environment.SystemPageSize / 4; i++)
            {
                Marshal.WriteInt32(_memory + (i * 4), i + 1);
            }

            _buffer = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(_buffer, 0);

            _fieldBuffer = 0;
            _fieldStruct = default;
        }

        [TestMethod]
        public void TestReadVirtualMemory()
        {
            NtOpenProcess(ProcessAccessFlags.Read, _processId, out var handle);

            Assert.IsTrue(NtSuccess(NtReadVirtualMemory(handle, _memory, _buffer, (IntPtr)4, IntPtr.Zero)));

            Assert.IsTrue(Marshal.ReadInt32(_buffer) == 1);

            Marshal.WriteInt32(_buffer, 0);

            Assert.IsTrue(NtSuccess(NtReadVirtualMemory(handle, _memory, _buffer, (IntPtr)4, out var bytesRead)));

            Assert.IsTrue(Marshal.ReadInt32(_buffer) == 1);
            Assert.IsTrue(bytesRead == (IntPtr)4);

            NtClose(handle);

            Marshal.WriteInt32(_buffer, 0);
        }

        [TestMethod]
        public void TestReadVirtualMemoryGeneric()
        {
            int stackBuffer = default;

            NtOpenProcess(ProcessAccessFlags.Read, _processId, out var handle);

            Assert.IsTrue(NtSuccess(NtReadVirtualMemory(handle, _memory, ref stackBuffer)));

            Assert.IsTrue(stackBuffer == 1);

            stackBuffer = 0;

            Assert.IsTrue(NtSuccess(NtReadVirtualMemory(handle, _memory, ref stackBuffer, out var bytesRead)));

            Assert.IsTrue(stackBuffer == 1);
            Assert.IsTrue(bytesRead == (IntPtr)4);

            Assert.IsTrue(NtSuccess(NtReadVirtualMemory(handle, _memory, ref _fieldBuffer)));

            Assert.IsTrue(_fieldBuffer == 1);

            _fieldBuffer = 0;

            Assert.IsTrue(NtSuccess(NtReadVirtualMemory(handle, _memory, ref _fieldBuffer, out bytesRead)));

            Assert.IsTrue(_fieldBuffer == 1);
            Assert.IsTrue(bytesRead == (IntPtr)4);

            NtClose(handle);

            _fieldBuffer = default;
        }

        [TestMethod]
        public void TestReadVirtualMemoryArray()
        {
            int[] array = new int[4];

            NtOpenProcess(ProcessAccessFlags.Read, _processId, out var handle);

            Assert.IsTrue(NtSuccess(NtReadVirtualMemoryArray(handle, _memory, array, 0, array.Length)));

            for (int i = 0; i < array.Length; i++)
            {
                Assert.IsTrue(array[i] == i + 1);
            }

            array = new int[4];

            Assert.IsTrue(NtSuccess(NtReadVirtualMemoryArray(handle, _memory, array, 0, array.Length, out var bytesRead)));

            for (int i = 0; i < array.Length; i++)
            {
                Assert.IsTrue(array[i] == i + 1);
            }

            Assert.IsTrue(bytesRead == (IntPtr)(array.Length * 4));

            NtClose(handle);
        }

        [TestMethod]
        public void TestReadVirtualMemoryPartial()
        {
            TestStruct stackStruct = default;

            NtOpenProcess(ProcessAccessFlags.Read, _processId, out var handle);

            Assert.IsTrue(NtSuccess(NtReadVirtualMemoryPartial(handle, _memory, ref stackStruct, 4, 4, out var bytesRead)));

            Assert.IsTrue(bytesRead == (IntPtr)4);
            Assert.IsTrue(stackStruct.Field1 == 0);
            Assert.IsTrue(stackStruct.Field2 == 1);
            Assert.IsTrue(stackStruct.Field3 == 0);
            Assert.IsTrue(stackStruct.Field4 == 0);

            Assert.IsTrue(NtSuccess(NtReadVirtualMemoryPartial(handle, _memory, ref _fieldStruct, 8, 4, out bytesRead)));

            Assert.IsTrue(bytesRead == (IntPtr)4);
            Assert.IsTrue(_fieldStruct.Field1 == 0);
            Assert.IsTrue(_fieldStruct.Field2 == 0);
            Assert.IsTrue(_fieldStruct.Field3 == 1);
            Assert.IsTrue(_fieldStruct.Field4 == 0);

            NtClose(handle);

            _fieldStruct = default;
        }
    }
}
