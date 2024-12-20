# LiVerse++
A complete re-write of LiVerse and TaiyouUI in C++ using SDL2

LIVerse is a Hackable and Extensible PNGTuber app for Linux, macOS and Windows


Powered by TaiyouUI and SDL2

# Build Instructions
To get started, rename CMakePresets.json.example to CMakePresets.json, and replace <VCPKG_PATH>
to where VCPKG is installed

## Build Instructions for Visual Studio on Windows
*Coming soon*

## Build Instructions for Linux
### Dependencies for Ubuntu/Debian:
```
build-essential cmake ninja-build pkg-config autoconf libtool libsdl2-dev
```

### Dependencies for Fedora
```
cmake ninja-build autoconf automake libtool perl-open perl-FindBin python3-jinja2 ibus-devel libXext-devel
```

#### Building using the build script 
---
Before running ./build.sh, create a file '.vcpkg_path' in the root directory of the project.
.vcpkg_path must contain path to vcpkg, ending with /

#### Compiling manually through CMake
``cd`` into the project and run
```
cmake -DCMAKE_BUILD_TYPE=Debug -B ./build/
```
to configure, and then
```
cmake --preset vcpkg-debug -DCMAKE_OUTPUT_TYPE=Debug -B ./build
```
to build

## Build Instructions for macOS
Build Dependencies (install via homebrew)
```
cmake ninja pkg-config
```

You can use the same build script as the one for Linux, the instructions are the same as above

# License
This project is licensed under the AGPL 3.0 license

Check [License](./LICENSE) for more details

(C) 2022 - 2024 by Aragubas - part of Taiyou Software Suite
