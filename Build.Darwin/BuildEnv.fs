namespace Build.Darwin

open Fake.IO.FileSystemOperators

open System.IO

module BuildEnv =
    let internal repositoryRoot = (__SOURCE_DIRECTORY__ @@ "..") |> Path.GetFullPath

    let internal buildRoot = repositoryRoot @@ "0.B"

    let internal buildDirOf (stage: string): string =
        buildRoot @@ stage
