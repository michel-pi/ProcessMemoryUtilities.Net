using System;
using System.Runtime.CompilerServices;

using ProcessMemoryUtilities.Native;

using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Managed
{
    public static partial class NativeWrapper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size)
            => WriteProcessMemory(handle, baseAddress, buffer, size, IntPtr.Zero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size,
            IntPtr numberOfBytesWritten)
        {
            var result = WriteProcessMemory(
                handle,
                baseAddress,
                buffer,
                size,
                out var tmp);

            if (numberOfBytesWritten != IntPtr.Zero) InternalHelper.WriteIntPtr(numberOfBytesWritten, tmp);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size,
            out IntPtr numberOfBytesWritten)
        {
            uint status = NtWriteVirtualMemory(
                handle,
                baseAddress,
                buffer,
                size,
                out numberOfBytesWritten);

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

            int offset = numberOfBytesWritten.ToInt32();

            baseAddress += offset;
            buffer += offset;
            size -= offset;

            while (size != IntPtr.Zero)
            {
                status = NtWriteVirtualMemory(
                    handle,
                    baseAddress,
                    buffer,
                    size,
                    out numberOfBytesWritten);

                if (NtSuccess(status))
                {
                    numberOfBytesWritten = initialSize;

                    if (CaptureErrors) LastError = 0;
                    return true;
                }
                else if (status == (uint)NtStatus.PARTIAL_COPY)
                {
                    offset = numberOfBytesWritten.ToInt32();

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

            numberOfBytesWritten = IntPtr.Zero;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer) where T : unmanaged
            => WriteProcessMemory(handle, baseAddress, ref buffer, out var _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            out IntPtr numberOfBytesWritten) where T : unmanaged
        {
            uint status = NtWriteVirtualMemory(
                handle,
                baseAddress,
                ref buffer,
                out numberOfBytesWritten);

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

            int bytesWritten = numberOfBytesWritten.ToInt32();

            var result = WriteProcessMemoryPartial(
                handle,
                baseAddress + bytesWritten,
                ref buffer,
                bytesWritten,
                InternalHelper.SizeOf<T>() - bytesWritten,
                out numberOfBytesWritten);

            if (result) numberOfBytesWritten = (IntPtr)InternalHelper.SizeOf<T>();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer) where T : unmanaged
            => WriteProcessMemoryArray(handle, baseAddress, buffer, 0, buffer.Length, out var _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset) where T : unmanaged
            => WriteProcessMemoryArray(handle, baseAddress, buffer, offset, buffer.Length - offset, out var _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            out IntPtr numberOfBytesWritten) where T : unmanaged
            => WriteProcessMemoryArray(handle, baseAddress, buffer, 0, buffer.Length, out numberOfBytesWritten);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            int length) where T : unmanaged
            => WriteProcessMemoryArray(handle, baseAddress, buffer, offset, length, out var _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            out IntPtr numberOfBytesWritten) where T : unmanaged
            => WriteProcessMemoryArray(handle, baseAddress, buffer, offset, buffer.Length - offset, out numberOfBytesWritten);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            int length,
            out IntPtr numberOfBytesWritten) where T : unmanaged
        {
            uint status = NtWriteVirtualMemoryArray(
                handle,
                baseAddress,
                buffer,
                offset,
                length,
                out numberOfBytesWritten);

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

            int bytesWritten = numberOfBytesWritten.ToInt32();

            bool result = WriteProcessMemoryPartial(
                handle,
                baseAddress + bytesWritten,
                ref buffer[0],
                bytesWritten,
                (length * InternalHelper.SizeOf<T>()) - bytesWritten);

            if (result) numberOfBytesWritten = (IntPtr)(length * InternalHelper.SizeOf<T>());

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset) where T : unmanaged
            => WriteProcessMemoryPartial(handle, baseAddress, ref buffer, offset, out var _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            int length) where T : unmanaged
            => WriteProcessMemoryPartial(handle, baseAddress, ref buffer, offset, length, out var _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            out IntPtr numberOfBytesWritten) where T : unmanaged
            => WriteProcessMemoryPartial(handle, baseAddress, ref buffer, offset, InternalHelper.SizeOf<T>() - offset, out numberOfBytesWritten);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            int length,
            out IntPtr numberOfBytesWritten) where T : unmanaged
        {
            uint status = NtWriteVirtualMemoryPartial(
                handle,
                baseAddress,
                ref buffer,
                offset,
                length,
                out numberOfBytesWritten);

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

            int previousWrite = numberOfBytesWritten.ToInt32();

            baseAddress += previousWrite;
            offset += previousWrite;
            length -= previousWrite;

            while (length > 0)
            {
                status = NtWriteVirtualMemoryPartial(
                    handle,
                    baseAddress,
                    ref buffer,
                    offset,
                    length,
                    out numberOfBytesWritten);

                if (NtSuccess(status))
                {
                    numberOfBytesWritten = initialSize;

                    if (CaptureErrors) LastError = 0;
                    return true;
                }
                else if (status == (uint)NtStatus.PARTIAL_COPY)
                {
                    previousWrite = numberOfBytesWritten.ToInt32();

                    baseAddress += previousWrite;
                    offset += previousWrite;
                    length -= previousWrite;
                }
                else
                {
                    if (CaptureErrors) LastError = (int)status;
                    return false;
                }
            }

            numberOfBytesWritten = IntPtr.Zero;
            return false;
        }
    }
}
