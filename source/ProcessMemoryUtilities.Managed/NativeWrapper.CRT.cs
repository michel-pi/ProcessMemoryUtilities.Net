using System;
using System.Runtime.CompilerServices;

using ProcessMemoryUtilities.Native;

namespace ProcessMemoryUtilities.Managed
{
    public static partial class NativeWrapper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThread(
            IntPtr handle,
            IntPtr threadAttributes,
            IntPtr stackSize,
            IntPtr startAddress,
            IntPtr parameter,
            ThreadCreationFlags creationFlags,
            IntPtr threadId)
            => Kernel32.CreateRemoteThreadEx(handle, threadAttributes, stackSize, startAddress, parameter, creationFlags, IntPtr.Zero, threadId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThread(
            IntPtr handle,
            IntPtr threadAttributes,
            IntPtr stackSize,
            IntPtr startAddress,
            IntPtr parameter,
            ThreadCreationFlags creationFlags,
            out uint threadId)
            => Kernel32.CreateRemoteThreadEx(handle, threadAttributes, stackSize, startAddress, parameter, creationFlags, IntPtr.Zero, out threadId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThread(
            IntPtr handle,
            IntPtr startAddress,
            IntPtr parameter,
            ThreadCreationFlags creationFlags)
            => Kernel32.CreateRemoteThreadEx(handle, startAddress, parameter, creationFlags);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThread(
            IntPtr handle,
            IntPtr startAddress,
            IntPtr parameter,
            ThreadCreationFlags creationFlags,
            out uint threadId)
            => Kernel32.CreateRemoteThreadEx(handle, startAddress, parameter, creationFlags, out threadId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThread(
            IntPtr handle,
            IntPtr startAddress,
            IntPtr parameter)
            => Kernel32.CreateRemoteThreadEx(handle, startAddress, parameter);
    }
}
