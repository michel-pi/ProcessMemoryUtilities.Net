<div align="center">

# ProcessMemoryUtilities.Net

[![Nuget](https://img.shields.io/nuget/v/ProcessMemoryUtilities.Net.svg)](https://www.nuget.org/packages/ProcessMemoryUtilities.Net/ "ProcessMemoryUtilities.Net on NuGet") [![Nuget](https://img.shields.io/nuget/dt/ProcessMemoryUtilities.Net.svg)](https://www.nuget.org/packages/ProcessMemoryUtilities.Net/ "Downloads on NuGet") [![Open issues](https://img.shields.io/github/issues-raw/michel-pi/ProcessMemoryUtilities.Net.svg)](https://github.com/michel-pi/ProcessMemoryUtilities.Net/issues "Open issues on Github") [![Closed issues](https://img.shields.io/github/issues-closed-raw/michel-pi/ProcessMemoryUtilities.Net.svg)](https://github.com/michel-pi/ProcessMemoryUtilities.Net/issues?q=is%3Aissue+is%3Aclosed "Closed issues on Github") [![MIT License](https://img.shields.io/github/license/michel-pi/ProcessMemoryUtilities.Net.svg)](https://github.com/michel-pi/ProcessMemoryUtilities.Net/blob/master/LICENSE "ProcessMemoryUtilities.Net license")

![Net Framework 4.52](https://img.shields.io/badge/.Net-4.52-informational.svg) ![Net Framework 4.7](https://img.shields.io/badge/.Net-4.7-informational.svg) ![Net Framework 4.72](https://img.shields.io/badge/.Net-4.72-informational.svg) ![Net Standard 2.0](https://img.shields.io/badge/.Net_Standard-2.0-informational.svg)
</div>

This library implements performant ReadProcessMemory and WriteProcessMemory with generic type parameters using InlineIL and also offers methods required to open processes, create remote threads and marshal value types and strings.

All methods are tested and working under 32bit and 64bit windows.

## NuGet

    Install-Package ProcessMemoryUtilities.Net

## Features

All methods are implemented in a way to be a drop in replacement for their `kernel32` equivalents even when they are implemented using their `ntdll` equivalent.

Enums used by these methods are definied in a correct way and provide XML documentation.

- CloseHandle
- CreateRemoteThreadEx
- OpenProcess
- Generic ReadProcessMemory
- VirtualAllocEx
- VirtualFreeEx
- VirtualProtectEx
- WaitForSingleObject
- Generic WriteProcessMemory

Every native method is implemented using the `calli` IL instruction and bypasses type limitations introduced in C#.

Some important improvements are:

- Direct calls to WinAPI methods
- Using `ntdll` methods instead of `kernel32` whenever possible
- No performance loss due to marshalling or delegates
- Optimized memory allocation

## Marshalling

The `UnsafeMarshal` class provides methods to dereference pointers, get the size of a value type and gerneric replacements methods for the methods conatined in the `Bitconverter` class.

The `StringMarshal` class not only converts strings and byte arrays but also handles the null bytes typically contained at the end of an unmanaged string.

## Methods

Here are some method signatures to provide a quick overview on what is actually possible when using this library.

```cs
// CreateRemoteThreadEx with a reduced set of parameters for easier usage
IntPtr CreateRemoteThreadEx(IntPtr handle, IntPtr startAddress);
// compared to this one which is also available
IntPtr CreateRemoteThreadEx(IntPtr handle, IntPtr threadAttributes, IntPtr stackSize, IntPtr startAddress, IntPtr parameter, ThreadCreationFlags creationFlags, IntPtr attributeList, ref uint threadId);

// ReadProcessMemory
bool ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, ref T buffer);
bool ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer);

// Additional methods
IntPtr VirtualAllocEx(IntPtr handle, IntPtr baseAddress, IntPtr size, AllocationType allocationType, MemoryProtectionFlags memoryProtection);

bool VirtualFreeEx(IntPtr handle, IntPtr address, IntPtr size, FreeType freeType);
bool VirtualProtectEx(IntPtr handle, IntPtr address, IntPtr size, MemoryProtectionFlags newProtect, ref MemoryProtectionFlags oldProtect);

WaitObjectResult WaitForSingleObject(IntPtr handle, uint timeout);

// WriteProcessMemory
bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T buffer)
public static bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer)
```

### [Documentation](https://michel-pi.github.io/ProcessMemoryUtilities.Net/ "ProcessMemoryUtilities.Net Documentation")

### Error Handling

Because the ReadProcessMemory and WriteProcessMemory methods are implemented using their `ntdll` equivalents they do not provide error codes using `GetLastError`.

However, their behaviour is simple.

Whenever one of these two methods return false you can check if their `numberOfBytesRead` or `numberOfBytesWritten` is

`0` which means that one of the following errors occured:

- The given process handle is invalid.
- The target process is currently terminating.
- The requested memory was not accessible through it's protection flags.
- The requested operation tried to query kernel memory.
- The requested operation crossed a memory boundary from usermode to kernel mode memory.

`not 0` which means that only a part of the data has been queried successfully. You can try to query it again using the same parameters or by only requesting the remaining bytes.

## Contribute

The project file was generated using Visual Studio 2019.

Clone or download the repository and update/install the required NuGet packages.

You can help by reporting issues, adding new features, fixing bugs and by providing a better documentation.  

### Dependencies

Following dependencies are used to build the project but are **NOT** included in the NuGet package.

    Fody, InlineIL.Fody

## Donate

Do you like this project and want to help me to keep working on it?

Then maybe consider to donate any amount you like.

[![Donate via PayPal](https://media.wtf/assets/img/pp.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=YJDWMDUSM8KKQ "Donate via PayPal")

```
BTC     14ES7f4GB3vD1C8Faz6ywqTcdDevxZoMyY

ETH     0xd9E2CB12d310E7BF5E72F591D7A2b8820adced04
```

## License

- [ProcessMemoryUtilities.Net License](https://github.com/michel-pi/ProcessMemoryUtilities.Net/blob/master/LICENSE "ProcessMemoryUtilities.Net License")
- [Fody License](https://github.com/Fody/Fody/blob/master/License.txt "Fody License")
- [InlineIL.Fody License](https://github.com/ltrzesniewski/InlineIL.Fody/blob/master/LICENSE "InlineIL.Fody License")
