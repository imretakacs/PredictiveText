module FSharp.TV.PredictiveText.Test
open FsUnit
open NUnit.Framework
open FSharp.TV.PredictiveText

let dictionary = [|"fsharp";"test1";"test2"|]

[<Test>]
let shouldLoadUsDict() =
   LoadDict() |> should not' (be Empty)

[<Test>]
let shouldNotContainTheWordFSharp() =
    LoadDict() |> should not' (contain "fsharp")