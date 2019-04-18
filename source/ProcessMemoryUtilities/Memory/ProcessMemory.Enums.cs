using System;

namespace ProcessMemoryUtilities.Memory
{
    [Flags]
    public enum AllocationType : uint
    {
        Commit = 0x1000,
        Reserve = 0x2000,
        Decommit = 0x4000,
        Release = 0x8000,
        Reset = 0x80000,
        Physical = 0x400000,
        TopDown = 0x100000,
        WriteWatch = 0x200000,
        LargePages = 0x20000000
    }

    [Flags]
    public enum FreeType : uint
    {
        Decommit = 0x4000,
        Release = 0x8000
    }

    [Flags]
    public enum MemoryProtectionFlags : uint
    {
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
        ExecuteWriteCopy = 0x80,
        NoAccess = 0x01,
        ReadOnly = 0x02,
        ReadWrite = 0x04,
        WriteCopy = 0x08,
        GuardModifierflag = 0x100,
        NoCacheModifierflag = 0x200,
        WriteCombineModifierflag = 0x400
    }

    /// <summary>
    /// Process Security and Access Rights
    /// </summary>
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        /// <summary>
        /// All possible access rights for a process object.Windows Server 2003 and Windows XP: The size of the PROCESS_ALL_ACCESS flag increased on Windows Server 2008 and Windows Vista. If an application compiled for Windows Server 2008 and Windows Vista is run on Windows Server 2003 or Windows XP, the PROCESS_ALL_ACCESS flag is too large and the function specifying this flag fails with ERROR_ACCESS_DENIED. To avoid this problem, specify the minimum set of access rights required for the operation. If PROCESS_ALL_ACCESS must be used, set _WIN32_WINNT to the minimum operating system targeted by your application (for example, #define _WIN32_WINNT _WIN32_WINNT_WINXP). For more information, see Using the Windows Headers.
        /// </summary>
        All = 0x001FFFFF,

        /// <summary>
        /// Required to terminate a process using TerminateProcess.
        /// </summary>
        Terminate = 0x00000001,

        /// <summary>
        /// Required to create a thread.
        /// </summary>
        CreateThread = 0x00000002,

        /// <summary>
        /// Required to perform an operation on the address space of a process (see VirtualProtectEx and WriteProcessMemory).
        /// </summary>
        VirtualMemoryOperation = 0x00000008,

        /// <summary>
        /// Required to read memory in a process using ReadProcessMemory.
        /// </summary>
        VirtualMemoryRead = 0x00000010,

        /// <summary>
        /// Required to write to memory in a process using WriteProcessMemory.
        /// </summary>
        VirtualMemoryWrite = 0x00000020,

        /// <summary>
        /// Required to duplicate a handle using DuplicateHandle.
        /// </summary>
        DuplicateHandle = 0x00000040,

        /// <summary>
        /// Required to create a process.
        /// </summary>
        CreateProcess = 0x000000080,

        /// <summary>
        /// Required to set memory limits using SetProcessWorkingSetSize.
        /// </summary>
        SetQuota = 0x00000100,

        /// <summary>
        /// Required to set certain information about a process, such as its priority class (see SetPriorityClass).
        /// </summary>
        SetInformation = 0x00000200,

        /// <summary>
        /// Required to retrieve certain information about a process, such as its token, exit code, and priority class (see OpenProcessToken).
        /// </summary>
        QueryInformation = 0x00000400,

        /// <summary>
        /// Required to set memory limits using SetProcessWorkingSetSize.
        /// </summary>
        SuspendResume = 0x0800,

        QueryLimitedInformation = 0x00001000,

        /// <summary>
        /// Required to wait for the process to terminate using the wait functions.
        /// </summary>
        Synchronize = 0x00100000
    }

    [Flags]
    public enum ThreadCreationFlags : uint
    {
        Immediately = 0,
        Suspended = 0x4,
        StackSizeParamIsAReservation = 0x10000
    }

    public enum WaitObjectResult : uint
    {
        Abandoned = 0x80,
        Success = 0x0,
        Timeout = 0x102,
        Failed = 0xFFFFFFFF
    }
}
