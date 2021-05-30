using System.IO;
using BankOCR.ConsoleApp;
using FsCheck.Xunit;
using LanguageExt;
using LanguageExt.Common;
using Xunit;

namespace BankOCR.Tests.FileLocationTests
{
    public record ParsingEntriesFromDataLines
    {
        public Arr<string> DataLines { get; init; }

        public Either<Arr<Entry>, Error> Run()
        {
            var result = Entry.Parse(DataLines);
            return result;
        }
    }

    public class EntriesParsingUnitTests
    {
        [Fact]
        public void Given_EmptyArrayOfDataLines_When_ParsingEntriesFromThatDataLine_Then_ErrorIsReturned()
        {
            //a
            var emptyArrayOfDataLines = Arr<string>.Empty;
            var test = new ParsingEntriesFromDataLines()
                {DataLines = emptyArrayOfDataLines};

            //aa
            var testResult = test.Run();

            //aaa
            var expectedError = Error.New("Entries parsing error. Incorrect number of lines (0 lines provided)");
            testResult
                .Should().BeFailure("At least four lines need to be provided in order to succesfuly parse data entries")
                .And
                .Should().HaveError(expectedError, "error needs to state issue to the user");
        }
    }
}