open FSharp.TV.PredictiveText

[<EntryPoint>]
let main argv = 
    let candidiates = Autocomplete "aa" (LoadDict())
    for candidate in candidiates do printfn "%A" candidate
    0 // return an integer exit code
