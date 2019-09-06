using System;
using System.Runtime.CompilerServices;

using ProcessMemoryUtilities.Native;

using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Managed
{
    public static partial class NativeWrapper
    {
        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size)
            => ReadProcessMemory(handle, baseAddress, buffer, size, IntPtr.Zero);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
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

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer. If lpNumberOfBytesRead is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
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

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer) where T : unmanaged
            => ReadProcessMemory(handle, baseAddress, ref buffer, out var _);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer. If lpNumberOfBytesRead is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
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

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer) where T : unmanaged
            => ReadProcessMemoryArray(handle, baseAddress, buffer, 0, buffer.Length, out var _);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy to.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset) where T : unmanaged
            => ReadProcessMemoryArray(handle, baseAddress, buffer, offset, buffer.Length - offset, out var _);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer. If lpNumberOfBytesRead is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            out IntPtr numberOfBytesRead) where T : unmanaged
            => ReadProcessMemoryArray(handle, baseAddress, buffer, 0, buffer.Length, out numberOfBytesRead);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy to.</param>
        /// <param name="length">The number of bytes to copy to the array.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            int length) where T : unmanaged
            => ReadProcessMemoryArray(handle, baseAddress, buffer, offset, length, out var _);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy to.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer. If lpNumberOfBytesRead is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            out IntPtr numberOfBytesRead) where T : unmanaged
            => ReadProcessMemoryArray(handle, baseAddress, buffer, offset, buffer.Length - offset, out numberOfBytesRead);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy to.</param>
        /// <param name="length">The number of bytes to copy to the array.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer. If lpNumberOfBytesRead is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
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

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from the start of the buffer.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset) where T : unmanaged
            => ReadProcessMemoryPartial(handle, baseAddress, ref buffer, offset, out var _);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from the start of the buffer.</param>
        /// <param name="length">The number of bytes to copy to the buffer.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            int length) where T : unmanaged
            => ReadProcessMemoryPartial(handle, baseAddress, ref buffer, offset, length, out var _);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from the start of the buffer.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            out IntPtr numberOfBytesRead) where T : unmanaged
            => ReadProcessMemoryPartial(handle, baseAddress, ref buffer, offset, InternalHelper.SizeOf<T>() - offset, out numberOfBytesRead);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from the start of the buffer.</param>
        /// <param name="length">The number of bytes to copy to the buffer.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns true when the function succeeds; otherwise false. To get extended error information, call GetLastError.</returns>
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
