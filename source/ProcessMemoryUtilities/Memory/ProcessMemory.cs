#pragma warning disable IDE0060 // Remove unused parameter

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ProcessMemoryUtilities.PInvoke;

using InlineIL;
using static InlineIL.IL.Emit;

namespace ProcessMemoryUtilities.Memory
{
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
        /// Provides a constant for OpenProcess with memory operation access.
        /// </summary>
        public const ProcessAccessFlags PROCESS_ALLOCATE_ACCESS = ProcessAccessFlags.VirtualMemoryOperation;

        /// <summary>
        /// Provides a constant for OpenProcess with execute access.
        /// </summary>
        public const ProcessAccessFlags PROCESS_EXECUTE_ACCESS = PROCESS_READ_WRITE_ACCESS | PROCESS_INFORMATION_ACCESS | ProcessAccessFlags.CreateThread;

        /// <summary>
        /// Provides a constant for OpenProcess with information access.
        /// </summary>
        public const ProcessAccessFlags PROCESS_INFORMATION_ACCESS = ProcessAccessFlags.QueryInformation | ProcessAccessFlags.QueryLimitedInformation;

        /// <summary>
        /// Provides a constant for OpenProcess with read access.
        /// </summary>
        public const ProcessAccessFlags PROCESS_READ_ACCESS = ProcessAccessFlags.VirtualMemoryRead;

        /// <summary>
        /// Provides a constant for OpenProcess with read and write access.
        /// </summary>
        public const ProcessAccessFlags PROCESS_READ_WRITE_ACCESS = PROCESS_READ_ACCESS | PROCESS_WRITE_ACCESS;

        /// <summary>
        /// Provides a constant for OpenProcess with write access.
        /// </summary>
        public const ProcessAccessFlags PROCESS_WRITE_ACCESS = ProcessAccessFlags.VirtualMemoryOperation | ProcessAccessFlags.VirtualMemoryWrite;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Close(IntPtr handle)
        {
            Ldarg(nameof(handle));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntClose)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr)));

            Ldc_I4(NT_STATUS_WARNING_START);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WaitObjectResult WaitForSingleObject(IntPtr handle, uint timeout)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(timeout));

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_kernelWaitForSingleObject)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(WaitObjectResult), typeof(IntPtr), typeof(uint)));

            return IL.Return<WaitObjectResult>();
        }

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
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
