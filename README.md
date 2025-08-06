# LiVerse++
A complete re-write of LiVerse and TaiyouUI in C++ using SDL2

> [!IMPORTANT]
> This project has been moved to Gitlab. 
> It is now under this URL: https://gitlab.com/aragubas1/LiVerse


LIVerse is a Hackable and Extensible PNGTuber app for Linux, macOS and Windows. Made by Aragubas


Powered by TaiyouUI and SDL2

# Build Instructions
To get started, rename CMakePresets.json.example to CMakePresets.json, and replace <VCPKG_PATH>
to where VCPKG is installed. And make sure VCPKG_ROOT is set in your path to the root of your vcpkg installation. Make sure you have installed all the build dependencies below before trying to run CMake

## CMake
First, make sure you have ``VCPKG_ROOT`` set in your path to the root of your vcpkg installation

```
# Debug Configuration
cmake --preset vcpkg-debug -B ./build/

# Release Configuration
cmake --preset vcpkg-release -B ./build/

# Building
cmake --build ./build
```

The following build presets are available:
```
vcpkg-debug, vcpkg-debug-asan, vcpkg-release, vcpkg-release-with-debug
```

## Build Dependencies

### Ubuntu/Debian:
```
build-essential cmake ninja-build pkg-config autoconf libtool libsdl2-dev
```

### Fedora
```
cmake ninja-build autoconf automake libtool perl-open perl-FindBin python3-jinja2 ibus-devel libXext-devel
```

### Arch Linux/endeavourOS
```
cmake ninja python-jinja
```

### macOS (Homebrew)
```
cmake ninja pkg-config
```

# License
This project is licensed under the AGPL 3.0 license

Check [License](./LICENSE) for more details

(C) 2022 - 2024 by Aragubas - part of Taiyou Software Suite
