namespace Build.Darwin

open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.Core.TargetOperators
open BlackFox.CommandLine

module Toplevel =
    let private toCMakeList (items: seq<string>) : string = items |> String.concat ";"

    let prepareBuildRoot (_: TargetParameter) =
        Trace.trace $"""Preparing build root...: {BuildEnv.buildRoot}"""
        Directory.ensure BuildEnv.buildRoot

        "restic-ignore: 58B12CA6-717F-4DA1-894A-C3126D8DFB2E"
        |> File.writeString false (BuildEnv.buildRoot @@ ".RESTIC-IGNORE")

    let clean (_: TargetParameter) =
        Trace.trace $"""Cleaning...: {BuildEnv.buildRoot}"""
        Shell.rm_rf BuildEnv.buildRoot

    let configure (_: TargetParameter) =
        let buildDir = BuildEnv.buildDirOf "bootstrap"
        let projects = [| "clang"; "clang-tools-extra"; "lld" |]
        let runtimes = [| "compiler-rt"; "libcxx"; "libcxxabi"; "libunwind" |]

        let bootstrapTargets =
            [| "check-all"
               "check-llvm"
               "check-clang"
               "llvm-config"
               "test-suite"
               "test-depends"
               "llvm-test-depends"
               "clang-test-depends"
               "distribution"
               "install-distribution"
               "clang" |]

        let cacheVars =
            [| "CMAKE_INSTALL_PREFIX=" + (BuildEnv.buildRoot @@ "stage2-install")
               "CMAKE_BUILD_TYPE=MinSizeRel"
               "LLVM_TARGETS_TO_BUILD=Native"
               "LLVM_ENABLE_PROJECTS=" + toCMakeList projects
               "LLVM_ENABLE_RUNTIMES=" + toCMakeList runtimes
               "PACKAGE_VENDOR=objectx"
               "BOOTSTRAP_LLVM_ENABLE_LTO=ON"
               "CLANG_BOOTSTRAP_TARGETS=" + toCMakeList bootstrapTargets
               "CLANG_ENABLE_BOOTSTRAP=ON"
               "CLANG_BOOTSTRAP_CMAKE_ARGS="
               + toCMakeList [| "-C"; (BuildEnv.repositoryRoot @@ "stage2-cache.cmake") |] |]

        Trace.trace $"Configuring in {buildDir}..."

        CmdLine.Empty
        |> CmdLine.appendPrefix "-B" buildDir
        |> CmdLine.appendPrefix "-G" "Ninja"
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

    let run (_: TargetParameter) = Trace.trace "Running..."

    let initTargets () =
        Target.create "PrepareBuildRoot" prepareBuildRoot
        Target.create "Clean" clean
        Target.create "Configure" configure
        Target.create "Build" build
        Target.create "Install" install
        Target.create "All" ignore
        Target.create "Rebuild" ignore

        "Clean" ?=> "PrepareBuildRoot" |> ignore
        "Clean" ?=> "Configure" |> ignore

        "PrepareBuildRoot" ==> "Configure" |> ignore
        "PrepareBuildRoot" ==> "Build" |> ignore

        "Configure" ?=> "Build" |> ignore

        "Build" ?=> "Install" |> ignore

        "All" <== [ "Configure"; "Build"; "Install" ] |> ignore
        "Rebuild" <== [ "Clean"; "Configure"; "Build"; "Install" ] |> ignore

    [<EntryPoint>]
    let main argv =
        argv
        |> Array.toList
        |> Context.FakeExecutionContext.Create false "build.fsx"
        |> Context.RuntimeContext.Fake
        |> Context.setExecutionContext

        initTargets ()
        Target.runOrList ()
        0 // return an integer exit code
