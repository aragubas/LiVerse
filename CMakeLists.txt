# Setup
cmake_minimum_required(VERSION 3.28)
project("LiVerse++")


# Enable Hot Reload for MSVC compilers, if supported.
if (POLICY CMP0141)
  cmake_policy(SET CMP0141 NEW)
  set(CMAKE_MSVC_DEBUG_INFORMATION_FORMAT "$<IF:$<AND:$<C_COMPILER_ID:MSVC>,$<CXX_COMPILER_ID:MSVC>>,$<$<CONFIG:Debug,RelWithDebInfo>:EditAndContinue>,$<$<CONFIG:Debug,RelWithDebInfo>:ProgramDatabase>>")
endif()


# Set properties
set(CMAKE_CXX_STANDARD 23)

# Include sub-projects.
add_subdirectory("TaiyouUI")
add_subdirectory("Runtime")
