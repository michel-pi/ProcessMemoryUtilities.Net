using System;
using System.Runtime.CompilerServices;

using ProcessMemoryUtilities.Native;

namespace ProcessMemoryUtilities.Managed
{
    public static partial class NativeWrapper
    {
        /// <summary>
        /// A constant used to specify an infinite waiting period
        /// </summary>
        public const uint INFINITE = uint.MaxValue;

        /// <summary>
        /// Waits until the specified object is in the signaled state or the time-out interval elapses.
        /// </summary>
        /// <param name="handle">A handle to the object.</param>
        /// <param name="timeout">The time-out interval, in milliseconds. If a nonzero value is specified, the function waits until the object is signaled or the interval elapses. If dwMilliseconds is zero, the function does not enter a wait state if the object is not signaled; it always returns immediately. If dwMilliseconds is INFINITE, the function will return only when the object is signaled.</param>
        /// <returns>If the function succeeds, the return value indicates the event that caused the function to return.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WaitObjectResult WaitForSingleObject(IntPtr handle, uint timeout)
            => Kernel32.WaitForSingleObject(handle, timeout);

        /// <summary>
        /// Turns the given WaitObjectResult into one of the defined enum values by stripping the objects index.
        /// </summary>
        /// <param name="value">A WaitObjectResult.</param>
        /// <returns>A WaitObjectResult which is guaranteed to be one of the defined enum values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WaitObjectResult GetRealWaitObjectResult(WaitObjectResult value)
            => Kernel32.GetRealWaitObjectResult(value);

        /// <summary>
        /// Turns the given WaitObjectResult into one of the defined enum values and returns the objects index.
        /// </summary>
        /// <param name="value">A WaitObjectResult</param>
        /// <param name="index">A variable that receives the index of the awaited object.</param>
        /// <returns>A WaitObjectResult which is guaranteed to be one of the defined enum values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WaitObjectResult GetRealWaitObjectResult(WaitObjectResult value, out int index)
            => Kernel32.GetRealWaitObjectResult(value, out index);
    }
}
