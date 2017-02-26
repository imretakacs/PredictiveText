using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSharp.TV;

namespace UseFSharpInCSharp
{
   class Program
   {
      static void Main(string[] args)
      {
         var dict = PredictiveText.LoadDict();
         foreach (var candidates in PredictiveText.Autocomplete("aa", dict))
         {
            Console.WriteLine(candidates);
         }
         Console.ReadKey();
      }
   }
}
