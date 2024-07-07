# Find required packages
find_package(fmt CONFIG REQUIRED)
find_package(SDL2 COMPONENTS SDL2-static CONFIG REQUIRED)

add_library(AnaBanUI STATIC 
  "AnaBanUI/UIRoot.h" "AnaBanUI/UIRoot.cpp" "AnaBanUI/Control.h" "AnaBanUI/Container.h"
  "AnaBanUI/Layer.h" "AnaBanUI/Layer.cpp" "AnaBanUI/Controls/Button.h" "AnaBanUI/Controls/Button.cpp"
  "AnaBanUI/Containers/DockFill.cpp" "AnaBanUI/Containers/DockFill.h"
)

if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET AnaBanUI PROPERTY CXX_STANDARD 20)
endif()

target_link_libraries(AnaBanUI PRIVATE fmt::fmt)
target_link_libraries(AnaBanUI PRIVATE SDL2::SDL2-static)