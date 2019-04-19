#pragma warning disable IDE0060 // Remove unused parameter

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ProcessMemoryUtilities.PInvoke;

using InlineIL;
using static InlineIL.IL.Emit;

namespace ProcessMemoryUtilities.Memory
{
    /// <summary>
    /// Provides methods to allocate, protect, free, read, write and execute memory in a remote process.
    /// </summary>
    public static class ProcessMemory
    {
        // https://gist.github.com/michel-pi/361f1f8bdca51235cb97aba0256d47e9
        private const int NT_STATUS_WARNING_START = 2147483647;

        private static readonly IntPtr _kernelCreateRemoteThreadEx;
        private static readonly IntPtr _kernelOpenProcess;
        private static readonly IntPtr _kernelWaitForSingleObject;

        private static readonly IntPtr _ntAllocateVirtualMemory;
        private static readonly IntPtr _ntClose;
        private static readonly IntPtr _ntFreeVirtualMemory;
        private static readonly IntPtr _ntOpenProcess;
        private static readonly IntPtr _ntProtectVirtualMemory;
        private static readonly IntPtr _ntReadVirtualMemory;
        private static readonly IntPtr _ntWriteVirtualMemory;

        /// <summary>
        /// A constant to use with OpenProcess when requiring access to allocate memory.
        /// </summary>
        public const ProcessAccessFlags PROCESS_ALLOCATE_ACCESS = ProcessAccessFlags.VirtualMemoryOperation;

        /// <summary>
        /// A constant to use with OpenProcess when requiring access to execute memory.
        /// </summary>
        public const ProcessAccessFlags PROCESS_EXECUTE_ACCESS = PROCESS_READ_WRITE_ACCESS | PROCESS_INFORMATION_ACCESS | ProcessAccessFlags.CreateThread;

        /// <summary>
        /// A constant to use with OpenProcess when requiring access to informations about the process and its memory.
        /// </summary>
        public const ProcessAccessFlags PROCESS_INFORMATION_ACCESS = ProcessAccessFlags.QueryInformation | ProcessAccessFlags.QueryLimitedInformation;

        /// <summary>
        /// A constant to be use with OpenProcess when requiring access to read memory.
        /// </summary>
        public const ProcessAccessFlags PROCESS_READ_ACCESS = ProcessAccessFlags.VirtualMemoryRead;

        /// <summary>
        /// A constant to be use with OpenProcess when requiring access to read and write memory.
        /// </summary>
        public const ProcessAccessFlags PROCESS_READ_WRITE_ACCESS = PROCESS_READ_ACCESS | PROCESS_WRITE_ACCESS;

        /// <summary>
        /// A constant to use with OpenProcess when requiring access to write memory.
        /// </summary>
        public const ProcessAccessFlags PROCESS_WRITE_ACCESS = ProcessAccessFlags.VirtualMemoryOperation | ProcessAccessFlags.VirtualMemoryWrite;

        /// <summary>
        /// A constant to use with WaitForSingleObject to wait for an infinite amount of time.
        /// </summary>
        public const uint WAIT_TIMEOUT_INFINITE = uint.MaxValue;

        static ProcessMemory()
        {
            var kernel = DynamicImport.ImportLibrary("kernel32.dll");

            _kernelCreateRemoteThreadEx = DynamicImport.ImportMethod(kernel, "CreateRemoteThreadEx");
            _kernelOpenProcess = DynamicImport.ImportMethod(kernel, "OpenProcess");
            _kernelWaitForSingleObject = DynamicImport.ImportMethod(kernel, "WaitForSingleObject");

            var ntdll = DynamicImport.ImportLibrary("ntdll.dll");

            _ntAllocateVirtualMemory = DynamicImport.ImportMethod(ntdll, "NtAllocateVirtualMemory");
            _ntClose = DynamicImport.ImportMethod(ntdll, "NtClose");
            _ntFreeVirtualMemory = DynamicImport.ImportMethod(ntdll, "NtFreeVirtualMemory");
            _ntOpenProcess = DynamicImport.ImportMethod(ntdll, "NtOpenProcess");
            _ntProtectVirtualMemory = DynamicImport.ImportMethod(ntdll, "NtProtectVirtualMemory");
            _ntReadVirtualMemory = DynamicImport.ImportMethod(ntdll, "NtReadVirtualMemory");
            _ntWriteVirtualMemory = DynamicImport.ImportMethod(ntdll, "NtWriteVirtualMemory");
        }

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="handle">A valid handle to an open object.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CloseHandle(IntPtr handle)
        {
            Ldarg(nameof(handle));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntClose)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process and optionally specifies extended attributes such as processor
        /// group affinity.
        /// </summary>
        /// <param name="handle">
        /// A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION,
        /// PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights.
        /// </param>
        /// <param name="startAddress">
        /// A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting
        /// address of the thread in the remote process. The function must exist in the remote process.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is NULL.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThreadEx(IntPtr handle, IntPtr startAddress)
        {
            Ldarg(nameof(handle));

            Ldc_I4_0();
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldarg(nameof(startAddress));

            Ldc_I4_0();
            Conv_I();

            Ldc_I4((int)ThreadCreationFlags.Immediately);
            Conv_U4();

            Ldc_I4_0();
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_kernelCreateRemoteThreadEx)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(IntPtr), typeof(IntPtr)));

            return IL.Return<IntPtr>();
        }

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process and optionally specifies extended attributes such as processor
        /// group affinity.
        /// </summary>
        /// <param name="handle">
        /// A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION,
        /// PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights.
        /// </param>
        /// <param name="startAddress">
        /// A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting
        /// address of the thread in the remote process. The function must exist in the remote process.
        /// </param>
        /// <param name="parameter">
        /// A pointer to a variable to be passed to the thread function pointed to by lpStartAddress. This parameter can be NULL.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is NULL.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThreadEx(IntPtr handle, IntPtr startAddress, IntPtr parameter)
        {
            Ldarg(nameof(handle));

            Ldc_I4_0();
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldarg(nameof(startAddress));

            Ldarg(nameof(parameter));

            Ldc_I4((int)ThreadCreationFlags.Immediately);
            Conv_U4();

            Ldc_I4_0();
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_kernelCreateRemoteThreadEx)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(IntPtr), typeof(IntPtr)));

            return IL.Return<IntPtr>();
        }

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process and optionally specifies extended attributes such as processor
        /// group affinity.
        /// </summary>
        /// <param name="handle">
        /// A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION,
        /// PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights.
        /// </param>
        /// <param name="threadAttributes">
        /// A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor for the new thread and determines whether child processes
        /// can inherit the returned handle. If lpThreadAttributes is NULL, the thread gets a default security descriptor and the handle cannot be
        /// inherited. The access control lists (ACL) in the default security descriptor for a thread come from the primary token of the creator.
        /// </param>
        /// <param name="stackSize">
        /// The initial size of the stack, in bytes. The system rounds this value to the nearest page. If this parameter is 0 (zero), the new thread
        /// uses the default size for the executable. For more information, see Thread Stack Size.
        /// </param>
        /// <param name="startAddress">
        /// A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting
        /// address of the thread in the remote process. The function must exist in the remote process.
        /// </param>
        /// <param name="parameter">
        /// A pointer to a variable to be passed to the thread function pointed to by lpStartAddress. This parameter can be NULL.
        /// </param>
        /// <param name="creationFlags">The flags that control the creation of the thread.</param>
        /// <param name="attributeList">
        /// An attribute list that contains additional parameters for the new thread. This list is created by the InitializeProcThreadAttributeList
        /// function.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is NULL.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThreadEx(IntPtr handle, IntPtr threadAttributes, IntPtr stackSize, IntPtr startAddress, IntPtr parameter, ThreadCreationFlags creationFlags, IntPtr attributeList)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(threadAttributes));
            Ldarg(nameof(stackSize));
            Ldarg(nameof(startAddress));
            Ldarg(nameof(parameter));
            Ldarg(nameof(creationFlags));
            Conv_U4();
            Ldarg(nameof(attributeList));

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_kernelCreateRemoteThreadEx)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(IntPtr), typeof(IntPtr)));

            return IL.Return<IntPtr>();
        }

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process and optionally specifies extended attributes such as processor
        /// group affinity.
        /// </summary>
        /// <param name="handle">
        /// A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION,
        /// PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights.
        /// </param>
        /// <param name="threadAttributes">
        /// A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor for the new thread and determines whether child processes
        /// can inherit the returned handle. If lpThreadAttributes is NULL, the thread gets a default security descriptor and the handle cannot be
        /// inherited. The access control lists (ACL) in the default security descriptor for a thread come from the primary token of the creator.
        /// </param>
        /// <param name="stackSize">
        /// The initial size of the stack, in bytes. The system rounds this value to the nearest page. If this parameter is 0 (zero), the new thread
        /// uses the default size for the executable. For more information, see Thread Stack Size.
        /// </param>
        /// <param name="startAddress">
        /// A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting
        /// address of the thread in the remote process. The function must exist in the remote process.
        /// </param>
        /// <param name="parameter">
        /// A pointer to a variable to be passed to the thread function pointed to by lpStartAddress. This parameter can be NULL.
        /// </param>
        /// <param name="creationFlags">The flags that control the creation of the thread.</param>
        /// <param name="attributeList">
        /// An attribute list that contains additional parameters for the new thread. This list is created by the InitializeProcThreadAttributeList
        /// function.
        /// </param>
        /// <param name="threadId">A variable that receives the thread identifier.</param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is NULL.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThreadEx(IntPtr handle, IntPtr threadAttributes, IntPtr stackSize, IntPtr startAddress, IntPtr parameter, ThreadCreationFlags creationFlags, IntPtr attributeList, ref uint threadId)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(threadAttributes));
            Ldarg(nameof(stackSize));
            Ldarg(nameof(startAddress));
            Ldarg(nameof(parameter));
            Ldarg(nameof(creationFlags));
            Conv_U4();
            Ldarg(nameof(attributeList));
            Ldarg(nameof(threadId));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_kernelCreateRemoteThreadEx)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(IntPtr), typeof(IntPtr)));

            return IL.Return<IntPtr>();
        }

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="desiredAccess">
        /// The access to the process object. This access right is checked against the security descriptor for the process. This parameter can be one
        /// or more of the process access rights.
        /// </param>
        /// <param name="processId">The identifier of the local process to be opened.</param>
        /// <returns>
        /// If the function succeeds, the return value is an open handle to the specified process. If the function fails, the return value is NULL.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr OpenProcess(ProcessAccessFlags desiredAccess, int processId)
        {
            IL.DeclareLocals(
                new LocalVar("handle", typeof(IntPtr)),
                new LocalVar("objectAttributes", typeof(ObjectAttributes)),
                new LocalVar("clientID", typeof(ClientID)));

            Ldc_I4_0();
            Conv_I();
            Stloc("handle");

            Ldloca("objectAttributes");
            Initobj(typeof(ObjectAttributes));

            Ldloca("objectAttributes");
            Sizeof(typeof(ObjectAttributes));
            Stfld(new FieldRef(typeof(ObjectAttributes), "Length"));

            Ldloca("clientID");
            Initobj(typeof(ClientID));

            Ldloca("clientID");
            Ldarg(nameof(processId));
            Conv_I();
            Stfld(new FieldRef(typeof(ClientID), "UniqueProcess"));

            Ldloca("handle");
            Ldarg(nameof(desiredAccess));
            Ldloca("objectAttributes");
            Ldloca("clientID");

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntOpenProcess)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(uint), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Blt_Un_S("success");

            Ldc_I4_0();
            Conv_I();
            Ret();

            IL.MarkLabel("success");

            Ldloc("handle");
            return IL.Return<IntPtr>();
        }

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="desiredAccess">
        /// The access to the process object. This access right is checked against the security descriptor for the process. This parameter can be one
        /// or more of the process access rights.
        /// </param>
        /// <param name="inheritHandle">
        /// If this value is TRUE, processes created by this process will inherit the handle. Otherwise, the processes do not inherit this handle.
        /// </param>
        /// <param name="processId">The identifier of the local process to be opened.</param>
        /// <returns>
        /// If the function succeeds, the return value is an open handle to the specified process. If the function fails, the return value is NULL.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr OpenProcess(ProcessAccessFlags desiredAccess, bool inheritHandle, int processId)
        {
            Ldarg(nameof(desiredAccess));
            Ldarg(nameof(inheritHandle));
            Conv_I4();
            Ldarg(nameof(processId));
            Conv_U4();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_kernelOpenProcess)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(IntPtr), typeof(uint), typeof(int), typeof(uint)));

            return IL.Return<IntPtr>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <param name="handle">
        /// A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.
        /// </param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, IntPtr size)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));
            Ldarg(nameof(size));

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <param name="handle">
        /// A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.
        /// </param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, IntPtr size, ref IntPtr numberOfBytesRead)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));
            Ldarg(nameof(size));
            Ldarg(nameof(numberOfBytesRead));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        /// <param name="handle">
        /// A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.
        /// </param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, ref T buffer) where T : struct
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));

            Sizeof(typeof(T));
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        /// <param name="handle">
        /// A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.
        /// </param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, ref T buffer, ref IntPtr numberOfBytesRead) where T : struct
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));

            Sizeof(typeof(T));
            Conv_I();

            Ldarg(nameof(numberOfBytesRead));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        /// <param name="handle">
        /// A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.
        /// </param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer) where T : struct
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarg(nameof(buffer));
            Ldc_I4_0();
            Ldelema(typeof(T));
            Conv_I();

            Ldarg(nameof(buffer));
            Ldlen();

            Sizeof(typeof(T));

            Mul();
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        /// <param name="handle">
        /// A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.
        /// </param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer, ref IntPtr numberOfBytesRead) where T : struct
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarg(nameof(buffer));
            Ldc_I4_0();
            Ldelema(typeof(T));
            Conv_I();

            Ldarg(nameof(buffer));
            Ldlen();

            Sizeof(typeof(T));

            Mul();
            Conv_I();

            Ldarg(nameof(numberOfBytesRead));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        /// <param name="handle">
        /// A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.
        /// </param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">The offset inside the buffer to read the data to.</param>
        /// <param name="length">The length of the buffer to fill after the offset.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer, int offset, int length) where T : struct
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarg(nameof(buffer));
            Ldarg(nameof(offset));
            Ldelema(typeof(T));
            Conv_I();

            Ldarg(nameof(length));
            Sizeof(typeof(T));

            Mul();
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to return.</typeparam>
        /// <param name="handle">
        /// A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for read access, and if it is not accessible the function fails.
        /// </param>
        /// <param name="buffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
        /// <param name="offset">The offset inside the buffer to read the data to.</param>
        /// <param name="length">The length of the buffer to fill after the offset.</param>
        /// <param name="numberOfBytesRead">A pointer to a variable that receives the number of bytes transferred into the specified buffer.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer, int offset, int length, ref IntPtr numberOfBytesRead) where T : struct
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarg(nameof(buffer));
            Ldarg(nameof(offset));
            Ldelema(typeof(T));
            Conv_I();

            Ldarg(nameof(length));
            Sizeof(typeof(T));

            Mul();
            Conv_I();

            Ldarg(nameof(numberOfBytesRead));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntReadVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Reserves, commits, or changes the state of a region of memory within the virtual address space of a specified process. The function
        /// initializes the memory it allocates to zero.
        /// </summary>
        /// <param name="handle">
        /// The handle to a process. The function allocates memory within the virtual address space of this process. The handle must have the
        /// PROCESS_VM_OPERATION access right.For more information, see Process Security and Access Rights.
        /// </param>
        /// <param name="baseAddress">The pointer that specifies a desired starting address for the region of pages that you want to allocate.</param>
        /// <param name="size">The size of the region of memory to allocate, in bytes.</param>
        /// <param name="allocationType">The type of memory allocation.</param>
        /// <param name="memoryProtection">
        /// The memory protection for the region of pages to be allocated. If the pages are being committed, you can specify any one of the memory
        /// protection constants.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the base address of the allocated region of pages. If the function fails, the return value is
        /// NULL.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr VirtualAllocEx(IntPtr handle, IntPtr baseAddress, IntPtr size, AllocationType allocationType, MemoryProtectionFlags memoryProtection)
        {
            Ldarg(nameof(handle));

            Ldarga(nameof(baseAddress));
            Conv_I();

            Ldc_I4_0();
            Conv_U4();

            Ldarga(nameof(size));
            Conv_I();

            Ldarg(nameof(allocationType));
            Conv_U4();
            Ldarg(nameof(memoryProtection));
            Conv_U4();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntAllocateVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(IntPtr), typeof(uint), typeof(uint)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();

            Blt_Un_S("success");

            Ldc_I4_0();
            Conv_I();
            Ret();

            IL.MarkLabel("success");

            Ldarg(nameof(baseAddress));
            return IL.Return<IntPtr>();
        }

        /// <summary>
        /// Releases, decommits, or releases and decommits a region of memory within the virtual address space of a specified process.
        /// </summary>
        /// <param name="handle">
        /// A handle to a process. The function frees memory within the virtual address space of the process. The handle must have the
        /// PROCESS_VM_OPERATION access right.For more information, see Process Security and Access Rights.
        /// </param>
        /// <param name="address">A pointer to the starting address of the region of memory to be freed.</param>
        /// <param name="size">
        /// The size of the region of memory to free, in bytes. If the dwFreeType parameter is MEM_RELEASE, dwSize must be 0 (zero).
        /// </param>
        /// <param name="freeType">The type of free operation.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VirtualFreeEx(IntPtr handle, IntPtr address, IntPtr size, FreeType freeType)
        {
            IL.DeclareLocals(new LocalVar("regionSize", typeof(uint)));

            Ldarg(nameof(size));
            Conv_U4();
            Stloc("regionSize");

            Ldarg(nameof(handle));

            Ldarga(nameof(address));
            Conv_I();

            Ldloca("regionSize");
            Conv_I();

            Ldarg(nameof(freeType));
            Conv_U4();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntFreeVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(uint)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();

            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Changes the protection on a region of committed pages in the virtual address space of a specified process.
        /// </summary>
        /// <param name="handle">
        /// A handle to the process whose memory protection is to be changed. The handle must have the PROCESS_VM_OPERATION access right. For more
        /// information, see Process Security and Access Rights.
        /// </param>
        /// <param name="address">A pointer to the base address of the region of pages whose access protection attributes are to be changed.</param>
        /// <param name="size">The size of the region whose access protection attributes are changed, in bytes.</param>
        /// <param name="newProtect">The memory protection option. This parameter can be one of the memory protection constants.</param>
        /// <param name="oldProtect">
        /// A pointer to a variable that receives the previous access protection of the first page in the specified region of pages.
        /// </param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VirtualProtectEx(IntPtr handle, IntPtr address, IntPtr size, MemoryProtectionFlags newProtect, ref MemoryProtectionFlags oldProtect)
        {
            IL.DeclareLocals(new LocalVar("regionSize", typeof(uint)));

            Ldarg(nameof(size));
            Conv_U4();
            Stloc("regionSize");

            Ldarg(nameof(handle));

            Ldarga(nameof(address));
            Conv_I();

            Ldloca("regionSize");
            Conv_I();

            Ldarg(nameof(newProtect));
            Conv_U4();

            Ldarg(nameof(oldProtect));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntProtectVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();

            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Waits until the specified object is in the signaled state or the time-out interval elapses.
        /// </summary>
        /// <param name="handle">
        /// A handle to the object. For a list of the object types whose handles can be specified, see the following Remarks section. If this handle is
        /// closed while the wait is still pending, the function's behavior is undefined. The handle must have the SYNCHRONIZE access right. For more
        /// information, see Standard Access Rights.
        /// </param>
        /// <param name="timeout">
        /// The time-out interval, in milliseconds. If a nonzero value is specified, the function waits until the object is signaled or the interval
        /// elapses. If dwMilliseconds is zero, the function does not enter a wait state if the object is not signaled; it always returns immediately.
        /// If dwMilliseconds is INFINITE, the function will return only when the object is signaled.
        /// </param>
        /// <returns>A WaitObjectResult representing the event caused WaitForSingleObject to return.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WaitObjectResult WaitForSingleObject(IntPtr handle, uint timeout)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(timeout));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_kernelWaitForSingleObject)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(WaitObjectResult), typeof(IntPtr), typeof(uint)));

            return IL.Return<WaitObjectResult>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <param name="handle">
        /// A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.
        /// </param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be written to the specified process.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, IntPtr size)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));
            Ldarg(nameof(size));

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <param name="handle">
        /// A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.
        /// </param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="size">The number of bytes to be written to the specified process.</param>
        /// <param name="numberOfBytesWritten">
        /// A pointer to a variable that receives the number of bytes transferred into the specified process.
        /// </param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, IntPtr size, ref IntPtr numberOfBytesWritten)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));
            Ldarg(nameof(buffer));
            Ldarg(nameof(size));
            Ldarg(nameof(numberOfBytesWritten));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to write.</typeparam>
        /// <param name="handle">
        /// A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.
        /// </param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T buffer) where T : struct
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarga(nameof(buffer));
            Conv_I();

            Sizeof(typeof(T));
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to write.</typeparam>
        /// <param name="handle">
        /// A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.
        /// </param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="numberOfBytesWritten">
        /// A pointer to a variable that receives the number of bytes transferred into the specified process.
        /// </param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T buffer, ref IntPtr numberOfBytesWritten) where T : struct
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarga(nameof(buffer));
            Conv_I();

            Sizeof(typeof(T));
            Conv_I();

            Ldarg(nameof(numberOfBytesWritten));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to write.</typeparam>
        /// <param name="handle">
        /// A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.
        /// </param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <returns>A pointer to a variable that receives the number of bytes transferred into the specified process.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer) where T : struct
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarg(nameof(buffer));
            Ldc_I4_0();
            Ldelema(typeof(T));
            Conv_I();

            Ldarg(nameof(buffer));
            Ldlen();

            Sizeof(typeof(T));

            Mul();
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to write.</typeparam>
        /// <param name="handle">
        /// A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.
        /// </param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="numberOfBytesWritten">
        /// A pointer to a variable that receives the number of bytes transferred into the specified process.
        /// </param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer, ref IntPtr numberOfBytesWritten) where T : struct
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarg(nameof(buffer));
            Ldc_I4_0();
            Ldelema(typeof(T));
            Conv_I();

            Ldarg(nameof(buffer));
            Ldlen();

            Sizeof(typeof(T));

            Mul();
            Conv_I();

            Ldarg(nameof(numberOfBytesWritten));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to write.</typeparam>
        /// <param name="handle">
        /// A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.
        /// </param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">The start offset in the buffer array to write data from.</param>
        /// <param name="length">The length of the the buffer to write starting from the offset.</param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer, int offset, int length) where T : struct
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarg(nameof(buffer));
            Ldarg(nameof(offset));
            Ldelema(typeof(T));
            Conv_I();

            Ldarg(nameof(length));
            Sizeof(typeof(T));

            Mul();
            Conv_I();

            Ldc_I4_0();
            Conv_I();

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        /// <summary>
        /// Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the operation fails.
        /// </summary>
        /// <typeparam name="T">The type of the data to write.</typeparam>
        /// <param name="handle">
        /// A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION access to the process.
        /// </param>
        /// <param name="baseAddress">
        /// A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the system verifies that all
        /// data in the base address and memory of the specified size is accessible for write access, and if it is not accessible, the function fails.
        /// </param>
        /// <param name="buffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
        /// <param name="offset">The start offset in the buffer array to write data from.</param>
        /// <param name="length">The length of the the buffer to write starting from the offset.</param>
        /// <param name="numberOfBytesWritten">
        /// A pointer to a variable that receives the number of bytes transferred into the specified process.
        /// </param>
        /// <returns><see langword="true"/> if the function succeeds; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer, int offset, int length, ref IntPtr numberOfBytesWritten) where T : struct
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(baseAddress));

            Ldarg(nameof(buffer));
            Ldarg(nameof(offset));
            Ldelema(typeof(T));
            Conv_I();

            Ldarg(nameof(length));
            Sizeof(typeof(T));

            Mul();
            Conv_I();

            Ldarg(nameof(numberOfBytesWritten));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntWriteVirtualMemory)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
