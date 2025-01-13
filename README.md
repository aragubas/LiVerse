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

### Dependencies for Arch Linux/endeavourOS
```
cmake ninja python-jinja
```

#### Building using the build script 
---
Before running ./build.sh, create a file '.vcpkg_path' in the root directory of the project.
.vcpkg_path must contain path to vcpkg, ending with /

#### Using CMake directly
---
First, make sure you have ``VCPKG_ROOT`` set in your path to the root of your vcpkg installation

```
# Configuration
cmake --preset vcpkg-debug -B ./build/

# Building
cmake --build ./build
```

The following presets are available:
```
vcpkg-debug, vcpkg-release, vcpkg-release-with-debug
```

# License
This project is licensed under the AGPL 3.0 license

Check [License](./LICENSE) for more details

(C) 2022 - 2024 by Aragubas - part of Taiyou Software Suite
