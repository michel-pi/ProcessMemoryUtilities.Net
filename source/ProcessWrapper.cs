#pragma warning disable IDE0060 // Remove unused parameter

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using InlineIL;
using static InlineIL.IL.Emit;

namespace ProcessMemoryWrapper
{
    /// <summary>
    /// Implements performant Read- and WriteProcessMemory using InlineIL.
    /// </summary>
    public static unsafe class ProcessWrapper
    {
        /// <summary>
        /// Provides a constant for OpenProcess with read access.
        /// </summary>
        public const ProcessAccessFlags ProcessReadAccess = ProcessAccessFlags.VirtualMemoryRead;
        /// <summary>
        /// Provides a constant for OpenProcess with write access.
        /// </summary>
        public const ProcessAccessFlags ProcessWriteAccess = ProcessAccessFlags.VirtualMemoryOperation | ProcessAccessFlags.VirtualMemoryWrite;
        /// <summary>
        /// Provides a constant for OpenProcess with read and write access.
        /// </summary>
        public const ProcessAccessFlags ProcessReadWriteAccess = ProcessReadAccess | ProcessWriteAccess;
        /// <summary>
        /// Provides a constant for OpenProcess with information access.
        /// </summary>
        public const ProcessAccessFlags ProcessInformationAccess = ProcessAccessFlags.QueryInformation | ProcessAccessFlags.QueryLimitedInformation;
        /// <summary>
        /// Provides a constant for OpenProcess with memory operation access.
        /// </summary>
        public const ProcessAccessFlags ProcessAllocateAccess = ProcessAccessFlags.VirtualMemoryOperation;
        /// <summary>
        /// Provides a constant for OpenProcess with execute access.
        /// </summary>
        public const ProcessAccessFlags ProcessExecuteAccess = ProcessReadWriteAccess | ProcessInformationAccess | ProcessAccessFlags.CreateThread;

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procname);

        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandleW", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr GetModuleHandle(string modulename);

        private static readonly void* _ntOpenProcess;
        private static readonly void* _ntClose;

        private static readonly void* _ntReadVirtualMemory;
        private static readonly void* _ntWriteVirtualMemory;

        static ProcessWrapper()
        {
            var ntdll = GetModuleHandle("ntdll.dll");

            _ntOpenProcess = GetProcAddress(ntdll, "NtOpenProcess").ToPointer();
            _ntClose = GetProcAddress(ntdll, "NtClose").ToPointer();

            _ntReadVirtualMemory = GetProcAddress(ntdll, "NtReadVirtualMemory").ToPointer();
            _ntWriteVirtualMemory = GetProcAddress(ntdll, "NtWriteVirtualMemory").ToPointer();
        }

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="handle">A valid handle to an open object.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CloseProcess(IntPtr handle)
        {
            Ldarg(nameof(handle));

            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntClose)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr)));

            Ldc_I4_0();
            Ceq();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="desiredAccess">The access to the process object. This access right is checked against the security descriptor for the process. This parameter can be one or more of the process access rights.</param>
        /// <param name="processId">The identifier of the local process to be opened.</param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified process.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr OpenProcess(ProcessAccessFlags desiredAccess, int processId)
        {
            IL.DeclareLocals(true,
                new LocalVar("handle", typeof(IntPtr)),
                new LocalVar("objectAttributes", typeof(ObjectAttributes)),
                new LocalVar("clientID", typeof(ClientID)));
            
            Ldloca("handle");
            Initobj(typeof(IntPtr));
            Ldloca("objectAttributes");
            Initobj(typeof(ObjectAttributes));
            Ldloca("clientID");
            Initobj(typeof(ClientID));
            
            Ldloca("clientID");
            Ldarg(nameof(processId));
            Conv_U();
            Stfld(new FieldRef(typeof(ClientID), "UniqueProcess"));
            
            Ldloca("objectAttributes");
            Sizeof(typeof(ObjectAttributes));
            Stfld(new FieldRef(typeof(ObjectAttributes), "Length"));
            
            Ldloca("handle");
            Ldarg(nameof(desiredAccess));
            Ldloca("objectAttributes");
            Ldloca("clientID");
            
            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntOpenProcess)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(uint), typeof(IntPtr), typeof(IntPtr)));
            
            Pop();
            
            Ldloc("handle");
            return IL.Return<IntPtr>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <returns>Returns a Boolean determining whether this method has succeeded.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, IntPtr size)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));
            Ldarg(nameof(size));
            Ldc_I4_0();
            
            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4_0();
            Ceq();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <param name="numberOfBytesRead">A IntPtr which receives the number of bytes read.</param>
        /// <returns>Returns a Boolean determining whether this method has succeeded.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, IntPtr size, ref IntPtr numberOfBytesRead)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));
            Ldarg(nameof(size));
            Ldarg(nameof(numberOfBytesRead));
            
            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));
            
            Ldc_I4_0();
            Ceq();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <returns>An array of data.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ReadProcessMemory(IntPtr handle, IntPtr baseAddress, int size)
        {
            IL.DeclareLocals(true, new LocalVar("buffer", typeof(byte[])));
            
            Ldarg(nameof(size));
            Newarr(typeof(byte));
            Stloc("buffer");
            
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            
            Ldloc("buffer");
            Ldc_I4_0();
            Ldelema(typeof(byte));

            Ldarg(nameof(size));
            Conv_I();

            Ldc_I4_0();
            
            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));
            
            Pop();

            Ldloc("buffer");
            return IL.Return<byte[]>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <param name="numberOfBytesRead">A IntPtr which receives the number of bytes read.</param>
        /// <returns>An array of data.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ReadProcessMemory(IntPtr handle, IntPtr baseAddress, int size, ref IntPtr numberOfBytesRead)
        {
            IL.DeclareLocals(true, new LocalVar("buffer", typeof(byte[])));
            
            Ldarg(nameof(size));
            Newarr(typeof(byte));
            Stloc("buffer");
            
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            
            Ldloc("buffer");
            Ldc_I4_0();
            Ldelema(typeof(byte));

            Ldarg(nameof(size));
            Conv_I();

            Ldarg(nameof(numberOfBytesRead));
            
            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));
            
            Pop();

            Ldloc("buffer");
            return IL.Return<byte[]>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <returns>The data read from the process.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress)
        {
            IL.DeclareLocals(true,
                new LocalVar("buffer", typeof(T)));

            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldloca("buffer");

            Sizeof(typeof(T));
            Conv_I();

            Ldc_I4_0();

            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Pop();

            Ldloc("buffer");
            return IL.Return<T>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="numberOfBytesRead">A IntPtr which receives the number of bytes read.</param>
        /// <returns>The data read from the process.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, ref IntPtr numberOfBytesRead)
        {
            IL.DeclareLocals(true,
                new LocalVar("buffer", typeof(T)));

            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldloca("buffer");

            Sizeof(typeof(T));
            Conv_I();

            Ldarg(nameof(numberOfBytesRead));

            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Pop();

            Ldloc("buffer");

            return IL.Return<T>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <returns>An array of data.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, int size)
        {
            IL.DeclareLocals(true, new LocalVar("buffer", typeof(T[])));
            
            Ldarg(nameof(size));
            Newarr(typeof(T));
            Stloc("buffer");
            
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            
            Ldloc("buffer");
            Ldc_I4_0();
            Ldelema(typeof(T));

            Ldarg(nameof(size));
            Sizeof(typeof(T));
            Mul();
            Conv_I();

            Ldc_I4_0();
            
            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));
            
            Pop();

            Ldloc("buffer");
            return IL.Return<T[]>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        /// <param name="handle">A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <param name="numberOfBytesRead">A IntPtr which receives the number of bytes read.</param>
        /// <returns>An array of data.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, int size, ref IntPtr numberOfBytesRead)
        {
            IL.DeclareLocals(true, new LocalVar("buffer", typeof(T[])));
            
            Ldarg(nameof(size));
            Newarr(typeof(T));
            Stloc("buffer");
            
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            
            Ldloc("buffer");
            Ldc_I4_0();
            Ldelema(typeof(T));

            Ldarg(nameof(size));
            Sizeof(typeof(T));
            Mul();
            Conv_I();

            Ldarg(nameof(numberOfBytesRead));
            
            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));
            
            Pop();

            Ldloc("buffer");
            return IL.Return<T[]>();
        }
        
        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be written to the specified process.</param>
        /// <returns>A Boolean determining whether this method has succeeded.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, IntPtr size)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));
            Ldarg(nameof(size));
            Ldc_I4_0();

            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4_0();
            Ceq();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be written to the specified process.</param>
        /// <param name="numberOfBytesWritten">A IntPtr receiving the number of bytes written to the specified process.</param>
        /// <returns>A Boolean determining whether this method has succeeded.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, IntPtr size, ref IntPtr numberOfBytesWritten)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));
            Ldarg(nameof(size));
            Ldarg(nameof(numberOfBytesWritten));

            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4_0();
            Ceq();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <returns>A Boolean determining whether this method has succeeded.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory(IntPtr handle, IntPtr baseAddress, byte[] buffer)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarg(nameof(buffer));
            Ldc_I4_0();
            Ldelema(typeof(byte));

            Ldarg(nameof(buffer));
            Ldlen();
            Conv_I();

            Ldc_I4_0();

            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4_0();
            Ceq();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="numberOfBytesWritten">A IntPtr receiving the number of bytes written to the specified process.</param>
        /// <returns>A Boolean determining whether this method has succeeded.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory(IntPtr handle, IntPtr baseAddress, byte[] buffer, ref IntPtr numberOfBytesWritten)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarg(nameof(buffer));
            Ldc_I4_0();
            Ldelema(typeof(byte));

            Ldarg(nameof(buffer));
            Ldlen();
            Conv_I();

            Ldarg(nameof(numberOfBytesWritten));

            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4_0();
            Ceq();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to write.</typeparam>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <returns>A Boolean determining whether this method has succeeded.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T buffer)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarga(nameof(buffer));

            Sizeof(typeof(T));
            Conv_I();

            Ldc_I4_0();

            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4_0();
            Ceq();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to write.</typeparam>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="numberOfBytesWritten">A IntPtr receiving the number of bytes written to the specified process.</param>
        /// <returns>A Boolean determining whether this method has succeeded.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T buffer, ref IntPtr numberOfBytesWritten)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarga(nameof(buffer));

            Sizeof(typeof(T));
            Conv_I();

            Ldarg(nameof(numberOfBytesWritten));

            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4_0();
            Ceq();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to write.</typeparam>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <returns>A Boolean determining whether this method has succeeded.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarg(nameof(buffer));
            Ldc_I4_0();
            Ldelema(typeof(T));

            Ldarg(nameof(buffer));
            Ldlen();
            Sizeof(typeof(T));
            Mul();
            Conv_I();

            Ldc_I4_0();

            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4_0();
            Ceq();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to write.</typeparam>
        /// <param name="handle">A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="baseAddress">A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.</param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="numberOfBytesWritten">A IntPtr receiving the number of bytes written to the specified process.</param>
        /// <returns>A Boolean determining whether this method has succeeded.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer, ref IntPtr numberOfBytesWritten)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarg(nameof(buffer));
            Ldc_I4_0();
            Ldelema(typeof(T));

            Ldarg(nameof(buffer));
            Ldlen();
            Sizeof(typeof(T));
            Mul();
            Conv_I();

            Ldarg(nameof(numberOfBytesWritten));

            Ldsfld(new FieldRef(typeof(ProcessWrapper), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4_0();
            Ceq();

            return IL.Return<bool>();
        }
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
