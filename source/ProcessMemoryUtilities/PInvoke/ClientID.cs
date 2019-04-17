using System;
using System.Runtime.InteropServices;

namespace ProcessMemoryUtilities.PInvoke
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ClientID
    {
        public IntPtr UniqueProcess;
        public IntPtr UniqueThread;
    }
}
