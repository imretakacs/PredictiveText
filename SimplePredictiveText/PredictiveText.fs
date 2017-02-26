module FSharp.TV.PredictiveText
open System.IO
open System

let private usDict = Path.Combine(__SOURCE_DIRECTORY__, "dict.txt")

///Allow you to load your own dictionary of data
let LoadDictFromPath path =
   File.ReadAllLines path

 ///Loads the default US dictionary of words
let LoadDict () : string array = 
   let dic : string array = LoadDictFromPath usDict
   dic

///Find the words that starts with the given prefix
let Autocomplete (prefix:string) (dict:string[]) =
   let candidtes = dict |> Array.filter (fun word -> word.StartsWith prefix)
   candidtes