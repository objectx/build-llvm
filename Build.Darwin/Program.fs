namespace Build.Darwin

open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.Core.TargetOperators
open BlackFox.CommandLine

module Toplevel =

    open Goodies

    let initTargets () =
        Target.create "PrepareBuildRoot" BuildEnv.prepareBuildRoot
        Target.create "Clean" BuildEnv.clean
        Target.create "Configure" Bootstrap.configure
        Target.create "Build" Bootstrap.build
        Target.create "Install" Bootstrap.install

        Target.create "All" ignore
        Target.create "Rebuild" ignore

        "Clean" ?=> "PrepareBuildRoot" |> ignore
        "Clean" ?=> "Configure" |> ignore

        "PrepareBuildRoot" ==> "Configure" |> ignore
        "PrepareBuildRoot" ==> "Build" |> ignore

        "Configure" ?=> "Build" |> ignore

        "Build" ?=> "Install" |> ignore

        "All" <== [ "Configure"; "Build"; "Install" ] |> ignore
        "Rebuild" <== [ "Clean"; "All" ] |> ignore

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
