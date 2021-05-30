using LanguageExt;
using LanguageExt.Common;

namespace BankOCR.ConsoleApp
{
    public record InputLine 
    {
        private readonly Arr<char> _data;
        public static Either<Error, InputLine> New(Arr<char> data)
        {
            return new InputLine(data);
        }

        private InputLine(Arr<char> data)
        {
            _data = data;
        }
        
        public char GetCharAt(int position)
        {
            return _data[position];
        }
    }
}