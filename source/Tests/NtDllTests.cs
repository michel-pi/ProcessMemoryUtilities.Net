using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProcessMemoryUtilities.Native;
using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Tests
{
    [TestClass]
    public class NtDllTests
    {
        private readonly int _processId;

        public NtDllTests()
        {
            _processId = Process.GetCurrentProcess().Id;
        }

        [TestMethod]
        public void TestRtlNtStatusToDosError()
        {
            Assert.IsTrue(RtlNtStatusToDosError(0) == 0);

            Assert.IsTrue(RtlNtStatusToDosError((uint)NtStatus.PARTIAL_COPY) == 299);
        }

        [TestMethod]
        public void TestNtSuccess()
        {
            Assert.IsTrue(NtSuccess(0));
            Assert.IsTrue(NtSuccess(0x3F2F2FFu));
            Assert.IsTrue(NtSuccess(0x3FFFFFFFu));
            Assert.IsTrue(NtSuccess(0x40000000u));
            Assert.IsTrue(NtSuccess(0x51234000u));
            Assert.IsTrue(NtSuccess(0x7FFFFFFFu));

            Assert.IsFalse(NtSuccess(0x80000000u));
            Assert.IsFalse(NtSuccess(0xC0000000u));
        }

        [TestMethod]
        public void TestNtSuccessOnly()
        {
            Assert.IsTrue(NtSuccessOnly(0));
            Assert.IsTrue(NtSuccessOnly(0x3F2F2FFu));
            Assert.IsTrue(NtSuccessOnly(0x3FFFFFFFu));

            Assert.IsFalse(NtSuccessOnly(0x40000000u));
            Assert.IsFalse(NtSuccessOnly(0x51234000u));
            Assert.IsFalse(NtSuccessOnly(0x7FFFFFFFu));
            Assert.IsFalse(NtSuccessOnly(0x80000000u));
            Assert.IsFalse(NtSuccessOnly(0xC0000000u));
        }

        [TestMethod]
        public void TestNtInformation()
        {
            Assert.IsFalse(NtInformation(0));
            Assert.IsFalse(NtInformation(0x3F2F2FFu));
            Assert.IsFalse(NtInformation(0x3FFFFFFFu));

            Assert.IsTrue(NtInformation(0x40000000u));
            Assert.IsTrue(NtInformation(0x51234000u));
            Assert.IsTrue(NtInformation(0x7FFFFFFFu));

            Assert.IsFalse(NtInformation(0x80000000u));
            Assert.IsFalse(NtInformation(0xC0000000u));
        }

        [TestMethod]
        public void TestNtWarning()
        {
            Assert.IsFalse(NtWarning(0));
            Assert.IsFalse(NtWarning(0x3F2F2FFu));
            Assert.IsFalse(NtWarning(0x3FFFFFFFu));
            Assert.IsFalse(NtWarning(0x40000000u));
            Assert.IsFalse(NtWarning(0x51234000u));
            Assert.IsFalse(NtWarning(0x7FFFFFFFu));

            Assert.IsTrue(NtWarning(0x80000000u));
            Assert.IsTrue(NtWarning(0xA0000000u));

            Assert.IsFalse(NtWarning(0xC0000000u));
        }

        [TestMethod]
        public void TestNtError()
        {
            Assert.IsFalse(NtError(0));
            Assert.IsFalse(NtError(0x3F2F2FFu));
            Assert.IsFalse(NtError(0x3FFFFFFFu));
            Assert.IsFalse(NtError(0x40000000u));
            Assert.IsFalse(NtError(0x51234000u));
            Assert.IsFalse(NtError(0x7FFFFFFFu));
            Assert.IsFalse(NtError(0x80000000u));
            Assert.IsFalse(NtError(0xA0000000u));

            Assert.IsTrue(NtError(0xC0000000u));
            Assert.IsTrue(NtError(0xF0000000u));
        }

        [TestMethod]
        public void TestNtClose()
        {
            Assert.IsTrue(NtSuccess(NtOpenProcess(ProcessAccessFlags.All, _processId, out var handle)));
            Assert.IsFalse(handle == IntPtr.Zero);

            Assert.IsTrue(NtSuccess(NtClose(handle)));
        }

        [TestMethod]
        public void TestNtOpenProcess()
        {
            Assert.IsTrue(NtSuccess(NtOpenProcess(ProcessAccessFlags.All, _processId, out var handle)));
            Assert.IsFalse(handle == IntPtr.Zero);

            NtClose(handle);

            Assert.IsTrue(NtSuccess(NtOpenProcess(ProcessAccessFlags.All, true, _processId, out handle)));
            Assert.IsFalse(handle == IntPtr.Zero);

            NtClose(handle);
        }

        [TestMethod]
        public void TestNtAllocateVirtualMemory()
        {
            NtOpenProcess(ProcessAccessFlags.Allocate, _processId, out var handle);

            Assert.IsTrue(NtSuccess(NtAllocateVirtualMemory(handle, (IntPtr)Environment.SystemPageSize, AllocationType.Commit | AllocationType.Reserve, MemoryProtectionFlags.ExecuteReadWrite, out var address)));
            Assert.IsFalse(address == IntPtr.Zero);

            try
            {
                Marshal.WriteInt32(address, 1337);
                Assert.IsTrue(Marshal.ReadInt32(address) == 1337);
            }
            catch
            {
                Assert.Fail();
            }

            NtFreeVirtualMemory(handle, address, IntPtr.Zero, FreeType.Release);
            NtClose(handle);
        }

        [TestMethod]
        public void TestNtFreeVirtualMemory()
        {
            NtOpenProcess(ProcessAccessFlags.Allocate, _processId, out var handle);

            NtAllocateVirtualMemory(handle, (IntPtr)Environment.SystemPageSize, AllocationType.Commit | AllocationType.Reserve, MemoryProtectionFlags.ExecuteReadWrite, out var address);

            Assert.IsTrue(NtSuccess(NtFreeVirtualMemory(handle, address, IntPtr.Zero, FreeType.Release)));

            NtClose(handle);
        }

        [TestMethod]
        public void TestNtProtectVirtualMemory()
        {
            NtOpenProcess(ProcessAccessFlags.Allocate, _processId, out var handle);

            NtAllocateVirtualMemory(handle, (IntPtr)Environment.SystemPageSize, AllocationType.Commit | AllocationType.Reserve, MemoryProtectionFlags.NoAccess, out var address);

            try
            {
                Marshal.WriteInt32(address, 1337);
                Assert.Fail();
            }
            catch
            {
            }

            Assert.IsTrue(NtSuccess(NtProtectVirtualMemory(handle, address, (IntPtr)Environment.SystemPageSize, MemoryProtectionFlags.ExecuteReadWrite, out var oldProtection)));
            Assert.IsTrue(oldProtection == MemoryProtectionFlags.NoAccess);

            try
            {
                Marshal.WriteInt32(address, 1337);
            }
            catch
            {
                Assert.Fail();
            }

            NtFreeVirtualMemory(handle, address, IntPtr.Zero, FreeType.Release);
            NtClose(handle);
        }
    }
}
