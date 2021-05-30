using System;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace BankOCR.ConsoleApp
{
    public record Entry
    {
        private readonly Arr<char> _chars;
        
        public Arr<char> GetContent()
        {
            return _chars;
        }
        
        private Entry(Arr<char> chars)
        {
            _chars = chars;
        }
        
        public static Either<Error, Entry> ParseFromLines(
            (InputLine FirstLine, InputLine SecondLine, InputLine ThirdLine) lines
        )
        {
            //todo:change to fold
            var parsedChars = new Arr<char>();
            
            for (var positionNo = 0; positionNo < 9; positionNo++)
            {
                var positionOffset = positionNo * 3;

                var charsForPosition = String.Join("",
                    lines.FirstLine.GetCharAt(positionOffset),
                    lines.FirstLine.GetCharAt(positionOffset + 1),
                    lines.FirstLine.GetCharAt(positionOffset + 2),
                    lines.SecondLine.GetCharAt(positionOffset),
                    lines.SecondLine.GetCharAt(positionOffset + 1),
                    lines.SecondLine.GetCharAt(positionOffset + 2),
                    lines.ThirdLine.GetCharAt(positionOffset),
                    lines.ThirdLine.GetCharAt(positionOffset + 1),
                    lines.ThirdLine.GetCharAt(positionOffset + 2));

                var currentCharacter = MatchChar(charsForPosition);
                
                parsedChars = parsedChars.Add(currentCharacter);
            }

            return new Entry(parsedChars);
        }

        private static char MatchChar(
            string input) =>
            (input) switch
            {
                (" _ " +
                 "| |" +
                 "|_|") => '0',
                ("   " +
                 " | " +
                 " | ") => '1',
                (" _ " +
                 " _|" +
                 "|_ ") => '2',
                (" _ " +
                 " _|" +
                 " _|") => '3',
                ("   " +
                 "|_|" +
                 "  |") => '4',
                (" _ " +
                 "|_ " +
                 " _|") => '5',
                (" _ " +
                 "|_ " +
                 "|_|") => '6',
                (" _ " +
                 "  |" +
                 "  |") => '7',
                (" _ " +
                 "|_|" +
                 "|_|") => '8',
                (" _ " +
                 "|_|" +
                 " _|") => '9',
                _ => '?'
            };
    }
}