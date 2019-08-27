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
        /// Reserves, commits, or changes the state of a region of memory within the virtual address space of a specified process. The function initializes the memory it allocates to zero.
        /// </summary>
        /// <param name="handle">The handle to a process. The function allocates memory within the virtual address space of this process. The handle must have the PROCESS_VM_OPERATION access right.For more information, see Process Security and Access Rights.</param>
        /// <param name="size">The size of the region of memory to allocate, in bytes.</param>
        /// <param name="allocationType">The type of memory allocation. Common flags are AllocationType.Commit | AllocationType.Reserve.</param>
        /// <param name="memoryProtection">The memory protection for the region of pages to be allocated. If the pages are being committed, you can specify any one of the memory protection constants.</param>
        /// <param name="address">A pointer to a variable that receives the base address of the allocated region of pages.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtAllocateVirtualMemory(
            IntPtr handle,
            IntPtr size,
            AllocationType allocationType,
            MemoryProtectionFlags memoryProtection,
            out IntPtr address)
        {
            IL.DeclareLocals(
                new LocalVar("result", typeof(uint)),
                new LocalVar("localAddress", typeof(IntPtr)));

            address = default;

            Ldloca("localAddress");
            Initobj(typeof(IntPtr));

            Ldarg(nameof(handle));

            Ldloca("localAddress");
            Conv_U();

            Ldc_I4_0();
            Conv_I();

            Ldarga(nameof(size));
            Conv_U();

            Ldarg(nameof(allocationType));
            Ldarg(nameof(memoryProtection));

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntAllocateVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr).MakePointerType(),
                typeof(IntPtr),
                typeof(IntPtr).MakePointerType(),
                typeof(AllocationType),
                typeof(MemoryProtectionFlags)));

            Stloc("result");

            Ldarg(nameof(address));
            Ldloc("localAddress");
            Stind_I();

            Ldloc("result");
            return IL.Return<uint>();
        }

        /// <summary>
        /// Releases, decommits, or releases and decommits a region of memory within the virtual address space of a specified process.
        /// </summary>
        /// <param name="handle">A handle to a process. The function frees memory within the virtual address space of the process. The handle must have the PROCESS_VM_OPERATION access right.For more information, see Process Security and Access Rights.</param>
        /// <param name="address">A pointer to the starting address of the region of memory to be freed. If the dwFreeType parameter is MEM_RELEASE, lpAddress must be the base address returned by the VirtualAllocEx function when the region is reserved.</param>
        /// <param name="size">The size of the region of memory to free, in bytes. If the dwFreeType parameter is MEM_RELEASE, dwSize must be 0 (zero). The function frees the entire region that is reserved in the initial allocation call to VirtualAllocEx. If dwFreeType is MEM_DECOMMIT, the function decommits all memory pages that contain one or more bytes in the range from the lpAddress parameter to (lpAddress+dwSize). This means, for example, that a 2-byte region of memory that straddles a page boundary causes both pages to be decommitted. If lpAddress is the base address returned by VirtualAllocEx and dwSize is 0 (zero), the function decommits the entire region that is allocated by VirtualAllocEx. After that, the entire region is in the reserved state.</param>
        /// <param name="freeType">The type of free operation. This parameter can be one of the FreeType values.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtFreeVirtualMemory(IntPtr handle, IntPtr address, IntPtr size, FreeType freeType)
        {
            Ldarg(nameof(handle));
            Ldarga(nameof(address));
            Conv_U();
            Ldarga(nameof(size));
            Conv_U();
            Ldarg(nameof(freeType));

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntFreeVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr).MakePointerType(),
                typeof(IntPtr).MakePointerType(),
                typeof(FreeType)));

            return IL.Return<uint>();
        }

        /// <summary>
        /// Changes the protection on a region of committed pages in the virtual address space of a specified process.
        /// </summary>
        /// <param name="handle">A handle to the process whose memory protection is to be changed. The handle must have the PROCESS_VM_OPERATION access right. For more information, see Process Security and Access Rights.</param>
        /// <param name="address">A pointer to the base address of the region of pages whose access protection attributes are to be changed. All pages in the specified region must be within the same reserved region allocated when calling the VirtualAlloc or VirtualAllocEx function using MEM_RESERVE. The pages cannot span adjacent reserved regions that were allocated by separate calls to VirtualAlloc or VirtualAllocEx using MEM_RESERVE.</param>
        /// <param name="size">The size of the region whose access protection attributes are changed, in bytes. The region of affected pages includes all pages containing one or more bytes in the range from the lpAddress parameter to (lpAddress+dwSize). This means that a 2-byte range straddling a page boundary causes the protection attributes of both pages to be changed.</param>
        /// <param name="newProtection">The memory protection option. This parameter can be one of the MemoryProtectionFlags.</param>
        /// <param name="oldProtection">A pointer to a variable that receives the previous MemoryProtectionFlags of the first page in the specified region of pages.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtProtectVirtualMemory(IntPtr handle, IntPtr address, IntPtr size, MemoryProtectionFlags newProtection, out MemoryProtectionFlags oldProtection)
        {
            IL.DeclareLocals(
                new LocalVar("localOldProtection", typeof(MemoryProtectionFlags)),
                new LocalVar("result", typeof(uint)));

            oldProtection = default;

            Ldarg(nameof(handle));
            Ldarga(nameof(address));
            Conv_U();
            Ldarga(nameof(size));
            Conv_U();
            Ldarg(nameof(newProtection));
            Ldloca("localOldProtection");

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntProtectVirtualMemory)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr),
                typeof(IntPtr).MakePointerType(),
                typeof(IntPtr).MakePointerType(),
                typeof(MemoryProtectionFlags),
                typeof(MemoryProtectionFlags).MakeByRefType()));

            Stloc("result");

            Ldarg(nameof(oldProtection));
            Ldloc("localOldProtection");
            Stind_I4();

            Ldloc("result");
            return IL.Return<uint>();
        }
    }
}
