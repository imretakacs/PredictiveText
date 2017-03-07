open System
open System.IO

type Label = Spam | Ham

type Corpus = {
   spamFreqs:Map<string,int>;
   hamFreqs:Map<string,int>;
   spamCount: int;
   hamCount: int;
   totalCount:int;
}

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

let features (msg:string) : seq<string> =
   msg.Split( [|' '|] ) |> Array.toSeq

let countLabel (label:Label) (labeledWords:seq<Label*string>) : int =
   labeledWords
   |> Seq.filter(fun (l,w) -> l = label)
   |> Seq.length
    
let frequencies label labeledWords : Map<string, int> =
   labeledWords
   |> Seq.filter (fun (l,w) -> l=label)
   |> Seq.groupBy (fun (l,w) -> w)
   |> Seq.map (fun (w,ws)-> (w, Seq.length ws))
   |> Map.ofSeq

let pHam (corpus:Corpus) : float=
   (float corpus.hamCount) / (float corpus.totalCount)

let pSpam (corpus:Corpus) : float=
   (float corpus.spamCount) / (float corpus.totalCount)

let getCount map word =
   match (Map.tryFind word map) with
   | Some(x) -> (float x)
   | _ -> 0.0001

let pWordGivenSpam word (corpus:Corpus) : float =
   (getCount corpus.spamFreqs word) / (float corpus.spamCount)

let pWordGivenHam word (corpus:Corpus) : float =
   (getCount corpus.hamFreqs word) / (float corpus.hamCount)

let pWord word (corpus:Corpus) : float =
   (pWordGivenHam word corpus)*(pHam corpus) + (pWordGivenSpam word corpus)*(pSpam corpus)


let pHamGivenWords words corpus =
   let product =
      words
      |> Seq.map (fun w-> (pWordGivenHam w corpus) / (pWord w corpus))
      |> Seq.reduce (*)
   product* (pHam corpus)

let pSpamGivenWords words corpus =
   let product =
      words
      |> Seq.map (fun w-> (pWordGivenSpam w corpus) / (pWord w corpus))
      |> Seq.reduce (*)
   product* (pSpam corpus)

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
    let words : seq<Label*seq<string>> =
      training
      |> Seq.map(fun (label, msg) -> (label, features msg))

    let labeledWords : seq<Label*string> =
      words
      |>Seq.collect (fun (l, words) -> Seq.map(fun w -> (l,w)) words)

    let corpus = {
      spamFreqs = (frequencies Spam labeledWords);
      hamFreqs = (frequencies Ham labeledWords);
      hamCount = (countLabel Ham labeledWords);
      spamCount = (countLabel Spam labeledWords);
      totalCount = (Seq.length labeledWords);
    }

    let BayesClassify (msg:string) : Label =
      let words = features msg
      let pHam = pHamGivenWords words corpus
      let pSpam = pSpamGivenWords words corpus
      if pHam > pSpam then Ham else Spam

    
    printfn "Training size: %A" training.Length
    printfn "The accuracy is %A" (accuracyRate BayesClassify validation)
    printfn "Total=%A, Ham=%A, Spam=%A" corpus.totalCount corpus.hamCount corpus.spamCount
    printfn "'the' occurs %A in Ham" (corpus.hamFreqs.Item "the")
    let value = Console.ReadKey()
    0 // return an integer exit code
