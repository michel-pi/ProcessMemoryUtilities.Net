using System;
using System.Runtime.CompilerServices;

using ProcessMemoryUtilities.Native;

namespace ProcessMemoryUtilities.Managed
{
    public static partial class NativeWrapper
    {
        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process.
        /// </summary>
        /// <param name="handle">A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION, PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights, and may fail without these rights on certain platforms. For more information, see Process Security and Access Rights.</param>
        /// <param name="threadAttributes">A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor for the new thread and determines whether child processes can inherit the returned handle. If lpThreadAttributes is NULL, the thread gets a default security descriptor and the handle cannot be inherited. The access control lists (ACL) in the default security descriptor for a thread come from the primary token of the creator.</param>
        /// <param name="stackSize">The initial size of the stack, in bytes. The system rounds this value to the nearest page. If this parameter is 0 (zero), the new thread uses the default size for the executable.</param>
        /// <param name="startAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process.</param>
        /// <param name="parameter">A pointer to a variable to be passed to the thread function.</param>
        /// <param name="creationFlags"></param>
        /// <param name="threadId">A pointer to a variable that receives the thread identifier. If this parameter is NULL, the thread identifier is not returned.</param>
        /// <returns>If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is IntPtr.Zero.To get extended error information, call GetLastError.</returns>
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

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process.
        /// </summary>
        /// <param name="handle">A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION, PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights, and may fail without these rights on certain platforms. For more information, see Process Security and Access Rights.</param>
        /// <param name="threadAttributes">A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor for the new thread and determines whether child processes can inherit the returned handle. If lpThreadAttributes is NULL, the thread gets a default security descriptor and the handle cannot be inherited. The access control lists (ACL) in the default security descriptor for a thread come from the primary token of the creator.</param>
        /// <param name="stackSize">The initial size of the stack, in bytes. The system rounds this value to the nearest page. If this parameter is 0 (zero), the new thread uses the default size for the executable.</param>
        /// <param name="startAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process.</param>
        /// <param name="parameter">A pointer to a variable to be passed to the thread function.</param>
        /// <param name="creationFlags"></param>
        /// <param name="threadId">A pointer to a variable that receives the thread identifier. If this parameter is NULL, the thread identifier is not returned.</param>
        /// <returns>If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is IntPtr.Zero.To get extended error information, call GetLastError.</returns>
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

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process.
        /// </summary>
        /// <param name="handle">A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION, PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights, and may fail without these rights on certain platforms. For more information, see Process Security and Access Rights.</param>
        /// <param name="startAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process.</param>
        /// <param name="parameter">A pointer to a variable to be passed to the thread function.</param>
        /// <param name="creationFlags"></param>
        /// <returns>If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is IntPtr.Zero.To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThread(
            IntPtr handle,
            IntPtr startAddress,
            IntPtr parameter,
            ThreadCreationFlags creationFlags)
            => Kernel32.CreateRemoteThreadEx(handle, startAddress, parameter, creationFlags);

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process.
        /// </summary>
        /// <param name="handle">A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION, PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights, and may fail without these rights on certain platforms. For more information, see Process Security and Access Rights.</param>
        /// <param name="startAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process.</param>
        /// <param name="parameter">A pointer to a variable to be passed to the thread function.</param>
        /// <param name="creationFlags"></param>
        /// <param name="threadId">A pointer to a variable that receives the thread identifier. If this parameter is NULL, the thread identifier is not returned.</param>
        /// <returns>If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is IntPtr.Zero.To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThread(
            IntPtr handle,
            IntPtr startAddress,
            IntPtr parameter,
            ThreadCreationFlags creationFlags,
            out uint threadId)
            => Kernel32.CreateRemoteThreadEx(handle, startAddress, parameter, creationFlags, out threadId);

        /// <summary>
        /// Creates a thread that runs in the virtual address space of another process.
        /// </summary>
        /// <param name="handle">A handle to the process in which the thread is to be created. The handle must have the PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION, PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ access rights, and may fail without these rights on certain platforms. For more information, see Process Security and Access Rights.</param>
        /// <param name="startAddress">A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be executed by the thread and represents the starting address of the thread in the remote process. The function must exist in the remote process.</param>
        /// <param name="parameter">A pointer to a variable to be passed to the thread function.</param>
        /// <returns>If the function succeeds, the return value is a handle to the new thread. If the function fails, the return value is IntPtr.Zero.To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CreateRemoteThread(
            IntPtr handle,
            IntPtr startAddress,
            IntPtr parameter)
            => Kernel32.CreateRemoteThreadEx(handle, startAddress, parameter);
    }
}
