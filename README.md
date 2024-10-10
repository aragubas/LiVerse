# LiVerse++
A complete re-write of LiVerse and TaiyouUI in C++ using SDL2

LIVerse is a Hackable and Extensible PNGTuber app for Linux, macOS and Windows


Powered by TaiyouUI and SDL2 

# Build Instructions

## Build Instructions for Visual Studio on Windows
To get started, rename CMakePresets.json.example to CMakePresets.json, and replace <VCPKG_PATH>
to where VCPKG is installed

## Build Instructions for Linux
### Ubuntu/Debian:
Build Dependencies
```
build-essential cmake ninja-build pkg-config autoconf libtool libsdl2-dev
```

### Fedora
Build Dependencies
```
cmake ninja-build autoconf automake libtool perl-open perl-FindBin python3-jinja2 ibus-devel libXext-devel
```


Before running ./build.sh, create a file '.vcpkg_path' in the root directory of the project.
.vcpkg_path must contain path to vcpkg, ending with /


## Build Instructions for macOS Environment
Build Dependencies (install via homebrew)
```
cmake ninja pkg-config
```

Before running ./build.sh, create a file called '.vcpkg_path' in the root of the project.
.vcpkg_path must contain path to vcpkg, ending with /


# License
This project is licensed under the AGPL 3.0 license

Check [License](./LICENSE) for more details

(C) 2022 - 2024 by Aragubas - part of Taiyou Software Suite
