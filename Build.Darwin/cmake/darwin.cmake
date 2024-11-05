
set (CMAKE_BUILD_TYPE MinSizeRel CACHE STRING "Build type")

set (CMAKE_MACOSX_RPATH ON CACHE BOOL "Enable RPATH handling for Darwin")

set (LLVM_TARGETS_TO_BUILD Native CACHE STRING "Build native target only")

set (LLVM_ENABLE_PROJECTS clang clang-tools-extra lld CACHE STRING "Initial build target")

set (LLVM_ENABLE_RUNTIMES compiler-rt libcxx libcxxabi libunwind CACHE STRING "Initial build runtimes")

set (LLVM_ENABLE_ZLIB OFF CACHE BOOL "Enable ZLIB")

set (LLVM_ENABLE_BACKTRACES OFF CACHE BOOL "")

set (PACAGE_VENDOR "objectx" CACHE STRING "Package vendor")

set (CLANG_BOOTSTRAP_TARGETS
    check-all
    check-llvm
    check-clang
    llvm-config
    test-suite
    test-depends
    llvm-test-depends
    clang-test-depends
    distribution
    install-distribution
    clang
    CACHE STRING "Bootstrap targets")

set (CLANG_ENABLE_BOOTSTRAP ON CACHE BOOL "Enable bootstrap")

set (CLANG_BOOTSTRAP_PASSTHROUGH CMAKE_OSX_ARCHITECTURES CACHE STRING "")

set (COMPILER_RT_ENABLE_IOS OFF CACHE BOOL "Enable iOS")
set (COMPILER_RT_ENABLE_WATCHOS OFF CACHE BOOL "Enable watchOS")
set (COMPILER_RT_ENABLE_TVOS OFF CACHE BOOL "Enable tvOS")

set (BOOTSTRAP_LLVM_ENABLE_LTO ON CACHE BOOL "Enable LTO")

set (LIBCXX_ENABLE_NEW_DELETE_DEFINITIONS OFF CACHE BOOL "")
set (LIBCXXABI_ENABLE_NEW_DELETE_DEFINITIONS ON CACHE BOOL "")
