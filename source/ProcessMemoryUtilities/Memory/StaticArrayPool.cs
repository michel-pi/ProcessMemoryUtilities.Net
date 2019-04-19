using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ProcessMemoryUtilities.Memory
{
    /// <summary>
    /// Provides a generic, thread safe and static implementation of a pool of arrays.
    /// </summary>
    /// <typeparam name="T">The type of the array.</typeparam>
    public static class StaticArrayPool<T> where T : struct
    {
        private static readonly bool _isStruct;
        [ThreadStatic] private static Dictionary<int, List<T[]>> _pool;

        static StaticArrayPool()
        {
            var type = typeof(T);

            if (type.IsArray) throw new NotSupportedException("This class does not support arrays as a generic type.");

            _isStruct = type.IsValueType;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ClearArray(ref T[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            if (_isStruct)
            {
                ClearValueTypeArray(ref array);
            }
            else
            {
                ClearClassArray(ref array);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ClearClassArray(ref T[] array)
        {
            array = new T[array.Length];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ClearValueTypeArray(ref T[] array)
        {
            UnsafeMarshal.ZeroArray<T>(array);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InitializeArrayPool()
        {
            _pool = new Dictionary<int, List<T[]>>();
        }

        /// <summary>
        /// Adds an already allocated array to the StaticArrayPool.
        /// </summary>
        /// <param name="array">The array to add.</param>
        /// <param name="clearArray">Indicates whether the values in the array should be cleared before adding it.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fill(T[] array, bool clearArray = false)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) throw new ArgumentException("The given array has a length of 0 which is not supported by this method.");

            if (_pool == null) InitializeArrayPool();

            if (clearArray) ClearArray(ref array);

            if (!_pool.ContainsKey(array.Length))
            {
                _pool.Add(array.Length, new List<T[]>());
            }

            _pool[array.Length].Add(array);
        }

        /// <summary>
        /// Adds a sequence of arrays to the StaticArrayPool.
        /// </summary>
        /// <param name="sequence">A sequence of arrays.</param>
        /// <param name="clearArray">Indicates whether the values of each array should be cleared before adding it.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FillSequence(IEnumerable<T[]> sequence, bool clearArray = false)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            foreach (var array in sequence)
            {
                Fill(array, clearArray);
            }
        }

        /// <summary>
        /// Retrieves a buffer the requested length.
        /// </summary>
        /// <param name="size">The length of the requested buffer.</param>
        /// <returns>An array from the StaticArrayPool.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Rent(int size)
        {
            if (size < 0) throw new ArgumentOutOfRangeException(nameof(size));

            if (_pool == null) InitializeArrayPool();

            if (!_pool.ContainsKey(size))
            {
                _pool.Add(size, new List<T[]>());
            }

            var list = _pool[size];

            if (list.Count == 0)
            {
                list.Add(new T[size]);
            }

            int listIndex = list.Count - 1;

            var array = list[listIndex];

            list.RemoveAt(listIndex);

            return array;
        }

        /// <summary>
        /// Returns a previously rented array to the StaticArrayPool.
        /// </summary>
        /// <param name="array">The array to return.</param>
        /// <param name="clearArray">Indicates whether the values of the array should be cleared before returning it.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return(T[] array, bool clearArray = false)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) throw new ArgumentException("The given array has a length of 0 which is not supported by this method.");
            if (_pool == null) throw new InvalidOperationException("Only rented arrays can be returned by this method.");

            if (clearArray) ClearArray(ref array);

            if (_pool.ContainsKey(array.Length))
            {
                _pool[array.Length].Add(array);
            }
            else
            {
                throw new InvalidOperationException("The given array was not rented from this pool.");
            }
        }
    }
}
