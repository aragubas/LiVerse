# Find required packages
find_package(fmt CONFIG REQUIRED)
find_package(SDL2 COMPONENTS SDL2-static CONFIG REQUIRED)

# Add source to this project's executable.
add_executable (Runtime "Runtime.cpp"  "Application.cpp" "Application.h")

if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET Runtime PROPERTY CXX_STANDARD 20)
endif()

target_link_libraries(Runtime PRIVATE fmt::fmt)
target_link_libraries(Runtime PRIVATE SDL2::SDL2-static)
target_link_libraries(Runtime PUBLIC AnaBanUI)
target_include_directories(Runtime PUBLIC ${PROJECT_BINARY_DIR} ${PROJECT_SOURCE_DIR}/AnaBanUI)