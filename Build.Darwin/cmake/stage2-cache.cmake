# This file sets up a CMakeCache for the second stage of a simple distribution
# bootstrap build.

set (LLVM_ENABLE_PROJECTS "clang;clang-tools-extra;lld" CACHE STRING "")
set (LLVM_ENABLE_RUNTIMES "compiler-rt;libcxx;libcxxabi;libunwind" CACHE STRING "")

set (LLVM_TARGETS_TO_BUILD X86 ARM AArch64 CACHE STRING "")

set (CMAKE_BUILD_TYPE RelWithDebInfo CACHE STRING "" FORCE)
set (CMAKE_C_FLAGS "-fno-stack-protector -fno-common -Wno-profile-instr-unprofiled" CACHE STRING "")
set (CMAKE_CXX_FLAGS "-fno-stack-protector -fno-common -Wno-profile-instr-unprofiled" CACHE STRING "")
# set (CMAKE_C_FLAGS_RELWITHDEBINFO "-O3 -gline-tables-only -DNDEBUG" CACHE STRING "")
# set (CMAKE_CXX_FLAGS_RELWITHDEBINFO "-O3 -gline-tables-only -DNDEBUG" CACHE STRING "")
set (CMAKE_MACOSX_RPATH ON CACHE STRING "")

# setup toolchain
set (LLVM_INSTALL_TOOLCHAIN_ONLY ON CACHE BOOL "")

set (LLVM_BUILD_UTILS ON CACHE BOOL "")
set (LLVM_INSTALL_UTILS ON CACHE BOOL "")
set (LLVM_CREATE_XCODE_TOOLCHAIN ON CACHE BOOL "")

set (LLVM_BUILD_TESTS ON CACHE BOOL "")
set (LLVM_ENABLE_LTO THIN CACHE BOOL "")

set (LLVM_TOOLCHAIN_TOOLS
     dsymutil
     llvm-cov
     llvm-dwarfdump
     llvm-profdata
     llvm-objdump
     llvm-nm
     llvm-size
     llvm-cxxfilt
     llvm-config
     CACHE STRING "")

set (LLVM_TOOLCHAIN_UTILITIES
     FileCheck
     yaml2obj
     not
     count
     CACHE STRING "")

set (LLVM_DISTRIBUTION_COMPONENTS
     clang
     LTO
     clang-format
     clang-resource-headers
     Remarks
     builtins
     runtimes
     ${LLVM_TOOLCHAIN_TOOLS}
     ${LLVM_TOOLCHAIN_UTILITIES}
     CACHE STRING "")

set(LIBCXX_USE_COMPILER_RT ON CACHE BOOL "")
