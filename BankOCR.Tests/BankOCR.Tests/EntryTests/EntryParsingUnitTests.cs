using System;
using System.IO;
using BankOCR.ConsoleApp;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using LanguageExt;
using LanguageExt.Common;
using Xunit;

namespace BankOCR.Tests.FileLocationTests
{
    public record ParsingEntryFromDataLines
    {
        public ParsingEntryFromDataLines(Tuple<Arr<Char>, Arr<Char>, Arr<Char>, Arr<Char>> dataLines)
        {
            DataLines = dataLines;
        }

        public Tuple<Arr<Char>, Arr<Char>, Arr<Char>, Arr<Char>> DataLines { get; init; }

        public Either<Error, Entry> Run()
        {
            var result = Entry.Parse(DataLines);
            return result;
        }
    }

    public class EntryParsingUnitTests
    {
        //Each line need to have 0 or 27 characters
        [Property(Verbose = true, Arbitrary = new[] {typeof(Arbitraries)})]
        public Property
            Given_AnyLineHasInvalidNumberOfCharacters_When_ParsingTheEntryFromThoseLines_Then_ErrorIsReturned(
                ParsingEntryFromDataLines entryParsing)
        {
            var result = entryParsing.Run();

            var prop =
                result.IsLeft
                    .When(
                        entryParsing.DataLines.Item1.Count != 27 ||
                        entryParsing.DataLines.Item2.Count != 27 ||
                        entryParsing.DataLines.Item3.Count != 27 ||
                        entryParsing.DataLines.Item4.Count != 0
                    );

            return prop;
        }

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

            return (((Entry) result).GetValue() == fakeMachineOutput.ActualOutputValue).ToProperty();
        }
    }
}