open System
open System.IO

type Label = Spam | Ham

let classify (msg:string) : Label =
   Spam

let accuracyRate (classifier:string->Label) (labeledData:seq<Label*string>) : float =
   let numberCorrectlyClassified =
      labeledData
         |> Seq.sumBy (fun (label,msg) -> if (classifier msg) = label then 1.0 else 0.0)
   numberCorrectlyClassified / float (Seq.length labeledData)

let string2Label str =
   match str with
      | "ham" -> Ham
      | _ -> Spam

let makeRandomPredicate fractionTrue =
   let r = Random()
   let predicate x =
      r.NextDouble() < fractionTrue
   predicate

[<EntryPoint>]
let main argv = 
    let path = Path.Combine(__SOURCE_DIRECTORY__, "SMSSpamCollection.txt")
    let lines = File.ReadLines(path)
    let labelData =
      lines
      |> Seq.map(fun l -> l.Split( [|'\t'|] ) )
      |> Seq.map(fun stra -> (string2Label stra.[0]), stra.[1])
    printfn "random pred: %A" ((makeRandomPredicate 0.9) ())

    let training, validation =
      labelData
      |> Seq.toList 
      |> List.partition(makeRandomPredicate 0.8)

    printfn "Training size: %A" training.Length
    printfn "The accuracy is %A" (accuracyRate classify labelData)
    let value = Console.ReadKey()
    0 // return an integer exit code
