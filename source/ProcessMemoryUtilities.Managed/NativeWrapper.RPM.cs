using System;
using System.Runtime.CompilerServices;

using ProcessMemoryUtilities.Native;

using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Managed
{
    public static partial class NativeWrapper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size)
            => ReadProcessMemory(handle, baseAddress, buffer, size, IntPtr.Zero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size,
            IntPtr numberOfBytesRead)
        {
            var result = ReadProcessMemory(
                handle,
                baseAddress,
                buffer,
                size,
                out var tmp);

            if (numberOfBytesRead != IntPtr.Zero) InternalHelper.WriteIntPtr(numberOfBytesRead, tmp);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size,
            out IntPtr numberOfBytesRead)
        {
            uint status = NtReadVirtualMemory(
                handle,
                baseAddress,
                buffer,
                size,
                out numberOfBytesRead);

            if (NtSuccess(status))
            {
                if (CaptureErrors) LastError = 0;
                return true;
            }

            if (status != (uint)NtStatus.PARTIAL_COPY)
            {
                if (CaptureErrors) LastError = (int)status;
                return false;
            }

            IntPtr initialSize = size;

            int offset = numberOfBytesRead.ToInt32();

            baseAddress += offset;
            buffer += offset;
            size -= offset;

            while (size != IntPtr.Zero)
            {
                status = NtReadVirtualMemory(
                    handle,
                    baseAddress,
                    buffer,
                    size,
                    out numberOfBytesRead);

                if (NtSuccess(status))
                {
                    numberOfBytesRead = initialSize;

                    if (CaptureErrors) LastError = 0;
                    return true;
                }
                else if (status == (uint)NtStatus.PARTIAL_COPY)
                {
                    offset = numberOfBytesRead.ToInt32();

                    baseAddress += offset;
                    buffer += offset;
                    size -= offset;
                }
                else
                {
                    if (CaptureErrors) LastError = (int)status;
                    return false;
                }
            }

            numberOfBytesRead = IntPtr.Zero;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer) where T : unmanaged
            => ReadProcessMemory(handle, baseAddress, ref buffer, out var _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            out IntPtr numberOfBytesRead) where T : unmanaged
        {
            uint status = NtReadVirtualMemory(
                handle,
                baseAddress,
                ref buffer,
                out numberOfBytesRead);

            if (NtSuccess(status))
            {
                if (CaptureErrors) LastError = 0;
                return true;
            }

            if (status != (uint)NtStatus.PARTIAL_COPY)
            {
                if (CaptureErrors) LastError = (int)status;
                return false;
            }

            int bytesRead = numberOfBytesRead.ToInt32();

            var result = ReadProcessMemoryPartial(
                handle,
                baseAddress + bytesRead,
                ref buffer,
                bytesRead,
                InternalHelper.SizeOf<T>() - bytesRead,
                out numberOfBytesRead);

            if (result) numberOfBytesRead = (IntPtr)InternalHelper.SizeOf<T>();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer) where T : unmanaged
            => ReadProcessMemoryArray(handle, baseAddress, buffer, 0, buffer.Length, out var _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset) where T : unmanaged
            => ReadProcessMemoryArray(handle, baseAddress, buffer, offset, buffer.Length - offset, out var _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            out IntPtr numberOfBytesRead) where T : unmanaged
            => ReadProcessMemoryArray(handle, baseAddress, buffer, 0, buffer.Length, out numberOfBytesRead);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            int length) where T : unmanaged
            => ReadProcessMemoryArray(handle, baseAddress, buffer, offset, length, out var _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            out IntPtr numberOfBytesRead) where T : unmanaged
            => ReadProcessMemoryArray(handle, baseAddress, buffer, offset, buffer.Length - offset, out numberOfBytesRead);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            int length,
            out IntPtr numberOfBytesRead) where T : unmanaged
        {
            uint status = NtReadVirtualMemoryArray(
                handle,
                baseAddress,
                buffer,
                offset,
                length,
                out numberOfBytesRead);

            if (NtSuccess(status))
            {
                if (CaptureErrors) LastError = 0;
                return true;
            }

            if (status != (uint)NtStatus.PARTIAL_COPY)
            {
                if (CaptureErrors) LastError = (int)status;
                return false;
            }

            int bytesRead = numberOfBytesRead.ToInt32();

            bool result = ReadProcessMemoryPartial(
                handle,
                baseAddress + bytesRead,
                ref buffer[0],
                bytesRead,
                (length * InternalHelper.SizeOf<T>()) - bytesRead);

            if (result) numberOfBytesRead = (IntPtr)(length * InternalHelper.SizeOf<T>());

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset) where T : unmanaged
            => ReadProcessMemoryPartial(handle, baseAddress, ref buffer, offset, out var _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            int length) where T : unmanaged
            => ReadProcessMemoryPartial(handle, baseAddress, ref buffer, offset, length, out var _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            out IntPtr numberOfBytesRead) where T : unmanaged
            => ReadProcessMemoryPartial(handle, baseAddress, ref buffer, offset, InternalHelper.SizeOf<T>() - offset, out numberOfBytesRead);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            int length,
            out IntPtr numberOfBytesRead) where T : unmanaged
        {
            uint status = NtReadVirtualMemoryPartial(
                handle,
                baseAddress,
                ref buffer,
                offset,
                length,
                out numberOfBytesRead);

            if (NtSuccess(status))
            {
                if (CaptureErrors) LastError = 0;
                return true;
            }

            if (status != (uint)NtStatus.PARTIAL_COPY)
            {
                if (CaptureErrors) LastError = (int)status;
                return false;
            }

            IntPtr initialSize = (IntPtr)length;

            int previousRead = numberOfBytesRead.ToInt32();

            baseAddress += previousRead;
            offset += previousRead;
            length -= previousRead;

            while (length > 0)
            {
                status = NtReadVirtualMemoryPartial(
                    handle,
                    baseAddress,
                    ref buffer,
                    offset,
                    length,
                    out numberOfBytesRead);

                if (NtSuccess(status))
                {
                    numberOfBytesRead = initialSize;

                    if (CaptureErrors) LastError = 0;
                    return true;
                }
                else if (status == (uint)NtStatus.PARTIAL_COPY)
                {
                    previousRead = numberOfBytesRead.ToInt32();

                    baseAddress += previousRead;
                    offset += previousRead;
                    length -= previousRead;
                }
                else
                {
                    if (CaptureErrors) LastError = (int)status;
                    return false;
                }
            }

            numberOfBytesRead = IntPtr.Zero;
            return false;
        }
    }
}
