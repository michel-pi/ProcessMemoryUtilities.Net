using System;
using System.Runtime.CompilerServices;

using InlineIL;
using static InlineIL.IL.Emit;

[assembly: InternalsVisibleTo("ProcessMemoryUtilities.Managed")]

namespace ProcessMemoryUtilities.Native
{
    internal static class InternalHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int SizeOf<T>() where T : unmanaged
        {
            Sizeof(typeof(T));
            return IL.Return<int>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void WriteIntPtr(IntPtr address, IntPtr value)
        {
            Ldarg(nameof(address));
            Ldarg(nameof(value));

            Stind_I();
        }
    }
}
