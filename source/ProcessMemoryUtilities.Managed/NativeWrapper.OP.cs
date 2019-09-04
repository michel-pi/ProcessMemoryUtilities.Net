using System;
using System.Runtime.CompilerServices;

using ProcessMemoryUtilities.Native;

using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Managed
{
    public static partial class NativeWrapper
    {
        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="handle">A valid handle to an open object.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CloseHandle(IntPtr handle)
        {
            uint status = NtClose(handle);

            if (CaptureErrors) LastError = (int)status;

            return NtSuccess(status);
        }

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="desiredAccess">The access to the process object. This access right is checked against the security descriptor for the process.</param>
        /// <param name="inheritHandle">If this value is TRUE, processes created by this process will inherit the handle. Otherwise, the processes do not inherit this handle.</param>
        /// <param name="processId">The identifier of the local process to be opened.</param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified process. If the function fails, the return value is IntPtr.Zero. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr OpenProcess(ProcessAccessFlags desiredAccess, bool inheritHandle, int processId)
        {
            uint status = NtOpenProcess(desiredAccess, inheritHandle, processId, out var handle);

            if (CaptureErrors) LastError = (int)status;

            return handle;
        }

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="desiredAccess">The access to the process object. This access right is checked against the security descriptor for the process.</param>
        /// <param name="processId">The identifier of the local process to be opened.</param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified process. If the function fails, the return value is IntPtr.Zero. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr OpenProcess(ProcessAccessFlags desiredAccess, int processId)
        {
            uint status = NtOpenProcess(desiredAccess, processId, out var handle);

            if (CaptureErrors) LastError = (int)status;

            return handle;
        }
    }
}
