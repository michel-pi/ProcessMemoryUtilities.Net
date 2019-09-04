using System;
using System.Runtime.CompilerServices;

using ProcessMemoryUtilities.Native;

using static ProcessMemoryUtilities.Native.NtDll;

namespace ProcessMemoryUtilities.Managed
{
    public static partial class NativeWrapper
    {
        /// <summary>
        /// Reserves, commits, or changes the state of a region of memory within the virtual address space of a specified process. The function initializes the memory it allocates to zero.
        /// </summary>
        /// <param name="handle">The handle to a process. The function allocates memory within the virtual address space of this process. The handle must have the PROCESS_VM_OPERATION access right.For more information, see Process Security and Access Rights.</param>
        /// <param name="address">The pointer that specifies a desired starting address for the region of pages that you want to allocate. If you are reserving memory, the function rounds this address down to the nearest multiple of the allocation granularity. If you are committing memory that is already reserved, the function rounds this address down to the nearest page boundary.To determine the size of a page and the allocation granularity on the host computer, use the GetSystemInfo function. If lpAddress is NULL, the function determines where to allocate the region. If this address is within an enclave that you have not initialized by calling InitializeEnclave, VirtualAllocEx allocates a page of zeros for the enclave at that address.The page must be previously uncommitted, and will not be measured with the EEXTEND instruction of the Intel Software Guard Extensions programming model. If the address in within an enclave that you initialized, then the allocation operation fails with the ERROR_INVALID_ADDRESS error.</param>
        /// <param name="size">The size of the region of memory to allocate, in bytes. If lpAddress is NULL, the function rounds dwSize up to the next page boundary. If lpAddress is not NULL, the function allocates all pages that contain one or more bytes in the range from lpAddress to lpAddress+dwSize.This means, for example, that a 2-byte range that straddles a page boundary causes the function to allocate both pages.</param>
        /// <param name="allocationType">The type of memory allocation. This parameter must contain one of the following values.</param>
        /// <param name="memoryProtection">The memory protection for the region of pages to be allocated. If the pages are being committed, you can specify any one of the memory protection constants.</param>
        /// <returns>If the function succeeds, the return value is the base address of the allocated region of pages. If the function fails, the return value is NULL.To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr VirtualAllocEx(IntPtr handle, IntPtr address, IntPtr size, AllocationType allocationType, MemoryProtectionFlags memoryProtection)
        {
            uint status = NtAllocateVirtualMemory(handle, size, allocationType, memoryProtection, out address);

            if (CaptureErrors) LastError = (int)status;

            return NtSuccess(status) ? address : IntPtr.Zero;
        }

        /// <summary>
        /// Releases, decommits, or releases and decommits a region of memory within the virtual address space of a specified process.
        /// </summary>
        /// <param name="handle">A handle to a process. The function frees memory within the virtual address space of the process. The handle must have the PROCESS_VM_OPERATION access right.For more information, see Process Security and Access Rights.</param>
        /// <param name="address">A pointer to the starting address of the region of memory to be freed. If the dwFreeType parameter is MEM_RELEASE, lpAddress must be the base address returned by the VirtualAllocEx function when the region is reserved.</param>
        /// <param name="size">The size of the region of memory to free, in bytes. If the dwFreeType parameter is MEM_RELEASE, dwSize must be 0 (zero). The function frees the entire region that is reserved in the initial allocation call to VirtualAllocEx. If dwFreeType is MEM_DECOMMIT, the function decommits all memory pages that contain one or more bytes in the range from the lpAddress parameter to (lpAddress+dwSize). This means, for example, that a 2-byte region of memory that straddles a page boundary causes both pages to be decommitted.If lpAddress is the base address returned by VirtualAllocEx and dwSize is 0 (zero), the function decommits the entire region that is allocated by VirtualAllocEx.After that, the entire region is in the reserved state.</param>
        /// <param name="freeType">The type of free operation.</param>
        /// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VirtualFreeEx(IntPtr handle, IntPtr address, IntPtr size, FreeType freeType)
        {
            uint status = NtFreeVirtualMemory(handle, address, size, freeType);

            if (CaptureErrors) LastError = (int)status;

            return NtSuccess(status);
        }

        /// <summary>
        /// Changes the protection on a region of committed pages in the virtual address space of a specified process.
        /// </summary>
        /// <param name="handle">A handle to the process whose memory protection is to be changed. The handle must have the PROCESS_VM_OPERATION access right. For more information, see Process Security and Access Rights.</param>
        /// <param name="address">A pointer to the base address of the region of pages whose access protection attributes are to be changed. All pages in the specified region must be within the same reserved region allocated when calling the VirtualAlloc or VirtualAllocEx function using MEM_RESERVE. The pages cannot span adjacent reserved regions that were allocated by separate calls to VirtualAlloc or VirtualAllocEx using MEM_RESERVE.</param>
        /// <param name="size">The size of the region whose access protection attributes are changed, in bytes. The region of affected pages includes all pages containing one or more bytes in the range from the lpAddress parameter to (lpAddress+dwSize). This means that a 2-byte range straddling a page boundary causes the protection attributes of both pages to be changed.</param>
        /// <param name="newProtect">The memory protection option. This parameter can be one of the memory protection constants. For mapped views, this value must be compatible with the access protection specified when the view was mapped(see MapViewOfFile, MapViewOfFileEx, and MapViewOfFileExNuma).</param>
        /// <param name="oldProtect">A pointer to a variable that receives the previous access protection of the first page in the specified region of pages. If this parameter is NULL or does not point to a valid variable, the function fails.</param>
        /// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool VirtualProtectEx(IntPtr handle, IntPtr address, IntPtr size, MemoryProtectionFlags newProtect, out MemoryProtectionFlags oldProtect)
        {
            uint status = NtProtectVirtualMemory(handle, address, size, newProtect, out oldProtect);

            if (CaptureErrors) LastError = (int)status;

            return NtSuccess(status);
        }
    }
}
