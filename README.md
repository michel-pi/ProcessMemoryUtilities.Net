<div align="center">

# ProcessMemoryUtilities.Net

[![Nuget](https://img.shields.io/nuget/v/ProcessMemoryUtilities.Net.svg)](https://www.nuget.org/packages/ProcessMemoryUtilities.Net/ "ProcessMemoryUtilities.Net on NuGet") [![Nuget](https://img.shields.io/nuget/dt/ProcessMemoryUtilities.Net.svg)](https://www.nuget.org/packages/ProcessMemoryUtilities.Net/ "Downloads on NuGet") [![Open issues](https://img.shields.io/github/issues-raw/michel-pi/ProcessMemoryUtilities.Net.svg)](https://github.com/michel-pi/ProcessMemoryUtilities.Net/issues "Open issues on Github") [![Closed issues](https://img.shields.io/github/issues-closed-raw/michel-pi/ProcessMemoryUtilities.Net.svg)](https://github.com/michel-pi/ProcessMemoryUtilities.Net/issues?q=is%3Aissue+is%3Aclosed "Closed issues on Github") [![MIT License](https://img.shields.io/github/license/michel-pi/ProcessMemoryUtilities.Net.svg)](https://github.com/michel-pi/ProcessMemoryUtilities.Net/blob/master/LICENSE "ProcessMemoryUtilities.Net license")

![Net Framework 4.52](https://img.shields.io/badge/.Net-4.52-informational.svg) ![Net Framework 4.7](https://img.shields.io/badge/.Net-4.7-informational.svg) ![Net Framework 4.8](https://img.shields.io/badge/.Net-4.8-informational.svg) ![Net Standard 2.0](https://img.shields.io/badge/.Net_Standard-2.0-informational.svg)
</div>

This library implements performant wrapper methods over, in game hacking, commonly used `NtDll` and `Kernel32` functions. The different classes allow you to use generic type parameters with `ReadProcessMemory` and `WriteProcessMemory` and call simpler functions like `OpenProcess`, `CreateRemoteThread` and more without any overhead.

While most of the methods are implemented using their `NtDll` equivalent instead of `Kernel32` some still require `Kernel32` to work properly or are depricated in `NtDll`.

All methods are tested and working under 32bit and 64bit windows and are well documented.

[Documentation](https://michel-pi.github.io/ProcessMemoryUtilities.Net/ "ProcessMemoryUtilities.Net Documentation")

## NuGet

I am currently recovering my NuGet account for which i lost the 2-factor-authentication. You can get the current version from the [Github Package Registry](https://github.com/michel-pi/ProcessMemoryUtilities.Net/packages/20404 "ProcessMemoryUtilities.Net Github Package").

    Install-Package ProcessMemoryUtilities.Net

This library is also available in the [Github Package Registry](https://github.com/michel-pi/ProcessMemoryUtilities.Net/packages/20404 "ProcessMemoryUtilities.Net Github Package").

## Features

The `ProcessMemoryUtilities.Native` namespace offers direct access to either `Kernel32` or `NtDll` methods without any overhead. Most of them not only offer the traditional function signature but also implement overloads with common default values set.

All the required enums and constants are available with their XML documentation.

The `ProcessMemoryUtilities.Managed` namespace offers the `NativeWrapper` class which is there to provide a single place to access all the implemented methods with a more user friendly and `Kernel32` like interface. This also adds basic error handling over `ReadProcessMemory` and `WriteProcessMemory` and offers a `Win32` error code when any function failed.

- CloseHandle
- CreateRemoteThread(Ex)
- Generic ReadProcessMemory
- Generic WriteProcessMemory
- OpenProcess
- VirtualAllocEx
- VirtualFreeEx
- VirtualProtectEx
- WaitForSingleObject

Every native method is implemented using the `calli` IL instruction and bypasses type limitations introduced in C#. All native calls are done in a safe manner and use correct types and pinning for pointer variables.

Some important improvements are:

- Direct calls to WinAPI methods
- Using `NtDll` methods instead of `Kernel32` whenever possible
- No performance loss due to marshaling or delegates
- Optimized memory allocations

## Methods

I copied some of the function signatures to give you a quick overview of what kind of methods you can expect from this library.

```cs
// CreateRemoteThreadEx with a reduced set of parameters for easier usage
IntPtr CreateRemoteThreadEx(IntPtr handle, IntPtr startAddress, IntPtr parameter);
// compared to this one which is also available
IntPtr CreateRemoteThreadEx(IntPtr handle, IntPtr threadAttributes, IntPtr stackSize, IntPtr startAddress, IntPtr parameter, ThreadCreationFlags creationFlags, IntPtr attributeList, out uint threadId);

// OpenProcess
IntPtr OpenProcess(ProcessAccessFlags desiredAccess, bool inheritHandle, int processId);
IntPtr OpenProcess(ProcessAccessFlags desiredAccess, int processId);

// WaitForSingleObject
WaitObjectResult WaitForSingleObject(IntPtr handle, uint timeout);

// ReadProcessMemory and WriteProcessMemory
uint NtReadVirtualMemory<T>(IntPtr handle, IntPtr baseAddress, ref T buffer, out IntPtr numberOfBytesRead);

bool WriteProcessMemoryArray<T>(IntPtr handle, IntPtr baseAddress, T[] buffer, int offset, out IntPtr numberOfBytesWritten);

// VirtualMemory functions
uint NtAllocateVirtualMemory(IntPtr handle, IntPtr size, AllocationType allocationType, MemoryProtectionFlags memoryProtection, out IntPtr address);

IntPtr VirtualAllocEx(IntPtr handle, IntPtr address, IntPtr size, AllocationType allocationType, MemoryProtectionFlags memoryProtection);
bool VirtualFreeEx(IntPtr handle, IntPtr address, IntPtr size, FreeType freeType);
bool VirtualProtectEx(IntPtr handle, IntPtr address, IntPtr size, MemoryProtectionFlags newProtect, out MemoryProtectionFlags oldProtect);
```

## Marshaling

While this approach offers a wide range of benefits you may encounter a single drawback.

Because we use the IL instruction `sizeof` instead of `Marshal.SizeOf` the whole marshaling layer is skipped. This means that you can not use classes and the following keywords inside of structs

```cs
[MarshalAs]
string
```

Please use `fixed` arrays instead of `[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]`

```cs
// This does not work with my library because sizeof gives us a wrong size (4 instead of 16)
[StructLayout(LayoutKind.Explicit)]
private struct Wrong
{
    [FieldOffset(0)]
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public int[] Numbers;

    [FieldOffset(0)]
    public int FirstNumber;
}

// [StructLayout(LayoutKind.Sequential)] is also a valid option
[StructLayout(LayoutKind.Explicit)]
private unsafe struct Correct
{
    [FieldOffset(0)]
    public fixed int Numbers[4];

    [FieldOffset(0)]
    public int FirstNumber;
}
```

### Error Handling

The `ProcessMemoryUtilities.NativeWrapper` class offers the `CaptureErrors` property (which is set true by default) to emulate `SetLastError` and `GetLastError`.

The `LastError` property converts the saved `NtStatus` to a equivalent `Win32` error code which you can use in your exceptions.

### [Documentation](https://michel-pi.github.io/ProcessMemoryUtilities.Net/ "ProcessMemoryUtilities.Net Documentation")

## Contribute

The project file was generated using Visual Studio 2019.

Clone or download the repository and restore the required NuGet packages.

You can help by reporting issues, adding new features, fixing bugs and by providing a better documentation.

### Dependencies

Following dependencies are used to build the project but are **NOT** included in the NuGet package.

    Fody
    InlineIL.Fody
    ILRepack.Lib.MSBuild.Task

## Donate

Do you like this project and want to help me to keep working on it?

I appreciate any donation that helps me to continue working on OSS like this.

[![Donate via PayPal](https://media.wtf/assets/img/pp.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=YJDWMDUSM8KKQ "Donate via PayPal")

```
BTC     14ES7f4GB3vD1C8Faz6ywqTcdDevxZoMyY

ETH     0xd9E2CB12d310E7BF5E72F591D7A2b8820adced04
```

## License

- [ProcessMemoryUtilities.Net License](https://github.com/michel-pi/ProcessMemoryUtilities.Net/blob/master/LICENSE "ProcessMemoryUtilities.Net License")
- [Fody License](https://github.com/Fody/Fody/blob/master/License.txt "Fody License")
- [InlineIL.Fody License](https://github.com/ltrzesniewski/InlineIL.Fody/blob/master/LICENSE "InlineIL.Fody License")
- [ILRepack.Lib.MSBuild.Task
 License](https://github.com/ravibpatel/ILRepack.Lib.MSBuild.Task/blob/master/LICENSE.md "ILRepack.Lib.MSBuild.Task License")
- [BenchmarkDotNet License](https://github.com/dotnet/BenchmarkDotNet "BenchmarkDotNet License")
