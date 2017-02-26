open System.IO
open System

let data = """
buses
buses
bwana
bylaw
bytes
byron
buzzy
buxom
bused
buyton
buyer
butts
butte
"""

let usDict = Path.Combine(__SOURCE_DIRECTORY__, "dict.txt")

let loadDictFromPath path =
   File.ReadAllLines path;



let loadDict () = 
   let dic = loadDictFromPath usDict
   dict