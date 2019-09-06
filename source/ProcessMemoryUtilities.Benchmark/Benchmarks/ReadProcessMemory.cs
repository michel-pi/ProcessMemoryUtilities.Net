using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

using BenchmarkDotNet.Attributes;

namespace ProcessMemoryUtilities.Benchmark.Benchmarks
{
    public class ReadProcessMemory
    {
        private readonly Process _process;
        private readonly IntPtr _handle;
        private readonly IntPtr _moduleBaseAddress;

        private readonly byte[] _buffer;

        public ReadProcessMemory()
        {
            string path;

            if (IntPtr.Size == 8)
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"System32\cmd.exe");
            }
            else if (Environment.Is64BitOperatingSystem)
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"SysWOW64\cmd.exe");
            }
            else
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"System32\cmd.exe");
            }

            _process = Process.Start(path);

            while (true)
            {
                try
                {
                    if (_process.MainWindowHandle != IntPtr.Zero)
                    {
                        break;
                    }
                }
                catch { }

                Thread.Sleep(10);

                _process.Refresh();
            }

            _handle = _process.Handle;
            _moduleBaseAddress = _process.MainModule.BaseAddress;

            _buffer = new byte[4096];
        }

        ~ReadProcessMemory()
        {
            _process.Kill();
        }

        [Benchmark]
        public uint RPM()
        {
            return Native.NtDll.NtReadVirtualMemoryArray(_handle, _moduleBaseAddress, _buffer);
        }
    }
}
