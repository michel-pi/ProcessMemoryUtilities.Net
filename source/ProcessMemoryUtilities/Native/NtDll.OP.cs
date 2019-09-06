using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ProcessMemoryUtilities.Internal;

using InlineIL;
using static InlineIL.IL.Emit;

namespace ProcessMemoryUtilities.Native
{
    public static partial class NtDll
    {
        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="handle">A valid handle to an open object.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtClose(IntPtr handle)
        {
            Ldarg(nameof(handle));

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntClose)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr)));

            return IL.Return<uint>();
        }

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="desiredAccess">The access to the process object. This access right is checked against the security descriptor for the process. This parameter can be one or more of the process access rights.</param>
        /// <param name="processId">The identifier of the local process to be opened.</param>
        /// <param name="handle">A variable that receives the opened handle.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtOpenProcess(ProcessAccessFlags desiredAccess, int processId, out IntPtr handle)
        {
            IL.DeclareLocals(
                new LocalVar("result", typeof(uint)),
                new LocalVar("localHandle", typeof(IntPtr)),
                new LocalVar("objectAttributes", typeof(ObjectAttributes)),
                new LocalVar("clientID", typeof(ClientID)));

            handle = default;

            Ldloca("localHandle");
            Initobj(typeof(IntPtr));

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

            Ldloca("localHandle");
            Conv_U();
            Ldarg(nameof(desiredAccess));
            Ldloca("objectAttributes");
            Conv_U();
            Ldloca("clientID");
            Conv_U();

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntOpenProcess)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr).MakePointerType(),
                typeof(ProcessAccessFlags),
                typeof(ObjectAttributes).MakePointerType(),
                typeof(ClientID).MakePointerType()));

            Stloc("result");

            Ldarg(nameof(handle));
            Ldloc("localHandle");
            Stind_I();

            Ldloc("result");
            return IL.Return<uint>();
        }

        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="desiredAccess">The access to the process object. This access right is checked against the security descriptor for the process. This parameter can be one or more of the process access rights.</param>
        /// <param name="inheritHandle">If this value is TRUE, processes created by this process will inherit the handle. Otherwise, the processes do not inherit this handle.</param>
        /// <param name="processId">The identifier of the local process to be opened.</param>
        /// <param name="handle">A variable that receives the opened handle.</param>
        /// <returns>Returns an NTSTATUS success or error code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint NtOpenProcess(ProcessAccessFlags desiredAccess, bool inheritHandle, int processId, out IntPtr handle)
        {
            IL.DeclareLocals(
                new LocalVar("result", typeof(uint)),
                new LocalVar("localHandle", typeof(IntPtr)),
                new LocalVar("objectAttributes", typeof(ObjectAttributes)),
                new LocalVar("clientID", typeof(ClientID)));

            handle = default;

            Ldloca("localHandle");
            Initobj(typeof(IntPtr));

            Ldloca("objectAttributes");
            Initobj(typeof(ObjectAttributes));

            Ldloca("objectAttributes");
            Sizeof(typeof(ObjectAttributes));
            Stfld(new FieldRef(typeof(ObjectAttributes), "Length"));

            Ldloca("objectAttributes");

            Ldarg(nameof(inheritHandle));
            Brtrue_S("true");

            Ldc_I4_0();
            Br_S("skip");

            IL.MarkLabel("true");

            Ldc_I4_2();

            IL.MarkLabel("skip");

            Stfld(new FieldRef(typeof(ObjectAttributes), "Attributes"));

            Ldloca("clientID");
            Initobj(typeof(ClientID));

            Ldloca("clientID");
            Ldarg(nameof(processId));
            Conv_I();
            Stfld(new FieldRef(typeof(ClientID), "UniqueProcess"));

            Ldloca("localHandle");
            Conv_U();
            Ldarg(nameof(desiredAccess));
            Ldloca("objectAttributes");
            Conv_U();
            Ldloca("clientID");
            Conv_U();

            Ldsfld(new FieldRef(typeof(NtDll), nameof(_ntOpenProcess)));
            Calli(new StandAloneMethodSig(
                CallingConvention.StdCall,
                typeof(uint),
                typeof(IntPtr).MakePointerType(),
                typeof(ProcessAccessFlags),
                typeof(ObjectAttributes).MakePointerType(),
                typeof(ClientID).MakePointerType()));

            Stloc("result");

            Ldarg(nameof(handle));
            Ldloc("localHandle");
            Stind_I();

            Ldloc("result");
            return IL.Return<uint>();
        }
    }
}
