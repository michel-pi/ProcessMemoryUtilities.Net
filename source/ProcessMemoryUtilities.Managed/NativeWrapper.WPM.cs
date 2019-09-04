using System;
using System.Runtime.CompilerServices;

using ProcessMemoryUtilities.Native;

using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Managed
{
    public static partial class NativeWrapper
    {
        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be written to the specified process.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size)
            => WriteProcessMemory(handle, baseAddress, buffer, size, IntPtr.Zero);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be written to the specified process.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process. This parameter is optional. If lpNumberOfBytesWritten is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
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

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be written to the specified process.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process. This parameter is optional. If lpNumberOfBytesWritten is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
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

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer) where T : unmanaged
            => WriteProcessMemory(handle, baseAddress, ref buffer, out var _);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process. This parameter is optional. If lpNumberOfBytesWritten is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
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

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer) where T : unmanaged
            => WriteProcessMemoryArray(handle, baseAddress, buffer, 0, buffer.Length, out var _);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy from.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset) where T : unmanaged
            => WriteProcessMemoryArray(handle, baseAddress, buffer, offset, buffer.Length - offset, out var _);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process. This parameter is optional. If lpNumberOfBytesWritten is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            out IntPtr numberOfBytesWritten) where T : unmanaged
            => WriteProcessMemoryArray(handle, baseAddress, buffer, 0, buffer.Length, out numberOfBytesWritten);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy from.</param>
        /// <param name="length">The number of bytes to copy from the array.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            int length) where T : unmanaged
            => WriteProcessMemoryArray(handle, baseAddress, buffer, offset, length, out var _);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy from.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process. This parameter is optional. If lpNumberOfBytesWritten is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            out IntPtr numberOfBytesWritten) where T : unmanaged
            => WriteProcessMemoryArray(handle, baseAddress, buffer, offset, buffer.Length - offset, out numberOfBytesWritten);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy from.</param>
        /// <param name="length">The number of bytes to copy from the array.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process. This parameter is optional. If lpNumberOfBytesWritten is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
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

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy from.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset) where T : unmanaged
            => WriteProcessMemoryPartial(handle, baseAddress, ref buffer, offset, out var _);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy from.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            int length) where T : unmanaged
            => WriteProcessMemoryPartial(handle, baseAddress, ref buffer, offset, length, out var _);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy from.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            out IntPtr numberOfBytesWritten) where T : unmanaged
            => WriteProcessMemoryPartial(handle, baseAddress, ref buffer, offset, InternalHelper.SizeOf<T>() - offset, out numberOfBytesWritten);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy from.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
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
