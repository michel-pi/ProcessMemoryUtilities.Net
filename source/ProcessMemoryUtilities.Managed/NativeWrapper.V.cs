using System;
using System.Runtime.CompilerServices;

using ProcessMemoryUtilities.Native;

using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Managed
{
    public static partial class NativeWrapper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr VirtualAllocEx(IntPtr handle, IntPtr address, IntPtr size, AllocationType allocationType, MemoryProtectionFlags memoryProtection)
        {
            uint status = NtAllocateVirtualMemory(handle, size, allocationType, memoryProtection, out address);

            if (CaptureErrors) LastError = (int)status;

            return NtSuccess(status) ? address : IntPtr.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VirtualFreeEx(IntPtr handle, IntPtr address, IntPtr size, FreeType freeType)
        {
            uint status = NtFreeVirtualMemory(handle, address, size, freeType);

            if (CaptureErrors) LastError = (int)status;

            return NtSuccess(status);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VirtualProtectEx(IntPtr handle, IntPtr address, IntPtr size, MemoryProtectionFlags newProtect, out MemoryProtectionFlags oldProtect)
        {
            uint status = NtProtectVirtualMemory(handle, address, size, newProtect, out oldProtect);

            if (CaptureErrors) LastError = (int)status;

            return NtSuccess(status);
        }
    }
}
