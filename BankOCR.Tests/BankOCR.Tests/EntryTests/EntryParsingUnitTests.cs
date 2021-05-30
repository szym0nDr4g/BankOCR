using System;
using System.IO;
using BankOCR.ConsoleApp;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using LanguageExt;
using LanguageExt.Common;
using Xunit;
using static LanguageExt.Prelude;

namespace BankOCR.Tests.FileLocationTests
{
    public record ParsingEntryFromDataLines
    {
        public ParsingEntryFromDataLines((InputLine first, InputLine second, InputLine third) dataLines)
        {
            DataLines = dataLines;
        }
        
        public ParsingEntryFromDataLines((Arr<char> first, Arr<char> second, Arr<char> third) dataLines)
        {
            var lines = map(dataLines, (first,second,third) =>
                (
                    (InputLine)InputLine.New(first),
                    (InputLine)InputLine.New(second),
                    (InputLine)InputLine.New(third)
                ));
            
            DataLines = lines;
        }

        public (InputLine first, InputLine second, InputLine third) DataLines { get; init; }

        public Either<Error, Entry> Run()
        {
            var result = Entry.Parse(DataLines); 
           
            return result;
        }
    }
    
    public class EntryParsingUnitTests
    {
        [Property(Verbose = true, Arbitrary = new[] {typeof(Arbitraries)})]
        public Property Given_AnyValidOutputOfMachine_AfterParsing_ValueShouldBeSame(
            FakeMachineOutput fakeMachineOutput
        )
        {
            //a
            var fakeOutput = fakeMachineOutput.MachineOutput;
            var test = new ParsingEntryFromDataLines(fakeOutput);

            //aa
            var result = test.Run();

            //aaa
            result.Should().BeSuccess();
            var actualResult = ((Entry) result).GetValue();
            var expectedResult = fakeMachineOutput.ActualOutputValue;
            return (actualResult == expectedResult).ToProperty()
                .Label($"Should be parsed into {expectedResult} (Actually parsed as {actualResult})");
        }
    }
}