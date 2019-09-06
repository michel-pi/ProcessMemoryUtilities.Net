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
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer. If lpNumberOfBytesRead is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size,
            IntPtr numberOfBytesRead)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));
            Ldarg(nameof(size));
            Ldarg(nameof(numberOfBytesRead));

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntReadVirtualMemory)));
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
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size,
            out IntPtr numberOfBytesRead)
        {
            IL.DeclareLocals(
                new LocalVar("result", typeof(uint)),
                new LocalVar("localBytesRead", typeof(IntPtr)));

            numberOfBytesRead = default;

            Ldloca("localBytesRead");
            Initobj(typeof(IntPtr));

            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));
            Ldarg(nameof(size));
            Ldloca("localBytesRead");
            Conv_U();

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr).MakePointerType()));

            Stloc("result");

            Ldarg(nameof(numberOfBytesRead));
            Ldloc("localBytesRead");
            Stind_I();

            Ldloc("result");
            return IL.Return<uint>();
        }

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemory(
            IntPtr handle,
            IntPtr baseAddress,
            IntPtr buffer,
            IntPtr size)
            => NtReadVirtualMemory(handle, baseAddress, buffer, size, default);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemory<T>(
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

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntReadVirtualMemory)));
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
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer. If lpNumberOfBytesRead is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemory<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            out IntPtr numberOfBytesRead) where T : unmanaged
        {
            IL.DeclareLocals(
                new LocalVar("result", typeof(uint)),
                new LocalVar("localNumberOfBytes", typeof(IntPtr)),
                new LocalVar("pinnedBuffer", typeof(T).MakeByRefType()).Pinned());

            numberOfBytesRead = default;

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

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(void).MakePointerType(),
                typeof(IntPtr),
                typeof(IntPtr).MakePointerType()));

            Stloc("result");

            Ldarg(nameof(numberOfBytesRead));
            Ldloc("localNumberOfBytes");
            Stind_I();

            Ldloc("result");
            return IL.Return<uint>();
        }

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy to.</param>
        /// <param name="length">The number of bytes to copy to the array.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemoryArray<T>(
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

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntReadVirtualMemory)));
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
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy to.</param>
        /// <param name="length">The number of bytes to copy to the array.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer. If lpNumberOfBytesRead is IntPtr.Zero, the parameter is ignored.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            int length,
            out IntPtr numberOfBytesRead) where T : unmanaged
        {
            IL.DeclareLocals(
                new LocalVar("result", typeof(uint)),
                new LocalVar("localNumberOfBytes", typeof(IntPtr)),
                new LocalVar("pinnedBuffer", typeof(T).MakeByRefType()).Pinned());

            numberOfBytesRead = default;

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

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(void).MakePointerType(),
                typeof(IntPtr),
                typeof(IntPtr).MakePointerType()));

            Stloc("result");

            Ldarg(nameof(numberOfBytesRead));
            Ldloc("localNumberOfBytes");
            Stind_I();

            Ldloc("result");
            return IL.Return<uint>();
        }

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy to.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset) where T : unmanaged
            => NtReadVirtualMemoryArray(handle, baseAddress, buffer, offset, buffer.Length - offset);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from inside the array to copy to.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            int offset,
            out IntPtr numberOfBytesRead) where T : unmanaged
            => NtReadVirtualMemoryArray(handle, baseAddress, buffer, offset, buffer.Length - offset, out numberOfBytesRead);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer) where T : unmanaged
            => NtReadVirtualMemoryArray(handle, baseAddress, buffer, 0, buffer.Length);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemoryArray<T>(
            IntPtr handle,
            IntPtr baseAddress,
            T[] buffer,
            out IntPtr numberOfBytesRead) where T : unmanaged
            => NtReadVirtualMemoryArray(handle, baseAddress, buffer, 0, buffer.Length, out numberOfBytesRead);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from the start of the buffer.</param>
        /// <param name="length">The number of bytes to copy to the buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemoryPartial<T>(
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

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntReadVirtualMemory)));
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
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from the start of the buffer.</param>
        /// <param name="length">The number of bytes to copy to the buffer.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            int length,
            out IntPtr numberOfBytesRead) where T : unmanaged
        {
            IL.DeclareLocals(
                new LocalVar("result", typeof(uint)),
                new LocalVar("localNumberOfBytes", typeof(IntPtr)),
                new LocalVar("pinnedBuffer", typeof(T).MakeByRefType()).Pinned(),
                new LocalVar("ptr", typeof(byte).MakePointerType()));

            numberOfBytesRead = default;

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

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(byte).MakePointerType(),
                typeof(IntPtr),
                typeof(IntPtr).MakePointerType()));

            Stloc("result");

            Ldarg(nameof(numberOfBytesRead));
            Ldloc("localNumberOfBytes");
            Stind_I();

            Ldloc("result");
            return IL.Return<uint>();
        }

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from the start of the buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset) where T : unmanaged
            => NtReadVirtualMemoryPartial(handle, baseAddress, ref buffer, offset, InternalHelper.SizeOf<T>() - offset);

        /// <summary>
        /// ReadProcessMemory copies the data in the specified address range from the address space of the specified process into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can call the function. The entire area to be read must be accessible, and if it is not accessible, the function fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">A byte offset from the start of the buffer.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtReadVirtualMemoryPartial<T>(
            IntPtr handle,
            IntPtr baseAddress,
            ref T buffer,
            int offset,
            out IntPtr numberOfBytesRead) where T : unmanaged
            => NtReadVirtualMemoryPartial(handle, baseAddress, ref buffer, offset, InternalHelper.SizeOf<T>() - offset, out numberOfBytesRead);
    }
}
