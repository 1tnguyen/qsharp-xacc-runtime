project(qsharp-xacc-runtime VERSION 1.0.0 LANGUAGES CXX)
cmake_minimum_required(VERSION 3.2.0 FATAL_ERROR)
include(${CMAKE_ROOT}/Modules/ExternalProject.cmake)

set(CMAKE_STANDARD_REQUIRED ON)
set(CMAKE_CXX_STANDARD 14)
message(STATUS "C++ version ${CXX_STANDARD} configured.")
# Export Cpp compile command
set(CMAKE_EXPORT_COMPILE_COMMANDS TRUE)

# If XACC install directory is not provided,
# then clone, build, and install XACC.
if (NOT XACC_DIR)
ExternalProject_Add(
    XACC
    GIT_REPOSITORY  https://github.com/eclipse/xacc.git
    GIT_TAG         "master"
    SOURCE_DIR      "${CMAKE_SOURCE_DIR}/tpls/xacc"
    CMAKE_ARGS -DCMAKE_INSTALL_PREFIX=${HOME}/.xacc
)
# Set the install directory accordingly
set(XACC_DIR "$ENV{HOME}/.xacc")
endif()

# Find Xacc (required)
find_package(XACC REQUIRED)

# include directories
add_subdirectory("src")