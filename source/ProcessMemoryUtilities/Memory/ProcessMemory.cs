#pragma warning disable IDE0060 // Remove unused parameter

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using ProcessMemoryUtilities.PInvoke;

using InlineIL;
using static InlineIL.IL.Emit;

namespace ProcessMemoryUtilities.Memory
{
    public static class ProcessMemory
    {
        // https://gist.github.com/michel-pi/361f1f8bdca51235cb97aba0256d47e9
        private const int NT_STATUS_INFORMATION_MAX = 2147483647;

        private static readonly IntPtr _ntClose;
        private static readonly IntPtr _ntOpenProcess;
        private static readonly IntPtr _ntProtectVirtualMemory;
        private static readonly IntPtr _ntReadVirtualMemory;
        private static readonly IntPtr _ntWriteVirtualMemory;

        /// <summary>
        /// Provides a constant for OpenProcess with memory operation access.
        /// </summary>
        public const ProcessAccessFlags ProcessAllocateAccess = ProcessAccessFlags.VirtualMemoryOperation;

        /// <summary>
        /// Provides a constant for OpenProcess with execute access.
        /// </summary>
        public const ProcessAccessFlags ProcessExecuteAccess = ProcessReadWriteAccess | ProcessInformationAccess | ProcessAccessFlags.CreateThread;

        /// <summary>
        /// Provides a constant for OpenProcess with information access.
        /// </summary>
        public const ProcessAccessFlags ProcessInformationAccess = ProcessAccessFlags.QueryInformation | ProcessAccessFlags.QueryLimitedInformation;

        /// <summary>
        /// Provides a constant for OpenProcess with read access.
        /// </summary>
        public const ProcessAccessFlags ProcessReadAccess = ProcessAccessFlags.VirtualMemoryRead;

        /// <summary>
        /// Provides a constant for OpenProcess with read and write access.
        /// </summary>
        public const ProcessAccessFlags ProcessReadWriteAccess = ProcessReadAccess | ProcessWriteAccess;

        /// <summary>
        /// Provides a constant for OpenProcess with write access.
        /// </summary>
        public const ProcessAccessFlags ProcessWriteAccess = ProcessAccessFlags.VirtualMemoryOperation | ProcessAccessFlags.VirtualMemoryWrite;

        static ProcessMemory()
        {
            var ntdll = DynamicImport.ImportLibrary("ntdll.dll");

            _ntClose = DynamicImport.ImportMethod(ntdll, "NtClose");
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr OpenProcess(ProcessAccessFlags desiredAccess, int processId)
        {
            IL.DeclareLocals(
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
            Conv_I();
            Stfld(new FieldRef(typeof(ClientID), "UniqueProcess"));

            Ldloca("objectAttributes");
            Sizeof(typeof(ObjectAttributes));
            Stfld(new FieldRef(typeof(ObjectAttributes), "Length"));

            Ldloca("handle");
            Ldarg(nameof(desiredAccess));
            Ldloca("objectAttributes");
            Ldloca("clientID");

            Ldsfld(new FieldRef(typeof(ProcessMemory), nameof(_ntOpenProcess)));
            Calli(new StandAloneMethodSig(CallingConvention.StdCall, typeof(uint), typeof(IntPtr), typeof(uint), typeof(IntPtr), typeof(IntPtr)));

            Pop();

            Ldloc("handle");
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
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

            Ldc_I4(NT_STATUS_INFORMATION_MAX);
            Conv_U4();
            Clt();

            return IL.Return<bool>();
        }
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
