using System;
using System.Runtime.InteropServices;

namespace ProcessMemoryWrapper
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct ClientID
    {
        public void* UniqueProcess;
        public void* UniqueThread;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct ObjectAttributes
    {
        public int Length;
        public void* RootDirectory;
        public void* ObjectName;
        public int Attributes;
        public void* SecurityDescriptor;
        public void* SecurityQualityOfService;
    }
}
