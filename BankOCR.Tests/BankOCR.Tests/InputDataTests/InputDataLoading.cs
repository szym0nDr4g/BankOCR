using BankOCR.ConsoleApp;
using LanguageExt;
using LanguageExt.Common;

namespace BankOCR.Tests.InputDataTests
{
    public record InputDataLoading
    {
        public InputDataLoading(Arr<string> sourceDataLines)
        {
            DataLinesToLoadFrom = sourceDataLines;
        }

        public Arr<string> DataLinesToLoadFrom { get; init; }

        public Either<Error, InputData> Run()
        {
            var result = InputData.Load(DataLinesToLoadFrom);

            return result;
        }
    }
}