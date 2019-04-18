#pragma warning disable IDE0060 // Remove unused parameter

using System;
using System.Runtime.CompilerServices;

using InlineIL;
using static InlineIL.IL.Emit;

namespace ProcessMemoryUtilities.Memory
{
    public static class UnsafeMarshal
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Allocate<T>() where T : struct
        {
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] AllocateArray<T>(int length) where T : struct
        {
            return new T[length];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T AllocateNonZero<T>() where T : struct
        {
            IL.DeclareLocals(new LocalVar("buffer", typeof(T)));

            Ldloc("buffer");

            return IL.Return<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr AsPointer<T>(ref T value) where T : struct
        {
            Ldarg(nameof(value));
            Conv_U();

            return IL.Return<IntPtr>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(IntPtr source, IntPtr destination, int length)
        {
            Ldarg(nameof(destination));
            Conv_I();

            Ldarg(nameof(source));
            Conv_I();

            Ldarg(nameof(length));
            Conv_U4();

            Cpblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy<T>(IntPtr source, ref T destination) where T : struct
        {
            Ldarg(nameof(destination));
            Conv_U();

            Ldarg(nameof(source));
            Conv_I();

            Sizeof(typeof(T));
            Conv_U4();

            Cpblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy<T>(ref T source, IntPtr destination) where T : struct
        {
            Ldarg(nameof(destination));
            Conv_I();

            Ldarg(nameof(source));
            Conv_U();

            Sizeof(typeof(T));
            Conv_U4();

            Cpblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(IntPtr address) where T : struct
        {
            Ldarg(nameof(address));
            Conv_I();

            Ldobj(typeof(T));

            return IL.Return<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(IntPtr address, int offset) where T : struct
        {
            Ldarg(nameof(address));
            Conv_I();

            Ldarg(nameof(offset));

            Add();

            Ldobj(typeof(T));

            return IL.Return<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(byte[] array) where T : struct
        {
            Ldarg(nameof(array));
            Ldc_I4_0();
            Ldelema(typeof(byte));
            Conv_I();

            Ldobj(typeof(T));

            return IL.Return<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(byte[] array, int offset) where T : struct
        {
            Ldarg(nameof(array));
            Ldarg(nameof(offset));
            Ldelema(typeof(byte));
            Conv_I();

            Ldobj(typeof(T));

            return IL.Return<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTo ReinterpretCast<TFrom,
        TTo>(ref TFrom value) where TTo : struct where TFrom : struct
        {
            Ldarg(nameof(value));
            Conv_I();

            Ldobj(typeof(TTo));

            return IL.Return<TTo>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOf<T>()
        {
            Sizeof(typeof(T));

            return IL.Return<int>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(IntPtr address, T value) where T : struct
        {
            Ldarg(nameof(address));
            Ldarg(nameof(value));

            Stobj(typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(IntPtr address, int offset, T value) where T : struct
        {
            Ldarg(nameof(address));
            Ldarg(nameof(offset));

            Add();

            Ldarg(nameof(value));

            Stobj(typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(byte[] buffer, T value) where T : struct
        {
            Ldarg(nameof(buffer));
            Ldc_I4_0();
            Ldelema(typeof(byte));
            Conv_I();

            Ldarg(nameof(value));

            Stobj(typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(byte[] buffer, int offset, T value) where T : struct
        {
            Ldarg(nameof(buffer));
            Ldarg(nameof(offset));
            Ldelema(typeof(byte));
            Conv_I();

            Ldarg(nameof(value));

            Stobj(typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ZeroArray<T>(T[] array) where T : struct
        {
            Ldarg(nameof(array));
            Ldc_I4_0();
            Ldelema(typeof(T));
            Conv_U();

            Ldc_I4_0();
            Conv_U1();

            Ldarg(nameof(array));
            Ldlen();

            Sizeof(typeof(T));

            Mul();
            Conv_U4();

            Initblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ZeroMemory(IntPtr address, int length)
        {
            Ldarg(nameof(address));

            Ldc_I4_0();

            Ldarg(nameof(length));
            Conv_U4();

            Initblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ZeroMemory<T>(ref T value)
        {
            Ldarg(nameof(value));

            Ldc_I4_0();

            Sizeof(typeof(T));
            Conv_U4();

            Initblk();
        }
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
