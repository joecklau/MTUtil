# MT Utilities
Add extra support to MQL4 EA

## Use of DllExport
DllExport is used to upgrade csproj file(s) in a sln to build dll library that can be invoked by MQL4 import like C++ Extern APIs function
1. Nuget Install DllExport, which will get the required files and DllExport.bat
2. Double-click DllExport.bat, select the selected csproj to upgrade
3. Change the project CPU from AnyCPU to x86
4. Build the dll and copy to same directory of the MQL script location

p.s. Remember the DllExport.bat must be placed at same location as the sln file, not csproj.
p.s. even dotPeek cannot see the Extern APIs made available from the dll built. Use `DllExport.bat -pe-ext-list (dllPath)` to check the Extern APIs made available.
p.s. To make it invokable in MQL4, don't add any Nuget reference other than DllExport (unless you know how to make it works like real MtApi/MTConnector)