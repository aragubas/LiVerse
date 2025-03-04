# Setup
cmake_minimum_required(VERSION 3.28)
project("Runtime")


# Find required packages
find_package(fmt CONFIG REQUIRED)
find_package(SDL3 CONFIG REQUIRED)
find_package(SDL3_ttf CONFIG REQUIRED)


# Add source to this project's executable.
add_executable(Runtime
  "main.cpp" "Application.cpp" "Application.h"
  "Views/View.cpp" "Views/View.h" "Views/CharacterView.cpp"
  "Views/CharacterView.h"
)

# Set C++ standard to C++20
set_property(TARGET Runtime PROPERTY CXX_STANDARD 20)
# Treat warnings as errors
set_property(TARGET TaiyouUI PROPERTY COMPILE_WARNING_AS_ERROR ON)

# Link and include libraries
target_link_libraries(Runtime PRIVATE fmt::fmt)
target_link_libraries(Runtime PRIVATE SDL3::SDL3)
# idk how the $<IF: part works, vcpkg told me this would work so I trust it :)
target_link_libraries(TaiyouUI PRIVATE $<IF:$<TARGET_EXISTS:SDL3_ttf::SDL3_ttf>,SDL3_ttf::SDL3_ttf,SDL3_ttf::SDL3_ttf-static>)
target_link_libraries(Runtime PRIVATE TaiyouUI)
