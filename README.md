# ProcessMemoryWrapper.Net

[![Nuget](https://img.shields.io/nuget/v/ProcessMemoryWrapper.Net.svg)](https://www.nuget.org/packages/ProcessMemoryWrapper.Net/ "ProcessMemoryWrapper.Net on NuGet") [![Nuget](https://img.shields.io/nuget/dt/ProcessMemoryWrapper.Net.svg)](https://www.nuget.org/packages/ProcessMemoryWrapper.Net/ "Downloads on NuGet") [![Open issues](https://img.shields.io/github/issues-raw/michel-pi/ProcessMemoryWrapper.Net.svg)](https://github.com/michel-pi/ProcessMemoryWrapper.Net/issues "Open issues on Github") [![Closed issues](https://img.shields.io/github/issues-closed-raw/michel-pi/ProcessMemoryWrapper.Net.svg)](https://github.com/michel-pi/ProcessMemoryWrapper.Net/issues?q=is%3Aissue+is%3Aclosed "Closed issues on Github") [![MIT License](https://img.shields.io/github/license/michel-pi/ProcessMemoryWrapper.Net.svg)](https://github.com/michel-pi/ProcessMemoryWrapper.Net/blob/master/LICENSE "ProcessMemoryWrapper.Net license")

Implements performant Read- and WriteProcessMemory using InlineIL.
Only .Net Framework 4.7 or higher is currently supported.

## NuGet

    Install-Package ProcessMemoryWrapper.Net

## Features

- Open- and CloseProcess
- Generic Read- and WriteProcessMemory
- Read and Write arrays
- Direct calls to WinApi methods
- Using Nt* methods instead of Kernel32
- No performance loss due to marshalling or delegates
- Optimized memory allocation

## Methods

```cs
// Open and Close handles
bool CloseProcess(IntPtr handle);
IntPtr OpenProcess(ProcessAccessFlags desiredAccess, int processId);

// ReadProcessMemory
bool ReadProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, IntPtr size);
bool ReadProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, IntPtr size, ref IntPtr numberOfBytesRead);

byte[] ReadProcessMemory(IntPtr handle, IntPtr baseAddress, int size);
byte[] ReadProcessMemory(IntPtr handle, IntPtr baseAddress, int size, ref IntPtr numberOfBytesRead);

T ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress);
T ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, ref IntPtr numberOfBytesRead);

T[] ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, int size);
T[] ReadProcessMemory<T>(IntPtr handle, IntPtr baseAddress, int size, ref IntPtr numberOfBytesRead);

// WriteProcessMemory
bool WriteProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, IntPtr size);
bool WriteProcessMemory(IntPtr handle, IntPtr baseAddress, IntPtr buffer, IntPtr size, ref IntPtr numberOfBytesWritten);

bool WriteProcessMemory(IntPtr handle, IntPtr baseAddress, byte[] buffer);
bool WriteProcessMemory(IntPtr handle, IntPtr baseAddress, byte[] buffer, ref IntPtr numberOfBytesWritten);

bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T buffer);
bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T buffer, ref IntPtr numberOfBytesWritten);

bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer);
bool WriteProcessMemory<T>(IntPtr handle, IntPtr baseAddress, T[] buffer, ref IntPtr numberOfBytesWritten);
```

### [Documentation](https://michel-pi.github.io/ProcessMemoryWrapper.Net/ "ProcessMemoryWrapper.Net Documentation")

## Contribute

The project file was generated using Visual Studio 2017.

Clone or download the repository and update/install the required NuGet packages.

You can help by reporting issues, adding new features, fixing bugs and by providing a better documentation.  

### Dependencies

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

- [ProcessMemoryWrapper.Net License](https://github.com/michel-pi/ProcessMemoryWrapper.Net/blob/master/LICENSE "ProcessMemoryWrapper.Net License")
- [InlineIL.Fody License](https://github.com/ltrzesniewski/InlineIL.Fody/blob/master/LICENSE "InlineIL.Fody")
