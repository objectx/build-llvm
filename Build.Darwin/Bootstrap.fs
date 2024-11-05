namespace Build.Darwin

module internal Bootstrap =
    open Fake.Core
    open Fake.IO.FileSystemOperators
    open BlackFox.CommandLine

    open Goodies

    open System.IO

    let configure (_: TargetParameter) =
        let cmakeCacheDir = (__SOURCE_DIRECTORY__ @@ "cmake") |> Path.GetFullPath

        let buildDir = BuildEnv.buildDirOf "bootstrap"

        let cacheVars =
            let stage2Args = toCMakeList [| "-C"; cmakeCacheDir @@ "stage2-cache.cmake" |]
            let installDir = BuildEnv.buildRoot @@ "stage2-install"

            [| "CMAKE_C_COMPILER_LAUNCHER=sccache"
               "CMAKE_CXX_COMPILER_LAUNCHER=sccache"
               $"CMAKE_INSTALL_PREFIX={installDir}"
               $"CLANG_BOOTSTRAP_CMAKE_ARGS={stage2Args}" |]

        Trace.trace $"Configuring in {buildDir}..."

        CmdLine.Empty
        |> CmdLine.appendPrefix "-B" buildDir
        |> CmdLine.appendPrefix "-G" "Ninja"
        |> CmdLine.appendPrefix "-C" (cmakeCacheDir @@ "darwin.cmake")
        |> CmdLine.appendPrefixSeq "-D" cacheVars
        |> CmdLine.append "llvm-project/llvm"
        |> CmdLine.toArray
        |> CreateProcess.fromRawCommand "cmake"
        |> CreateProcess.ensureExitCode
        |> CreateProcess.withWorkingDirectory BuildEnv.repositoryRoot
        |> Proc.run
        |> ignore

    let build (_: TargetParameter) =
        let buildDir = BuildEnv.buildDirOf "bootstrap"
        Trace.trace $"Building in {buildDir}..."

        CmdLine.Empty
        |> CmdLine.appendPrefix "--build" buildDir
        |> CmdLine.appendPrefix "--target" "stage2-distribution"
        // |> CmdLine.append "-v"
        |> CmdLine.toArray
        |> CreateProcess.fromRawCommand "cmake"
        |> CreateProcess.ensureExitCode
        |> CreateProcess.withWorkingDirectory BuildEnv.repositoryRoot
        |> Proc.run
        |> ignore

    let install (_: TargetParameter) =
        let buildDir = BuildEnv.buildDirOf "bootstrap"
        Trace.trace $"Installing in {buildDir}..."

        CmdLine.Empty
        |> CmdLine.appendPrefix "--build" buildDir
        |> CmdLine.appendPrefix "--target" "stage2-install-distribution"
        |> CmdLine.toArray
        |> CreateProcess.fromRawCommand "cmake"
        |> CreateProcess.ensureExitCode
        |> CreateProcess.withWorkingDirectory BuildEnv.repositoryRoot
        |> Proc.run
        |> ignore
