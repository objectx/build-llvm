namespace Build.Darwin

open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators

open System.IO

/// Provides methods and properties related to the build environment.
module internal BuildEnv =
    /// Provides the root directory of the repository.
    let repositoryRoot = (__SOURCE_DIRECTORY__ @@ "..") |> Path.GetFullPath

    /// Represents the root directory of the build environment.
    let buildRoot = repositoryRoot @@ "0.B"

    /// <summary>Constructs a build directory path from <paramref name="stage"/>.</summary>
    /// <param name="stage">State identifier</param>
    let buildDirOf (stage: string) : string = buildRoot @@ stage

    /// <summary>
    /// Creates the root build directory.
    /// </summary>
    let prepareBuildRoot (_: TargetParameter) =
        Trace.trace $"""Preparing build root...: {buildRoot}"""
        Directory.ensure buildRoot

        "restic-ignore: 58B12CA6-717F-4DA1-894A-C3126D8DFB2E"
        |> File.writeString false (buildRoot @@ ".RESTIC-IGNORE")

    /// <summary>
    /// Cleans the specified build directory and removes all its contents.
    /// </summary>
    let clean (_: TargetParameter) =
        Trace.trace $"""Cleaning...: {buildRoot}"""
        Shell.rm_rf buildRoot
