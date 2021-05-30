using LanguageExt;
using LanguageExt.Common;

namespace BankOCR.ConsoleApp
{
    public class Entry
    {
        public static Either<Arr<Entry>,Error> Parse(Arr<DataLine> dataLines)
        {
            return Error.New("Entries parsing error. Incorrect number of lines (0 lines provided)");
        }
    }
}