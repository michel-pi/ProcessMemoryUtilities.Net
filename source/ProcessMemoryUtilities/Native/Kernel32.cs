using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

using ProcessMemoryUtilities.Internal;

using InlineIL;
using static InlineIL.IL.Emit;

namespace ProcessMemoryUtilities.Native
{
    /// <summary>
    /// Provides access to some methods of kernel32.dll
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public static partial class Kernel32
    {
        private static readonly IntPtr _createRemoteThreadEx;
        private static readonly IntPtr _openProcess;
        private static readonly IntPtr _waitForSingleObject;

        /// <summary>
        /// A constant used to specify an infinite waiting period
        /// </summary>
        public const uint INFINITE = uint.MaxValue;

        static Kernel32()
        {
            var lib = DynamicImport.ImportLibrary("kernel32.dll");

            _createRemoteThreadEx = DynamicImport.ImportMethod(lib, "CreateRemoteThreadEx");
            _openProcess = DynamicImport.ImportMethod(lib, "OpenProcess");
            _waitForSingleObject = DynamicImport.ImportMethod(lib, "WaitForSingleObject");
        }

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="desiredAccess">The access to the process object. This access right is checked against the security descriptor for the process.</param>
        /// <param name="inheritHandle">If this value is TRUE, processes created by this process will inherit the handle. Otherwise, the processes do not inherit this handle.</param>
        /// <param name="processId">The identifier of the local process to be opened.</param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified process. If the function fails, the return value is IntPtr.Zero. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr OpenProcess(ProcessAccessFlags desiredAccess, bool inheritHandle, int processId)
        {
            Ldarg(nameof(desiredAccess));

            Ldarg(nameof(inheritHandle));
            Conv_I4();

            Ldarg(nameof(processId));

            Ldsfld(new FieldRef(typeof(Kernel32), nameof(_openProcess)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(IntPtr),
                typeof(ProcessAccessFlags),
                typeof(int),
                typeof(int)));

            return IL.Return<IntPtr>();
        }

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="desiredAccess">The access to the process object. This access right is checked against the security descriptor for the process.</param>
        /// <param name="processId">The identifier of the local process to be opened.</param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified process. If the function fails, the return value is IntPtr.Zero. To get extended error information, call GetLastError.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr OpenProcess(ProcessAccessFlags desiredAccess, int processId)
            => OpenProcess(desiredAccess, false, processId);

        /// <summary>
        /// Waits until the specified object is in the signaled state or the time-out interval elapses.
        /// </summary>
        /// <param name="handle">A handle to the object.</param>
        /// <param name="timeout">The time-out interval, in milliseconds. If a nonzero value is specified, the function waits until the object is signaled or the interval elapses. If dwMilliseconds is zero, the function does not enter a wait state if the object is not signaled; it always returns immediately. If dwMilliseconds is INFINITE, the function will return only when the object is signaled.</param>
        /// <returns>If the function succeeds, the return value indicates the event that caused the function to return.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WaitObjectResult WaitForSingleObject(IntPtr handle, uint timeout)
        {
            Ldarg(nameof(handle));
            Ldarg(nameof(timeout));

            Ldsfld(new FieldRef(typeof(Kernel32), nameof(_waitForSingleObject)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(WaitObjectResult),
                typeof(IntPtr),
                typeof(uint)));

            return IL.Return<WaitObjectResult>();
        }

        /// <summary>
        /// Turns the given WaitObjectResult into one of the defined enum values by stripping the objects index.
        /// </summary>
        /// <param name="value">A WaitObjectResult.</param>
        /// <returns>A WaitObjectResult which is guaranteed to be one of the defined enum values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WaitObjectResult GetRealWaitObjectResult(WaitObjectResult value)
        {
            Ldarg_0();
            Ldc_I4((int)WaitObjectResult.Abandoned);
            Blt_Un_S("IL_0022");

            Ldarg_0();
            Ldc_I4((int)WaitObjectResult.Timeout);
            Blt_Un_S("IL_001C");

            Ldarg_0();
            Ldc_I4_M1();
            Blt_Un_S("IL_0016");

            Ldc_I4_M1();
            Ret();

            IL.MarkLabel("IL_0016");

            Ldc_I4((int)WaitObjectResult.Timeout);
            Ret();

            IL.MarkLabel("IL_001C");

            Ldc_I4((int)WaitObjectResult.Abandoned);
            Ret();

            IL.MarkLabel("IL_0022");

            Ldc_I4_0();
            return IL.Return<WaitObjectResult>();
        }

        /// <summary>
        /// Turns the given WaitObjectResult into one of the defined enum values and returns the objects index.
        /// </summary>
        /// <param name="value">A WaitObjectResult</param>
        /// <param name="index">A variable that receives the index of the awaited object.</param>
        /// <returns>A WaitObjectResult which is guaranteed to be one of the defined enum values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WaitObjectResult GetRealWaitObjectResult(WaitObjectResult value, out int index)
        {
            Br("skip");
            index = 0;
            IL.MarkLabel("skip");

            Ldarg_0();
            Ldc_I4((int)WaitObjectResult.Abandoned);
            Bge_Un_S("IL_000D");

            Ldarg_1();
            Ldarg_0();
            Stind_I4();
            Ldc_I4_0();
            Ret();

            IL.MarkLabel("IL_000D");

            Ldarg_0();
            Ldc_I4((int)WaitObjectResult.Timeout);
            Bge_Un_S("IL_0024");

            Ldarg_1();
            Ldarg_0();
            Ldc_I4((int)WaitObjectResult.Abandoned);
            Sub();
            Stind_I4();
            Ldc_I4((int)WaitObjectResult.Abandoned);
            Ret();

            IL.MarkLabel("IL_0024");

            Ldarg_0();
            Ldc_I4_M1();
            Bge_Un_S("IL_0037");

            Ldarg_1();
            Ldarg_0();
            Ldc_I4((int)WaitObjectResult.Timeout);
            Sub();
            Stind_I4();
            Ldc_I4((int)WaitObjectResult.Timeout);
            Ret();

            IL.MarkLabel("IL_0037");

            Ldarg_1();
            Ldc_I4_0();
            Stind_I4();
            Ldc_I4_M1();

            return IL.Return<WaitObjectResult>();
        }
    }
}
