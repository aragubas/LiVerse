# Setup
cmake_minimum_required(VERSION 3.28)
project("Runtime")


# Find required packages
find_package(fmt CONFIG REQUIRED)
find_package(SDL2 CONFIG REQUIRED)
find_package(SDL2_ttf CONFIG REQUIRED)


# Add source to this project's executable.
add_executable(Runtime
  "main.cpp" "Application.cpp" "Application.h"
)

# Set C++ standard to C++20
set_property(TARGET Runtime PROPERTY CXX_STANDARD 20)
# Treat warnings as errors
set_property(TARGET TaiyouUI PROPERTY COMPILE_WARNING_AS_ERROR ON)

# Link and include libraries
target_link_libraries(Runtime PRIVATE fmt::fmt)
target_link_libraries(Runtime PRIVATE SDL2::SDL2)
# idk how the $<IF: part works, vcpkg told me this would work so I trust it :)
target_link_libraries(TaiyouUI PRIVATE $<IF:$<TARGET_EXISTS:SDL2_ttf::SDL2_ttf>,SDL2_ttf::SDL2_ttf,SDL2_ttf::SDL2_ttf-static>)
target_link_libraries(Runtime PRIVATE TaiyouUI)
