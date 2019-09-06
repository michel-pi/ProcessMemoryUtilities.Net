using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProcessMemoryUtilities.Native;
using static ProcessMemoryUtilities.Native.Kernel32;
using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Tests
{
    [TestClass]
    public class Kernel32Tests
    {
        private readonly int _processId;
        private volatile int _counter;

        private readonly Action _remoteThreadMethodDelegate;
        private IntPtr _remoteThreadMethodPointer;

        public Kernel32Tests()
        {
            _processId = Process.GetCurrentProcess().Id;

            _remoteThreadMethodDelegate = RemoteThreadMethod;
            _remoteThreadMethodPointer = Marshal.GetFunctionPointerForDelegate(_remoteThreadMethodDelegate);
        }

        private void RemoteThreadMethod()
        {
            _counter++;
        }

        [TestMethod]
        public void TestOpenProcess()
        {
            var handle = OpenProcess(ProcessAccessFlags.All, _processId);

            Assert.IsFalse(handle == IntPtr.Zero);

            NtClose(handle);
        }

        [TestMethod]
        public void TestGetRealWaitObjectResult()
        {
            Assert.IsTrue(GetRealWaitObjectResult(WaitObjectResult.Success) == WaitObjectResult.Success);
            Assert.IsTrue(GetRealWaitObjectResult(WaitObjectResult.Abandoned) == WaitObjectResult.Abandoned);
            Assert.IsTrue(GetRealWaitObjectResult(WaitObjectResult.Timeout) == WaitObjectResult.Timeout);
            Assert.IsTrue(GetRealWaitObjectResult(WaitObjectResult.Failed) == WaitObjectResult.Failed);

            Assert.IsTrue(GetRealWaitObjectResult(WaitObjectResult.Success + 1) == WaitObjectResult.Success);
            Assert.IsTrue(GetRealWaitObjectResult(WaitObjectResult.Abandoned + 1) == WaitObjectResult.Abandoned);
            Assert.IsTrue(GetRealWaitObjectResult(WaitObjectResult.Timeout + 1) == WaitObjectResult.Timeout);

            Assert.IsTrue(GetRealWaitObjectResult(WaitObjectResult.Success + 1, out var index) == WaitObjectResult.Success);
            Assert.IsTrue(index == 1);
            Assert.IsTrue(GetRealWaitObjectResult(WaitObjectResult.Abandoned + 1, out index) == WaitObjectResult.Abandoned);
            Assert.IsTrue(index == 1);
            Assert.IsTrue(GetRealWaitObjectResult(WaitObjectResult.Timeout + 1, out index) == WaitObjectResult.Timeout);
            Assert.IsTrue(index == 1);
        }

        [TestMethod]
        public void TestCreateRemoteThread()
        {
            var handle = OpenProcess(ProcessAccessFlags.Execute, _processId);

            Assert.IsFalse(handle == IntPtr.Zero);

            var oldCounter = _counter;
            var thread = CreateRemoteThreadEx(handle, IntPtr.Zero, IntPtr.Zero, _remoteThreadMethodPointer, IntPtr.Zero, ThreadCreationFlags.Immediately, IntPtr.Zero, IntPtr.Zero);

            Assert.IsFalse(thread == IntPtr.Zero);
            Assert.IsTrue(WaitForSingleObject(thread, INFINITE) == WaitObjectResult.Success);
            Assert.IsTrue(oldCounter + 1 == _counter);

            oldCounter = _counter;
            thread = CreateRemoteThreadEx(handle, IntPtr.Zero, IntPtr.Zero, _remoteThreadMethodPointer, IntPtr.Zero, ThreadCreationFlags.Immediately, IntPtr.Zero, out var threadId);

            Assert.IsFalse(thread == IntPtr.Zero);
            Assert.IsFalse(threadId == 0);
            Assert.IsTrue(WaitForSingleObject(thread, INFINITE) == WaitObjectResult.Success);
            Assert.IsTrue(oldCounter + 1 == _counter);

            NtClose(handle);
        }
    }
}
