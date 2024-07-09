#!/bin/bash
vcpkg_path=undefined

#
#   Check if .vcpkg_path file exists
#
if [ -f .vcpkg_path ]; then
    vcpkg_path="$(cat .vcpkg_path | tr -d '\n')"
else
    echo E: Could not find .vcpkg_path
    echo Make sure to create .vcpkg_path with path to your vcpkg installation
    exit 1
fi

echo vcpkg path set to: \"$vcpkg_path\"
#
#   Check if path specified in vcpkg_path exists
#
if [ ! -d $vcpkg_path ]; then
    echo E: Path specified in .vcpkg_path doesn\'t exist
    echo Please check if there\'s any typo or if the path specified in .vcpkg_path is valid.
    exit 1
fi

#
#   Check if vcpkg executable inside vcpkg_path exists
#
vcpkg_executable="$vcpkg_path"vcpkg
if [ ! -f $vcpkg_executable ]; then
    echo E: Could not find vcpkg executable inside vcpkg_path
    echo Make sure vcpkg_path ends with slash or if it\'s pointing to the correct path
    echo tried $vcpkg_executable
    exit 1
fi

# 
#   Get current directory
#
current_path=$(pwd)

#
#   Run CMake
#
cmake -DCMAKE_TOOLCHAIN_FILE="$vcpkg_path"/scripts/buildsystems/vcpkg.cmake -DCMAKE_BUILD_TYPE=Debug -S"$current_path" -B"$current_path"/build -G Ninja && cmake --build "$current_path"/build --parallel 6