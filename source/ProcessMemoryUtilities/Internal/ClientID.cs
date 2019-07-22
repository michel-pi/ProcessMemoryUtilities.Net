using System;
using System.Runtime.InteropServices;

namespace ProcessMemoryUtilities.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ClientID
    {
        public IntPtr UniqueProcess;
        public IntPtr UniqueThread;
    }
}
