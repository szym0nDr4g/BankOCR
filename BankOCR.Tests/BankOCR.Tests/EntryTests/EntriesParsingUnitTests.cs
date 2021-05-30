using System.IO;
using BankOCR.ConsoleApp;
using FsCheck;
using FsCheck.Xunit;
using LanguageExt;
using LanguageExt.Common;
using Xunit;

namespace BankOCR.Tests.FileLocationTests
{
    public record ParsingEntriesFromDataLines
    {
        public ParsingEntriesFromDataLines(Arr<DataLine> dataLines)
        {
            DataLines = dataLines;
        }
        
        public Arr<DataLine> DataLines { get; init; }

        public Either<Arr<Entry>, Error> Run()
        {
            var result = Entry.Parse(DataLines);
            return result;
        }

        public override string ToString()
        {
            return DataLines.Count.ToString();
        }
    }

    public class EntriesParsingUnitTests
    {
        [Property(Verbose=true, Arbitrary = new[] {typeof(Arbitraries),})]
        public Property
            Given_NumberOfDataLinesIsNotMultiplicationOf4_When_ParsingEntriesFromThatDataLine_Then_ErrorIsReturned(
                ParsingEntriesFromDataLines dataLinesParsing)
        {
            var result = dataLinesParsing.Run();

            var prop =
                result.IsRight
                    .When(
                        dataLinesParsing.DataLines.Count % 4 != 0);

            return prop;
        }
    }
}