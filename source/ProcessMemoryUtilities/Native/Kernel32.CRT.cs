using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using InlineIL;
using static InlineIL.IL.Emit;

namespace ProcessMemoryUtilities.Native
{
    public static partial class Kernel32
    {
        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process and optionally specifies extended attributes such as processor group affinity.
        /// </summary>
        /// <param name="handle">A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION, PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights.</param>
        /// <param name="startAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process.</param>
        /// <param name="parameter">A pointer to a variable to be passed to the thread function pointed to by lpStartAddress. This parameter can be IntPtr.Zero.</param>
        /// <returns>If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is IntPtr.Zero. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThreadEx(IntPtr handle, IntPtr startAddress, IntPtr parameter)
            => CreateRemoteThreadEx(
                handle,
                IntPtr.Zero, IntPtr.Zero,
                startAddress,
                parameter,
                ThreadCreationFlags.Immediately,
                IntPtr.Zero, IntPtr.Zero);

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process and optionally specifies extended attributes such as processor group affinity.
        /// </summary>
        /// <param name="handle">A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION, PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights.</param>
        /// <param name="startAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process.</param>
        /// <param name="parameter">A pointer to a variable to be passed to the thread function pointed to by lpStartAddress. This parameter can be IntPtr.Zero.</param>
        /// <param name="creationFlags">The flags that control the creation of the thread.</param>
        /// <returns>If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is IntPtr.Zero. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThreadEx(
            IntPtr handle,
            IntPtr startAddress,
            IntPtr parameter,
            ThreadCreationFlags creationFlags)
            => CreateRemoteThreadEx(
                handle,
                IntPtr.Zero, IntPtr.Zero,
                startAddress,
                parameter,
                creationFlags,
                IntPtr.Zero, IntPtr.Zero);

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process and optionally specifies extended attributes such as processor group affinity.
        /// </summary>
        /// <param name="handle">A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION, PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights.</param>
        /// <param name="startAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process.</param>
        /// <param name="parameter">A pointer to a variable to be passed to the thread function pointed to by lpStartAddress. This parameter can be IntPtr.Zero.</param>
        /// <param name="creationFlags">The flags that control the creation of the thread.</param>
        /// <param name="threadId">A variable that receives the thread identifier.</param>
        /// <returns>If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is IntPtr.Zero. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThreadEx(
            IntPtr handle,
            IntPtr startAddress,
            IntPtr parameter,
            ThreadCreationFlags creationFlags,
            out uint threadId)
            => CreateRemoteThreadEx(
                handle,
                IntPtr.Zero, IntPtr.Zero,
                startAddress,
                parameter,
                creationFlags,
                IntPtr.Zero, out threadId);

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process and optionally specifies extended attributes such as processor group affinity.
        /// </summary>
        /// <param name="handle">A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION, PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights.</param>
        /// <param name="threadAttributes">A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor for the new thread and determines whether child processes can inherit the returned handle. If lpThreadAttributes is NULL, the thread gets a default security descriptor and the handle cannot be inherited. The access control lists (ACL) in the default security descriptor for a thread come from the primary token of the creator.</param>
        /// <param name="stackSize">The initial size of the stack, in bytes. The system rounds this value to the nearest page. If this parameter is 0 (zero), the new thread uses the default size for the executable.</param>
        /// <param name="startAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process.</param>
        /// <param name="parameter">A pointer to a variable to be passed to the thread function pointed to by lpStartAddress. This parameter can be IntPtr.Zero.</param>
        /// <param name="creationFlags">The flags that control the creation of the thread.</param>
        /// <param name="attributeList">An attribute list that contains additional parameters for the new thread. This list is created by the InitializeProcThreadAttributeList function.</param>
        /// <param name="threadId">A variable that receives the thread identifier.</param>
        /// <returns>If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is IntPtr.Zero. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThreadEx(
            IntPtr handle,
            IntPtr threadAttributes,
            IntPtr stackSize,
            IntPtr startAddress,
            IntPtr parameter,
            ThreadCreationFlags creationFlags,
            IntPtr attributeList,
            out uint threadId)
        {
            IL.DeclareLocals(
                new LocalVar("localThreadId", typeof(uint)),
                new LocalVar("result", typeof(IntPtr)));

            threadId = 0u;

            Ldloca("localThreadId");
            Initobj(typeof(uint));

            Ldarg(nameof(handle));
            Ldarg(nameof(threadAttributes));
            Ldarg(nameof(stackSize));
            Ldarg(nameof(startAddress));
            Ldarg(nameof(parameter));
            Ldarg(nameof(creationFlags));
            Ldarg(nameof(attributeList));
            Ldloca("localThreadId");
            Conv_U();

            Ldsfld(new FieldRef(typeof(Kernel32), nameof(_createRemoteThreadEx)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(ThreadCreationFlags),
                typeof(IntPtr),
                typeof(uint).MakePointerType()));

            Stloc("result");

            Ldarg(nameof(threadId));
            Ldloc("localThreadId");
            Stind_I4();

            Ldloc("result");
            return IL.Return<IntPtr>();
        }

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process and optionally specifies extended attributes such as processor group affinity.
        /// </summary>
        /// <param name="handle">A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION, PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights.</param>
        /// <param name="threadAttributes">A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor for the new thread and determines whether child processes can inherit the returned handle. If lpThreadAttributes is NULL, the thread gets a default security descriptor and the handle cannot be inherited. The access control lists (ACL) in the default security descriptor for a thread come from the primary token of the creator.</param>
        /// <param name="stackSize">The initial size of the stack, in bytes. The system rounds this value to the nearest page. If this parameter is 0 (zero), the new thread uses the default size for the executable.</param>
        /// <param name="startAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process.</param>
        /// <param name="parameter">A pointer to a variable to be passed to the thread function pointed to by lpStartAddress. This parameter can be IntPtr.Zero.</param>
        /// <param name="creationFlags">The flags that control the creation of the thread.</param>
        /// <param name="attributeList">An attribute list that contains additional parameters for the new thread. This list is created by the InitializeProcThreadAttributeList function.</param>
        /// <param name="threadId">A variable that receives the thread identifier.</param>
        /// <returns>If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is IntPtr.Zero. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThreadEx(
            IntPtr handle,
            IntPtr threadAttributes,
            IntPtr stackSize,
            IntPtr startAddress,
            IntPtr parameter,
            ThreadCreationFlags creationFlags,
            IntPtr attributeList,
            IntPtr threadId)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(threadAttributes));
            Ldarg(nameof(stackSize));
            Ldarg(nameof(startAddress));
            Ldarg(nameof(parameter));
            Ldarg(nameof(creationFlags));
            Ldarg(nameof(attributeList));
            Ldarg(nameof(threadId));

            Ldsfld(new FieldRef(typeof(Kernel32), nameof(_createRemoteThreadEx)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(IntPtr),
                typeof(ThreadCreationFlags),
                typeof(IntPtr),
                typeof(IntPtr)));

            return IL.Return<IntPtr>();
        }
    }
}
