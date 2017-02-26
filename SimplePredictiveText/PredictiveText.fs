module FSharp.TV.PredictiveText
open System.IO
open System

let usDict = Path.Combine(__SOURCE_DIRECTORY__, "dict.txt")

let loadDictFromPath path =
   File.ReadAllLines path

let loadDict () : string array = 
   let dic : string array = loadDictFromPath usDict
   dic

let loaded : string array = loadDict()

let filtering =
   let data = [|"butyl";"buyer";"buzzy";"bwana";"bylaw";"byron";"bytes"|]
   let candidtes = data |> Array.filter (fun word -> word.Contains "w")
   candidtes

let autocomplete (prefix:string) (dict:string[]) =
   let candidtes = dict |> Array.filter (fun word -> word.StartsWith prefix)
   candidtes