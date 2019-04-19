using System;

namespace ProcessMemoryUtilities.Memory
{
    /// <summary>
    /// Defines the different types of memory allocations.
    /// </summary>
    [Flags]
    public enum AllocationType : uint
    {
        /// <summary>
        /// Allocates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved memory pages. The
        /// function also guarantees that when the caller later initially accesses the memory, the contents will be zero. Actual physical pages are not
        /// allocated unless/until the virtual addresses are actually accessed. To reserve and commit pages in one step, call VirtualAllocEx with
        /// MEM_COMMIT | MEM_RESERVE. Attempting to commit a specific address range by specifying MEM_COMMIT without MEM_RESERVE and a non-NULL
        /// lpAddress fails unless the entire range has already been reserved. The resulting error code is ERROR_INVALID_ADDRESS. An attempt to commit
        /// a page that is already committed does not cause the function to fail. This means that you can commit pages without first determining the
        /// current commitment state of each page. If lpAddress specifies an address within an enclave, flAllocationType must be MEM_COMMIT.
        /// </summary>
        Commit = 0x1000,

        /// <summary>
        /// Reserves a range of the process's virtual address space without allocating any actual physical storage in memory or in the paging file on
        /// disk. You commit reserved pages by calling VirtualAllocEx again with MEM_COMMIT. To reserve and commit pages in one step, call
        /// VirtualAllocEx with MEM_COMMIT | MEM_RESERVE. Other memory allocation functions, such as malloc and LocalAlloc, cannot use reserved memory
        /// until it has been released.
        /// </summary>
        Reserve = 0x2000,

        /// <summary>
        /// Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest. The pages should not be read from or
        /// written to the paging file. However, the memory block will be used again later, so it should not be decommitted. This value cannot be used
        /// with any other value. Using this value does not guarantee that the range operated on with MEM_RESET will contain zeros. If you want the
        /// range to contain zeros, decommit the memory and then recommit it. When you use MEM_RESET, the VirtualAllocEx function ignores the value of
        /// fProtect. However, you must still set fProtect to a valid protection value, such as PAGE_NOACCESS. VirtualAllocEx returns an error if you
        /// use MEM_RESET and the range of memory is mapped to a file. A shared view is only acceptable if it is mapped to a paging file.
        /// </summary>
        Reset = 0x80000,

        /// <summary>
        /// Reserves an address range that can be used to map Address Windowing Extensions (AWE) pages. This value must be used with MEM_RESERVE and no
        /// other values.
        /// </summary>
        Physical = 0x400000,

        /// <summary>
        /// Allocates memory at the highest possible address. This can be slower than regular allocations, especially when there are many allocations.
        /// </summary>
        TopDown = 0x100000,

        /// <summary>
        /// Causes the system to track pages that are written to in the allocated region. If you specify this value, you must also specify MEM_RESERVE.
        /// To retrieve the addresses of the pages that have been written to since the region was allocated or the write-tracking state was reset, call
        /// the GetWriteWatch function. To reset the write-tracking state, call GetWriteWatch or ResetWriteWatch. The write-tracking feature remains
        /// enabled for the memory region until the region is freed.
        /// </summary>
        WriteWatch = 0x200000,

        /// <summary>
        /// MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully applied earlier. It indicates that the data in
        /// the specified memory range specified by lpAddress and dwSize is of interest to the caller and attempts to reverse the effects of MEM_RESET.
        /// If the function succeeds, that means all data in the specified address range is intact. If the function fails, at least some of the data in
        /// the address range has been replaced with zeroes. This value cannot be used with any other value. If MEM_RESET_UNDO is called on an address
        /// range which was not MEM_RESET earlier, the behavior is undefined. When you specify MEM_RESET, the VirtualAllocEx function ignores the value
        /// of flProtect. However, you must still set flProtect to a valid protection value, such as PAGE_NOACCESS. Windows Server 2008 R2, Windows 7,
        /// Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP: The MEM_RESET_UNDO flag is not supported until Windows 8 and
        /// Windows Server 2012.
        /// </summary>
        ResetUndo = 0x1000000,

        /// <summary>
        /// Allocates memory using large page support. The size and alignment must be a multiple of the large-page minimum. To obtain this value, use
        /// the GetLargePageMinimum function. If you specify this value, you must also specify MEM_RESERVE and MEM_COMMIT.
        /// </summary>
        LargePages = 0x20000000
    }

    /// <summary>
    /// Defines the different types of free operations.
    /// </summary>
    [Flags]
    public enum FreeType : uint
    {
        /// <summary>
        /// To coalesce two adjacent placeholders, specify MEM_RELEASE | MEM_COALESCE_PLACEHOLDERS. When you coalesce placeholders, lpAddress and
        /// dwSize must exactly match those of the placeholder.
        /// </summary>
        CoalescePlaceholders = 0x00000001,

        /// <summary>
        /// Frees an allocation back to a placeholder (after you've replaced a placeholder with a private allocation using VirtualAlloc2 or
        /// Virtual2AllocFromApp). To split a placeholder into two placeholders, specify MEM_RELEASE | MEM_PRESERVE_PLACEHOLDER.
        /// </summary>
        PreservePlaceholder = 0x00000002,

        /// <summary>
        /// Decommits the specified region of committed pages. After the operation, the pages are in the reserved state. The function does not fail if
        /// you attempt to decommit an uncommitted page. This means that you can decommit a range of pages without first determining their current
        /// commitment state. Do not use this value with MEM_RELEASE. The MEM_DECOMMIT value is not supported when the lpAddress parameter provides the
        /// base address for an enclave.
        /// </summary>
        Decommit = 0x4000,

        /// <summary>
        /// Releases the specified region of pages, or placeholder (for a placeholder, the address space is released and available for other
        /// allocations). After the operation, the pages are in the free state. If you specify this value, dwSize must be 0 (zero), and lpAddress must
        /// point to the base address returned by the VirtualAllocEx function when the region is reserved. The function fails if either of these
        /// conditions is not met. If any pages in the region are committed currently, the function first decommits, and then releases them. The
        /// function does not fail if you attempt to release pages that are in different states, some reserved and some committed. This means that you
        /// can release a range of pages without first determining the current commitment state. Do not use this value with MEM_DECOMMIT.
        /// </summary>
        Release = 0x8000
    }

    /// <summary>
    /// Defines the memory protection constants.
    /// </summary>
    [Flags]
    public enum MemoryProtectionFlags : uint
    {
        /// <summary>
        /// Enables execute access to the committed region of pages. An attempt to write to the committed region results in an access violation. This
        /// flag is not supported by the CreateFileMapping function.
        /// </summary>
        Execute = 0x10,

        /// <summary>
        /// Enables execute or read-only access to the committed region of pages. An attempt to write to the committed region results in an access
        /// violation. Windows Server 2003 and Windows XP: This attribute is not supported by the CreateFileMapping function until Windows XP with SP2
        /// and Windows Server 2003 with SP1.
        /// </summary>
        ExecuteRead = 0x20,

        /// <summary>
        /// Enables execute, read-only, or read/write access to the committed region of pages. Windows Server 2003 and Windows XP: This attribute is
        /// not supported by the CreateFileMapping function until Windows XP with SP2 and Windows Server 2003 with SP1.
        /// </summary>
        ExecuteReadWrite = 0x40,

        /// <summary>
        /// Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a committed
        /// copy-on-write page results in a private copy of the page being made for the process. The private page is marked as PAGE_EXECUTE_READWRITE,
        /// and the change is written to the new page. This flag is not supported by the VirtualAlloc or VirtualAllocEx functions. Windows Vista,
        /// Windows Server 2003 and Windows XP: This attribute is not supported by the CreateFileMapping function until Windows Vista with SP1 and
        /// Windows Server 2008.
        /// </summary>
        ExecuteWriteCopy = 0x80,

        /// <summary>
        /// Disables all access to the committed region of pages. An attempt to read from, write to, or execute the committed region results in an
        /// access violation. This flag is not supported by the CreateFileMapping function.
        /// </summary>
        NoAccess = 0x01,

        /// <summary>
        /// Enables read-only access to the committed region of pages. An attempt to write to the committed region results in an access violation. If
        /// Data Execution Prevention is enabled, an attempt to execute code in the committed region results in an access violation.
        /// </summary>
        ReadOnly = 0x02,

        /// <summary>
        /// Enables read-only or read/write access to the committed region of pages. If Data Execution Prevention is enabled, attempting to execute
        /// code in the committed region results in an access violation.
        /// </summary>
        ReadWrite = 0x04,

        /// <summary>
        /// Enables read-only or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a committed copy-on-write page
        /// results in a private copy of the page being made for the process. The private page is marked as PAGE_READWRITE, and the change is written
        /// to the new page. If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access
        /// violation. This flag is not supported by the VirtualAlloc or VirtualAllocEx functions.
        /// </summary>
        WriteCopy = 0x08,

        /// <summary>
        /// Pages in the region become guard pages. Any attempt to access a guard page causes the system to raise a STATUS_GUARD_PAGE_VIOLATION
        /// exception and turn off the guard page status. Guard pages thus act as a one-time access alarm. For more information, see Creating Guard
        /// Pages. When an access attempt leads the system to turn off guard page status, the underlying page protection takes over. If a guard page
        /// exception occurs during a system service, the service typically returns a failure status indicator. This value cannot be used with
        /// PAGE_NOACCESS. This flag is not supported by the CreateFileMapping function.
        /// </summary>
        GuardModifierflag = 0x100,

        /// <summary>
        /// Sets all pages to be non-cachable. Applications should not use this attribute except when explicitly required for a device. Using the
        /// interlocked functions with memory that is mapped with SEC_NOCACHE can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception. The
        /// PAGE_NOCACHE flag cannot be used with the PAGE_GUARD, PAGE_NOACCESS, or PAGE_WRITECOMBINE flags. The PAGE_NOCACHE flag can be used only
        /// when allocating private memory with the VirtualAlloc, VirtualAllocEx, or VirtualAllocExNuma functions. To enable non-cached memory access
        /// for shared memory, specify the SEC_NOCACHE flag when calling the CreateFileMapping function.
        /// </summary>
        NoCacheModifierflag = 0x200,

        /// <summary>
        /// Sets all pages to be write-combined. Applications should not use this attribute except when explicitly required for a device. Using the
        /// interlocked functions with memory that is mapped as write-combined can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception. The
        /// PAGE_WRITECOMBINE flag cannot be specified with the PAGE_NOACCESS, PAGE_GUARD, and PAGE_NOCACHE flags. The PAGE_WRITECOMBINE flag can be
        /// used only when allocating private memory with the VirtualAlloc, VirtualAllocEx, or VirtualAllocExNuma functions. To enable write-combined
        /// memory access for shared memory, specify the SEC_WRITECOMBINE flag when calling the CreateFileMapping function. Windows Server 2003 and
        /// Windows XP: This flag is not supported until Windows Server 2003 with SP1.
        /// </summary>
        WriteCombineModifierflag = 0x400
    }

    /// <summary>
    /// Defines process security and access rights.
    /// </summary>
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        /// <summary>
        /// All possible access rights for a process object.Windows Server 2003 and Windows XP: The size of the PROCESS_ALL_ACCESS flag increased on
        /// Windows Server 2008 and Windows Vista. If an application compiled for Windows Server 2008 and Windows Vista is run on Windows Server 2003
        /// or Windows XP, the PROCESS_ALL_ACCESS flag is too large and the function specifying this flag fails with ERROR_ACCESS_DENIED. To avoid this
        /// problem, specify the minimum set of access rights required for the operation. If PROCESS_ALL_ACCESS must be used, set _WIN32_WINNT to the
        /// minimum operating system targeted by your application (for example, #define _WIN32_WINNT _WIN32_WINNT_WINXP). For more information, see
        /// Using the Windows Headers.
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

        /// <summary>
        /// Required to retrieve certain information about a process (see GetExitCodeProcess, GetPriorityClass, IsProcessInJob,
        /// QueryFullProcessImageName). A handle that has the PROCESS_QUERY_INFORMATION access right is automatically granted
        /// PROCESS_QUERY_LIMITED_INFORMATION.Windows Server 2003 and Windows XP: This access right is not supported.
        /// </summary>
        QueryLimitedInformation = 0x00001000,

        /// <summary>
        /// Required to wait for the process to terminate using the wait functions.
        /// </summary>
        Synchronize = 0x00100000
    }

    /// <summary>
    /// Defines flags that control the creation of a remote thread.
    /// </summary>
    [Flags]
    public enum ThreadCreationFlags : uint
    {
        /// <summary>
        /// The thread runs immediately after creation.
        /// </summary>
        Immediately = 0,

        /// <summary>
        /// The thread is created in a suspended state and does not run until the ResumeThread function is called.
        /// </summary>
        Suspended = 0x4,

        /// <summary>
        /// The dwStackSize parameter specifies the initial reserve size of the stack. If this flag is not specified, dwStackSize specifies the commit
        /// size.
        /// </summary>
        StackSizeParamIsAReservation = 0x10000
    }

    /// <summary>
    /// Defines the events that cause a function like WaitForSingleObject to return.
    /// </summary>
    public enum WaitObjectResult : uint
    {
        /// <summary>
        /// The specified object is a mutex object that was not released by the thread that owned the mutex object before the owning thread terminated.
        /// Ownership of the mutex object is granted to the calling thread and the mutex state is set to nonsignaled. If the mutex was protecting
        /// persistent state information, you should check it for consistency.
        /// </summary>
        Abandoned = 0x80,

        /// <summary>
        /// The state of the specified object is signaled.
        /// </summary>
        Success = 0x0,

        /// <summary>
        /// The time-out interval elapsed, and the object's state is nonsignaled.
        /// </summary>
        Timeout = 0x102,

        /// <summary>
        /// The function has failed. To get extended error information, call GetLastError.
        /// </summary>
        Failed = 0xFFFFFFFF
    }
}
