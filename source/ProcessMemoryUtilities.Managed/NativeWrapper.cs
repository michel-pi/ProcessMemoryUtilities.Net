using System;
using System.Security;
using System.Runtime.CompilerServices;

using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Managed
{
    /// <summary>
    /// Provides methods that replicate the behaviour of Kernel32 by using NtDll methods and applies basic error checking.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public static partial class NativeWrapper
    {
        private static volatile bool _captureErrors = true;
        [ThreadStatic] private static uint _lastError;

        /// <summary>
        /// Determines whether to capture the "LastError".
        /// </summary>
        public static bool CaptureErrors
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _captureErrors;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _captureErrors = value;
        }

        /// <summary>
        /// Determines if the previously called method on this thread was successful.
        /// </summary>
        public static bool HasError => _lastError != 0;

        /// <summary>
        /// Returns the win32 error code set by the last failed method on this thread.
        /// </summary>
        public static int LastError
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _lastError == 0 ? 0 : (int)RtlNtStatusToDosError(_lastError);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set => _lastError = (uint)value;
        }
    }
}
