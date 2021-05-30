using System.Linq;
using BankOCR.ConsoleApp;
using BankOCR.Tests.FileLocationTests;
using FsCheck;
using LanguageExt;
using LanguageExt.ClassInstances.Const;
using LanguageExt.TypeClasses;
using Microsoft.FSharp.Core;

namespace BankOCR.Tests
{
    //this class is used to provide a way of data generation for FSCheck.
    public class Arbitraries
    {
        public static Arbitrary<Arr<DataLine>> DataLines()
        {
            return Arb.From(
                from items in
                    Gen.OneOf( //generate one of following with equal probability:
                        Gen.ArrayOf(Arb.Generate<DataLine>()), // any array
                        Gen.ArrayOf(Arb.Generate<DataLine>())
                            .Where(arr => arr.Length > 0 && arr.Length % 4 == 0) //array divisable by four
                    )
                select
                    items.ToArr()
            );
        }

        public static Arbitrary<ParsingEntriesFromDataLines> ParsingEntriesFromDataLines()
        {
            var gen = from itemsArray in Arb.Generate<Arr<DataLine>>()
                select new ParsingEntriesFromDataLines(itemsArray);
            var arb = Arb.From(gen);
            
            return arb;
        }
    }
}