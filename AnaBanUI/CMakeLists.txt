# Find required packages
find_package(fmt CONFIG REQUIRED)
find_package(SFML COMPONENTS system window graphics CONFIG REQUIRED)

add_library(AnaBanUI STATIC "AnaBanUI/UIRoot.h" "AnaBanUI/UIRoot.cpp" "AnaBanUI/Control.h" "AnaBanUI/Container.h")

if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET AnaBanUI PROPERTY CXX_STANDARD 20)
endif()

target_link_libraries(AnaBanUI PRIVATE fmt::fmt)
target_link_libraries(AnaBanUI PRIVATE sfml-system sfml-graphics sfml-window)