using System;
using LanguageExt;
using LanguageExt.Common;

namespace BankOCR.ConsoleApp
{
    public class Entry
    {
        public Arr<char> GetValue()
        {
            return Arr<char>.Empty;
        }
        
        public static Either<Error,Entry> Parse(
            Tuple<Arr<char>,Arr<char>,Arr<char>,Arr<char>> dataLines
            )
        {
            if(dataLines.Item1.Count == 27)
                return new Entry();
                
                
            return Error.New("Entries parsing error. Incorrect number of lines (0 lines provided)");
        }
    }
}