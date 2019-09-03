using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using InlineIL;
using static InlineIL.IL.Emit;

namespace ProcessMemoryUtilities.Native
{
    public static partial class NtDll
    {
        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be written to the specified process.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process. This parameter is optional. If lpNumberOfBytesWritten is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size,
            IntPtr numberOfBytesWritten)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));
            Ldarg(nameof(size));
            Ldarg(nameof(numberOfBytesWritten));

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr)));

            return IL.Return<uint>();
        }

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be written to the specified process.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size,
            out IntPtr numberOfBytesWritten)
        {
            IL.DeclareLocals(
                new LocalVar("result", typeof(uint)),
                new LocalVar("localBytesRead", typeof(IntPtr)));

            numberOfBytesWritten = default;

            Ldloca("localBytesRead");
            Initobj(typeof(IntPtr));

            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));
            Ldarg(nameof(size));
            Ldloca("localBytesRead");
            Conv_U();

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr).MakePointerType()));

            Stloc("result");

            Ldarg(nameof(numberOfBytesWritten));
            Ldloc("localBytesRead");
            Stind_I();

            Ldloc("result");
            return IL.Return<uint>();
        }

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be written to the specified process.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size)
            => NtWriteVirtualMemory(handle, baseAddress, buffer, size, default);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemory<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer) where T : unmanaged
        {
            IL.DeclareLocals(
                new LocalVar("pinnedBuffer", typeof(T).MakeByRefType()).Pinned());

            Ldarg(nameof(buffer));
            Stloc("pinnedBuffer");

            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldloc("pinnedBuffer");
            Conv_U();

            Sizeof(typeof(T));
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(void).MakePointerType(),
                typeof(IntPtr),
                typeof(IntPtr)));

            return IL.Return<uint>();
        }

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process. This parameter is optional. If lpNumberOfBytesWritten is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemory<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            out IntPtr numberOfBytesWritten) where T : unmanaged
        {
            IL.DeclareLocals(
                new LocalVar("result", typeof(uint)),
                new LocalVar("localNumberOfBytes", typeof(IntPtr)),
                new LocalVar("pinnedBuffer", typeof(T).MakeByRefType()).Pinned());

            numberOfBytesWritten = default;

            Ldloca("localNumberOfBytes");
            Initobj(typeof(IntPtr));

            Ldarg(nameof(buffer));
            Stloc("pinnedBuffer");

            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldloc("pinnedBuffer");
            Conv_U();

            Sizeof(typeof(T));
            Conv_I();

            Ldloca("localNumberOfBytes");
            Conv_U();

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(void).MakePointerType(),
                typeof(IntPtr),
                typeof(IntPtr).MakePointerType()));

            Stloc("result");

            Ldarg(nameof(numberOfBytesWritten));
            Ldloc("localNumberOfBytes");
            Stind_I();

            Ldloc("result");
            return IL.Return<uint>();
        }

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy from.</param>
        /// <param name="length">The number of bytes to copy from the array.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            int length) where T : unmanaged
        {
            IL.DeclareLocals(
                new LocalVar("pinnedBuffer", typeof(T).MakeByRefType()).Pinned());

            Ldarg(nameof(buffer));
            Ldarg(nameof(offset));
            Ldelema(typeof(T));
            Stloc("pinnedBuffer");

            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldloc("pinnedBuffer");
            Conv_U();

            Sizeof(typeof(T));
            Ldarg(nameof(length));
            Mul();
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(void).MakePointerType(),
                typeof(IntPtr),
                typeof(IntPtr)));

            return IL.Return<uint>();
        }

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy from.</param>
        /// <param name="length">The number of bytes to copy from the array.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified process. This parameter is optional. If lpNumberOfBytesWritten is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            int length,
            out IntPtr numberOfBytesWritten) where T : unmanaged
        {
            IL.DeclareLocals(
                new LocalVar("result", typeof(uint)),
                new LocalVar("localNumberOfBytes", typeof(IntPtr)),
                new LocalVar("pinnedBuffer", typeof(T).MakeByRefType()).Pinned());

            numberOfBytesWritten = default;

            Ldloca("localNumberOfBytes");
            Initobj(typeof(IntPtr));

            Ldarg(nameof(buffer));
            Ldarg(nameof(offset));
            Ldelema(typeof(T));
            Stloc("pinnedBuffer");

            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldloc("pinnedBuffer");
            Conv_U();

            Sizeof(typeof(T));
            Ldarg(nameof(length));
            Mul();
            Conv_I();

            Ldloca("localNumberOfBytes");
            Conv_U();

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(void).MakePointerType(),
                typeof(IntPtr),
                typeof(IntPtr).MakePointerType()));

            Stloc("result");

            Ldarg(nameof(numberOfBytesWritten));
            Ldloc("localNumberOfBytes");
            Stind_I();

            Ldloc("result");
            return IL.Return<uint>();
        }

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy from.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset) where T : unmanaged
            => NtWriteVirtualMemoryArray(handle, baseAddress, buffer, offset, buffer.Length - offset);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy from.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            out IntPtr numberOfBytesWritten) where T : unmanaged
            => NtWriteVirtualMemoryArray(handle, baseAddress, buffer, offset, buffer.Length - offset, out numberOfBytesWritten);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer) where T : unmanaged
            => NtWriteVirtualMemoryArray(handle, baseAddress, buffer, 0, buffer.Length);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            out IntPtr numberOfBytesWritten) where T : unmanaged
            => NtWriteVirtualMemoryArray(handle, baseAddress, buffer, 0, buffer.Length, out numberOfBytesWritten);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from the start of the buffer.</param>
        /// <param name="length">The number of bytes to copy to the buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            int length) where T : unmanaged
        {
            IL.DeclareLocals(
                new LocalVar("pinnedBuffer", typeof(T).MakeByRefType()).Pinned(),
                new LocalVar("ptr", typeof(byte).MakePointerType()));

            Ldarg(nameof(buffer));
            Stloc("pinnedBuffer");

            Ldloc("pinnedBuffer");
            Conv_U();
            Stloc("ptr");

            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldloc("ptr");
            Ldarg(nameof(offset));
            Add();

            Ldarg(nameof(length));
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(byte).MakePointerType(),
                typeof(IntPtr),
                typeof(IntPtr)));

            return IL.Return<uint>();
        }

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from the start of the buffer.</param>
        /// <param name="length">The number of bytes to copy to the buffer.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            int length,
            out IntPtr numberOfBytesWritten) where T : unmanaged
        {
            IL.DeclareLocals(
                new LocalVar("result", typeof(uint)),
                new LocalVar("localNumberOfBytes", typeof(IntPtr)),
                new LocalVar("pinnedBuffer", typeof(T).MakeByRefType()).Pinned(),
                new LocalVar("ptr", typeof(byte).MakePointerType()));

            numberOfBytesWritten = default;

            Ldloca("localNumberOfBytes");
            Initobj(typeof(IntPtr));

            Ldarg(nameof(buffer));
            Stloc("pinnedBuffer");

            Ldloc("pinnedBuffer");
            Conv_U();
            Stloc("ptr");

            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldloc("ptr");
            Ldarg(nameof(offset));
            Add();

            Ldarg(nameof(length));
            Conv_I();

            Ldloca("localNumberOfBytes");
            Conv_U();

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(byte).MakePointerType(),
                typeof(IntPtr),
                typeof(IntPtr).MakePointerType()));

            Stloc("result");

            Ldarg(nameof(numberOfBytesWritten));
            Ldloc("localNumberOfBytes");
            Stind_I();

            Ldloc("result");
            return IL.Return<uint>();
        }

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from the start of the buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset) where T : unmanaged
            => NtWriteVirtualMemoryPartial(handle, baseAddress, ref buffer, offset, SizeOfHelper<T>() - offset);

        /// <summary>
        /// WriteProcessMemory copies the data from the specified buffer in the current process to the address range of the specified process. Any process that has a handle with PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process to be written to can call the function. Typically but not always, the process with address space that is being written to is being debugged. The entire area to be written to must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">A byte offset from the start of the buffer.</param>
        /// <param name="numberOfBytesWritten">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtWriteVirtualMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            out IntPtr numberOfBytesWritten) where T : unmanaged
            => NtWriteVirtualMemoryPartial(handle, baseAddress, ref buffer, offset, SizeOfHelper<T>() - offset, out numberOfBytesWritten);
    }
}
