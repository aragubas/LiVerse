# LiVerse++
A complete re-write of LiVerse and TaiyouUI in C++ using SDL2

# Build Instructions

## Build Instructions for Visual Studio + Windows Environment
To get started, rename CMakePresets.json.example to CMakePresets.json, and replace <VCPKG_PATH>
to where VCPKG is installed

## Build Instructions for Linux Environment
### Ubuntu/Debian:
Build Dependencies
- build-essential
- cmake
- ninja-build
- pkg-config
- autoconf
- libtool
- libsdl2-dev

Before running ./build_linux.sh, create a file called '.vcpkg_path' in the root of the project.
.vcpkg_path must contain path to vcpkg, ending with /


## Build Instructions for macOS Environment
Build Dependencies (install via homebrew)
- cmake
- ninja
- pkg-config
- gdb (Optional, for Debugging, will require code signing)

Before running ./build_linux.sh, create a file called '.vcpkg_path' in the root of the project.
.vcpkg_path must contain path to vcpkg, ending with /


# License
This project is licensed under the Apache 2.0 License. 

Check [License](./LICENSE) for more details

(C) 2022 - 2024 by Aragubas - part of Taiyou Software Suite