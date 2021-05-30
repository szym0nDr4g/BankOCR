using System;
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
        public static Arbitrary<FakeMachineOutput> FakeMachineOutputGen()
        {
            var gen = from array in Gen.ArrayOf(
                        9,
                        Gen.OneOf(
                            Gen.Constant('1'),
                            Gen.Constant('2'),
                            Gen.Constant('3'),
                            Gen.Constant('4'),
                            Gen.Constant('5'),
                            Gen.Constant('6'),
                            Gen.Constant('7'),
                            Gen.Constant('8'),
                            Gen.Constant('9'),
                            Gen.Constant('0')
                        ))
                    select (FakeMachineOutput)FakeMachineOutput.FromActualValueOf(array);

            var arb = Arb.From(gen);

            return arb;
        }

        public static Arbitrary<ParsingEntryFromDataLines> ParsingEntriesFromDataLines()
        {
            var gen = from dataLines in Gen.Four(
                    Gen.OneOf(
                        Gen.ArrayOf(Arb.Generate<char>()), // any array of chars
                        Gen.ArrayOf( // any array of chars of length 27 contaning only pipes and underscores
                            27,
                            Gen.OneOf(
                                Gen.Constant('|'),
                                Gen.Constant('_'),
                                Gen.Constant(' '))),
                        Gen.Constant(new char[0]) // an empty array
                    )
                )
                select new ParsingEntryFromDataLines(
                    new Tuple<Arr<char>, Arr<char>, Arr<char>, Arr<char>>(
                        dataLines.Item1.ToArr(),
                        dataLines.Item2.ToArr(),
                        dataLines.Item3.ToArr(),
                        dataLines.Item4.ToArr())
                );
            var arb = Arb.From(gen);

            return arb;
        }
    }
}