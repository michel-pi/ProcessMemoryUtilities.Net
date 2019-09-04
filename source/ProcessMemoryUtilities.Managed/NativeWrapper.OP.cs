using System;
using System.Runtime.CompilerServices;

using ProcessMemoryUtilities.Native;

using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Managed
{
    public static partial class NativeWrapper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CloseHandle(IntPtr handle)
        {
            uint status = NtClose(handle);

            if (CaptureErrors) LastError = (int)status;

            return NtSuccess(status);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr OpenProcess(ProcessAccessFlags desiredAccess, bool inheritHandle, int processId)
        {
            uint status = NtOpenProcess(desiredAccess, inheritHandle, processId, out var handle);

            if (CaptureErrors) LastError = (int)status;

            return handle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr OpenProcess(ProcessAccessFlags desiredAccess, int processId)
        {
            uint status = NtOpenProcess(desiredAccess, processId, out var handle);

            if (CaptureErrors) LastError = (int)status;

            return handle;
        }
    }
}
