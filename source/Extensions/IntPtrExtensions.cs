using System;
using System.Runtime.CompilerServices;

namespace ProcessMemoryUtilities.Extensions
{
    public static class IntPtrExtensions
    {
        private static readonly bool _x86 = IntPtr.Size == 4;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAligned(this IntPtr ptr)
        {
            var value = ptr.GetValue();

            return value == 1 || value % 1 == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong GetValue(this IntPtr ptr)
        {
            if (_x86)
            {
                return (uint)ptr;
            }
            else
            {
                return (ulong)ptr;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(this IntPtr ptr)
        {
            return ptr == IntPtr.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotZero(this IntPtr ptr)
        {
            return ptr != IntPtr.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(this IntPtr ptr)
        {
            if (_x86)
            {
                uint value = (uint)ptr;

                return (value > 0x10000u && value < 0xFFF00000u);
            }
            else
            {
                ulong value = (ulong)ptr;

                return (value > 0x10000u && value < 0x000F000000000000u);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Add(this IntPtr left, IntPtr right)
        {
            if (_x86)
            {
                return new IntPtr((uint)left + (uint)right);
            }
            else
            {
                ulong result = (ulong)left + (ulong)right;

                return new IntPtr((long)result);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Subtract(this IntPtr left, IntPtr right)
        {
            if (_x86)
            {
                return new IntPtr((uint)left - (uint)right);
            }
            else
            {
                ulong result = (ulong)left - (ulong)right;

                return new IntPtr((long)result);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Multiply(this IntPtr left, IntPtr right)
        {
            if (_x86)
            {
                return new IntPtr((uint)left * (uint)right);
            }
            else
            {
                ulong result = (ulong)left * (ulong)right;

                return new IntPtr((long)result);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Divide(this IntPtr left, IntPtr right)
        {
            if (_x86)
            {
                return new IntPtr((uint)left / (uint)right);
            }
            else
            {
                ulong result = (ulong)left / (ulong)right;

                return new IntPtr((long)result);
            }
        }
    }
}
