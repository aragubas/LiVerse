# Setup
cmake_minimum_required(VERSION 3.28)
project("Runtime")


# Find required packages
find_package(fmt CONFIG REQUIRED)
find_package(SDL2 COMPONENTS SDL2-static CONFIG REQUIRED)


# Add source to this project's executable.
add_executable(Runtime
  "Runtime.cpp" "Application.cpp" "Application.h"
)

# Set C++ standard to C++20
set_property(TARGET Runtime PROPERTY CXX_STANDARD 20)

# Link and include libraries
target_link_libraries(Runtime PRIVATE fmt::fmt)
target_link_libraries(Runtime PRIVATE SDL2::SDL2-static)
target_link_libraries(Runtime PRIVATE TaiyouUI)