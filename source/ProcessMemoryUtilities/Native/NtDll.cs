using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

using ProcessMemoryUtilities.Internal;

using InlineIL;
using static InlineIL.IL.Emit;

namespace ProcessMemoryUtilities.Native
{
    /// <summary>
    /// Provides access to some methods of ntdll.dll
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public static partial class NtDll
    {
        private static readonly IntPtr _ntAllocateVirtualMemory;
        private static readonly IntPtr _ntClose;
        private static readonly IntPtr _ntFreeVirtualMemory;
        private static readonly IntPtr _ntOpenProcess;
        private static readonly IntPtr _ntProtectVirtualMemory;
        private static readonly IntPtr _ntReadVirtualMemory;
        private static readonly IntPtr _ntWriteVirtualMemory;
        private static readonly IntPtr _rtlNtStatusToDosError;

        static NtDll()
        {
            var lib = DynamicImport.ImportLibrary("ntdll.dll");

            _ntAllocateVirtualMemory = DynamicImport.ImportMethod(lib, "NtAllocateVirtualMemory");
            _ntClose = DynamicImport.ImportMethod(lib, "NtClose");
            _ntFreeVirtualMemory = DynamicImport.ImportMethod(lib, "NtFreeVirtualMemory");
            _ntOpenProcess = DynamicImport.ImportMethod(lib, "NtOpenProcess");
            _ntProtectVirtualMemory = DynamicImport.ImportMethod(lib, "NtProtectVirtualMemory");
            _ntReadVirtualMemory = DynamicImport.ImportMethod(lib, "NtReadVirtualMemory");
            _ntWriteVirtualMemory = DynamicImport.ImportMethod(lib, "NtWriteVirtualMemory");
            _rtlNtStatusToDosError = DynamicImport.ImportMethod(lib, "RtlNtStatusToDosError");
        }

        /// <summary>
        /// Converts the specified NTSTATUS code to its equivalent system error code.
        /// </summary>
        /// <param name="value">The NTSTATUS code to be converted.</param>
        /// <returns>The function returns the corresponding system error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RtlNtStatusToDosError(uint value)
        {
            Ldarg(nameof(value));

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_rtlNtStatusToDosError)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(uint)));

            return IL.Return<uint>();
        }

        /// <summary>
        /// Checks if the specified NTSTATUS is a success or informational type.
        /// </summary>
        /// <param name="value">A NTSTATUS value.</param>
        /// <returns>Returns true if the specified NTSTATUS is a success or informational type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NtSuccess(uint value)
            => value <= 0x3FFFFFFFu || (value >= 0x40000000u && value <= 0x7FFFFFFFu);

        /// <summary>
        /// Checks if the specified NTSTATUS is a success type.
        /// </summary>
        /// <param name="value">A NTSTATUS value.</param>
        /// <returns>Returns true if the specified NTSTATUS is a success type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NtSuccessOnly(uint value)
            => value <= 0x3FFFFFFFu;

        /// <summary>
        /// Checks if the specified NTSTATUS is a informational type.
        /// </summary>
        /// <param name="value">A NTSTATUS value.</param>
        /// <returns>Returns true if the specified NTSTATUS is a informational type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NtInformation(uint value)
            => value >= 0x40000000u && value <= 0x7FFFFFFFu;

        /// <summary>
        /// Checks if the specified NTSTATUS is a warning type.
        /// </summary>
        /// <param name="value">A NTSTATUS value.</param>
        /// <returns>Returns true if the specified NTSTATUS is a warning type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NtWarning(uint value)
            => value >= 0x80000000u && value <= 0xBFFFFFFFu;

        /// <summary>
        /// Checks if the specified NTSTATUS is a error type.
        /// </summary>
        /// <param name="value">A NTSTATUS value.</param>
        /// <returns>Returns true if the specified NTSTATUS is a error type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NtError(uint value)
            => value >= 0xC0000000u;
    }
}
