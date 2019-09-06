using System;
using System.IO;
using BenchmarkDotNet.Running;

using ProcessMemoryUtilities.Benchmark.Benchmarks;

namespace ProcessMemoryUtilities.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ReadProcessMemory>();
        }
    }
}
