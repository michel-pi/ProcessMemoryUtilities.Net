using System;
using System.Runtime.InteropServices;

namespace ProcessMemoryUtilities.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ObjectAttributes
    {
        public int Length;
        public IntPtr RootDirectory;
        public IntPtr ObjectName;
        public int Attributes;
        public IntPtr SecurityDescriptor;
        public IntPtr SecurityQualityOfService;
    }
}
