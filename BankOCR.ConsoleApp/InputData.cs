using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.VisualBasic;
using static LanguageExt.Prelude;

namespace BankOCR.ConsoleApp
{
    public record InputData
    {
        private readonly Arr<string> _inputData;

        private InputData(Arr<string> inputData)
        {
            _inputData = inputData;
        }

        public static Either<Error, InputData> Load(Arr<string> dataLines)
        {
            var result = ValidateDataLines(dataLines)
                    .Map(correctDataLines => new InputData(correctDataLines))
                    .ToEither()
                    .MapLeft(seq => Error.New(seq.ToFullString())); 

            return result;
        }

        public Either<Error, Arr<Entry>> GetEntries()
        {
            var result = _inputData
                .Map((lineNumber, lineContent) => (
                    lineContent: lineContent,
                    positionInEntry: ResolvePlacementInEntry(lineNumber)
                ))
                .Fold(
                    HashMap<int, HashMap<LinePosition, string>>(),
                    (hashMap, lineInfo) =>
                        hashMap.AddOrUpdate(
                            key: lineInfo.positionInEntry.EntryNumber,
                            Some: entryContent =>
                                entryContent.Add(lineInfo.positionInEntry.PositionInEntry, lineInfo.lineContent),
                            None: () => HashMap<LinePosition, string>((lineInfo.positionInEntry.PositionInEntry, lineInfo.lineContent))))
                .Values
                .Map(entryContent =>
                    ParseEntry(
                        entryContent[LinePosition.FirstLine],
                        entryContent[LinePosition.SecondLine],
                        entryContent[LinePosition.ThirdLine]))
                .Sequence()
                .Map(seq => seq.ToArr());

            return result;
        }

        private static Either<Error, Entry> ParseEntry(string a, string b, string c)
        {
            var lines =
                from firstLine in InputLine.New(a.ToArr())
                from secondLine in InputLine.New(b.ToArr())
                from thirdLine in InputLine.New(c.ToArr())
                select (firstLine, secondLine, thirdLine);

            var entry = lines.Bind(Entry.ParseFromLines);

            return entry;
        }

        private enum LinePosition
        {
            FirstLine,
            SecondLine,
            ThirdLine,
            FourthLine,
            Unknown
        }

        private static (int EntryNumber, LinePosition PositionInEntry) ResolvePlacementInEntry(int lineNumber) =>
        (
            EntryNumber: lineNumber / 4,
            PositionInEntry: lineNumber switch
            {
                var val when val % 4 == 0 => LinePosition.FirstLine,
                var val when val % 4 == 1 => LinePosition.SecondLine,
                var val when val % 4 == 2 => LinePosition.ThirdLine,
                var val when val % 4 == 3 => LinePosition.FourthLine,
                _ => LinePosition.Unknown
            }
        );


        private static Validation<Error, Arr<string>> AllLinesHaveCorrectLength(Arr<string> dataLines) =>
            dataLines.Map(
                    (lineIndex, lineContent) =>
                        (LineIsSplittingLine(lineIndex)
                            ? EnsureThatLineHasLengthOf(0, lineContent)
                            : EnsureThatLineHasLengthOf(27, lineContent))
                        .MapFail(lineError => Error.New($"Error in line {lineIndex}({lineError})")))
                .ToArr()
                .Sequence();

        private static bool LineIsSplittingLine(int lineIndex) => (lineIndex + 1) % 4 == 0;

        private static Validation<Error, string> EnsureThatLineHasLengthOf(int length, string line)
        {
            return line.Length == length
                ? Success<Error, string>(line)
                : Fail<Error, string>(Error.New($"Line should contain exactly {length} characters long."));
        }

        private static Validation<Error, Arr<string>> ValidateDataLines(Arr<string> dataLines) =>
            AllLinesHaveCorrectLength(dataLines) | NumberOfLinesIsCorrect(dataLines);

        private static Validation<Error, Arr<string>> NumberOfLinesIsCorrect(Arr<string> dataLines) =>
            dataLines.Count % 4 == 0
                ? Success<Error, Arr<string>>(dataLines)
                : Fail<Error, Arr<string>>(Error.New("Incorrect number of lines"));
    }
}