using System;
using System.Collections.Generic;
using System.IO;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace BankOCR.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var fileLocation = Path.Combine(currentDir, "test.txt");
            
            OpenFile(fileLocation)
                .Bind(InputData.Load) // try to load data from it
                .Bind(inputData => inputData.GetEntries())
                .Map(entries =>
                    entries.Map(
                        entry =>
                            Checksum.ForEntry(entry).Match(
                                    Right: checksum => $"{checksum}",
                                    Left: checksumCalcError => checksumCalcError.Message)
                                .Apply(checksumString => $"{entry}{Environment.NewLine}checksum:{checksumString}")
                    ))
                .Match(
                    Right: entriesWithTheirChecksums => entriesWithTheirChecksums.Iter(Console.WriteLine),
                    Left: error => Console.WriteLine(error));
        }

        private static Either<Error, Arr<string>> OpenFile(string path)
        {
            var result =
                Try(fun(() => File.ReadLines(path)))
                    .ToEither()
                    .BiMap(
                        linesColl => linesColl.ToArr(),
                        exception =>
                            Error.New($"Exception during attempt to open file: {exception.Message}"));

            return result;
        }
    }
}