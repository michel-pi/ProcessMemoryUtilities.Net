#pragma warning disable IDE0060 // Remove unused parameter

using System;
using System.Runtime.CompilerServices;

using InlineIL;
using static InlineIL.IL.Emit;

namespace ProcessMemoryUtilities.Memory
{
    /// <summary>
    /// Provides methods to work with unmanaged memory.
    /// </summary>
    public static class UnsafeMarshal
    {
        /// <summary>
        /// Allocates a new value type.
        /// </summary>
        /// <typeparam name="T">The type of the object to allocate.</typeparam>
        /// <returns>The allocated object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Allocate<T>() where T : struct
        {
            return default;
        }

        /// <summary>
        /// Allocates a new array of value types.
        /// </summary>
        /// <typeparam name="T">The type of the objects inside the array.</typeparam>
        /// <param name="length">The length of the array to allocate.</param>
        /// <returns>The allocated array.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] AllocateArray<T>(int length) where T : struct
        {
            return new T[length];
        }

        /// <summary>
        /// Allocates a new value type without zeroing its memory.
        /// </summary>
        /// <typeparam name="T">The type of the object to allocate.</typeparam>
        /// <returns>The allocated object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T AllocateNonZero<T>() where T : struct
        {
            IL.DeclareLocals(new LocalVar("buffer", typeof(T)));

            Ldloc("buffer");

            return IL.Return<T>();
        }

        /// <summary>
        /// Returns a pointer to the given by-ref parameter.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="value">The object whose pointer is obtained.</param>
        /// <returns>A pointer to the given value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr AsPointer<T>(ref T value) where T : struct
        {
            Ldarg(nameof(value));
            Conv_U();

            return IL.Return<IntPtr>();
        }

        /// <summary>
        /// Copies bytes from the source address to the destination address.
        /// </summary>
        /// <param name="source">The source address to copy from.</param>
        /// <param name="destination">The destination address to copy to.</param>
        /// <param name="length">The number of bytes to copy.</param>
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

        /// <summary>
        /// Copies a value of type <typeparamref name="T">T</typeparamref> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of value to copy.</typeparam>
        /// <param name="source">A pointer to the value to copy.</param>
        /// <param name="destination">The location to copy to.</param>
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

        /// <summary>
        /// Copies a value of type <typeparamref name="T">T</typeparamref> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of value to copy.</typeparam>
        /// <param name="source">A pointer to the value to copy.</param>
        /// <param name="destination">The location to copy to.</param>
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

        /// <summary>
        /// Reads a value of type <typeparamref name="T">T</typeparamref> from the given location.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="address">The location to read from.</param>
        /// <returns>An object of type <typeparamref name="T">T</typeparamref> read from the given location.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(IntPtr address) where T : struct
        {
            Ldarg(nameof(address));
            Conv_I();

            Ldobj(typeof(T));

            return IL.Return<T>();
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T">T</typeparamref> from the given location.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="address">The location to read from.</param>
        /// <param name="offset">The byte offset to add.</param>
        /// <returns>An object of type <typeparamref name="T">T</typeparamref> read from the given location.</returns>
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

        /// <summary>
        /// Reads a value from the array.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="array">The array to read from.</param>
        /// <returns>An object of type <typeparamref name="T">T</typeparamref> read from the given array.</returns>
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

        /// <summary>
        /// Reads a value from the array.
        /// </summary>
        /// <typeparam name="T">The type to read.</typeparam>
        /// <param name="array">The array to read from.</param>
        /// <param name="offset">The byte offset to add.</param>
        /// <returns>An object of type <typeparamref name="T">T</typeparamref> read from the given array.</returns>
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

        /// <summary>
        /// Casts the value of the by-ref parameter from <typeparamref name="TFrom">TFrom</typeparamref> to
        /// <typeparamref name="TTo">TTo</typeparamref>.
        /// </summary>
        /// <typeparam name="TFrom">The initial type of the object.</typeparam>
        /// <typeparam name="TTo">The type to cast the given by-ref parameter to.</typeparam>
        /// <param name="value">The original object.</param>
        /// <returns>The value of the by-ref parameter reinterpreted as another type.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTo ReinterpretCast<TFrom, TTo>(ref TFrom value) where TTo : struct where TFrom : struct
        {
            Ldarg(nameof(value));
            Conv_I();

            Ldobj(typeof(TTo));

            return IL.Return<TTo>();
        }

        /// <summary>
        /// Returns the size of an object of the given type parameter.
        /// </summary>
        /// <typeparam name="T">The type of object whose size is retrieved.</typeparam>
        /// <returns>The size of an object of type <typeparamref name="T">T</typeparamref>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SizeOf<T>()
        {
            Sizeof(typeof(T));

            return IL.Return<int>();
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T">T</typeparamref> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of value to write.</typeparam>
        /// <param name="address">The location to write to.</param>
        /// <param name="value">The value to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(IntPtr address, T value) where T : struct
        {
            Ldarg(nameof(address));
            Ldarg(nameof(value));

            Stobj(typeof(T));
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T">T</typeparamref> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of value to write.</typeparam>
        /// <param name="address">The location to write to.</param>
        /// <param name="offset">The byte offset to add to the address.</param>
        /// <param name="value">The value to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(IntPtr address, int offset, T value) where T : struct
        {
            Ldarg(nameof(address));
            Ldarg(nameof(offset));

            Add();

            Ldarg(nameof(value));

            Stobj(typeof(T));
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T">T</typeparamref> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of value to write.</typeparam>
        /// <param name="buffer">The buffer to be written.</param>
        /// <param name="value">The value to write.</param>
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

        /// <summary>
        /// Writes a value of type <typeparamref name="T">T</typeparamref> to the given location.
        /// </summary>
        /// <typeparam name="T">The type of value to write.</typeparam>
        /// <param name="buffer">The buffer to be written.</param>
        /// <param name="offset">The byte offset inside the buffer.</param>
        /// <param name="value">The value to write.</param>
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

        /// <summary>
        /// Initializes the values in the given array to zero.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="array">The array that should be initialized to zero.</param>
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

        /// <summary>
        /// Initializes a block of memory at the given address to zero.
        /// </summary>
        /// <param name="address">The location of the memory.</param>
        /// <param name="length">The length of the block in bytes.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ZeroMemory(IntPtr address, int length)
        {
            Ldarg(nameof(address));

            Ldc_I4_0();

            Ldarg(nameof(length));
            Conv_U4();

            Initblk();
        }

        /// <summary>
        /// Initializes the by-ref parameter to zero.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="value">A reference to an object.</param>
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
