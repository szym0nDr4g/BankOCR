using System;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace BankOCR.ConsoleApp
{
    public class Checksum
    {
        public enum Validity
        {
            IsValid,
            IsInvalid
        }

        private readonly int _value;

        private Checksum(int value)
        {
            _value = value;
        }

        public Validity CheckValidity() => _value switch
        {
            var val when val % 11 == 0 => Validity.IsValid,
            var val when val % 11 != 0 => Validity.IsInvalid,
            _ => Validity.IsInvalid
        };

        public static Either<Error, Checksum> ForEntry(Entry entry)
        {
            var values = entry.GetContent();
            if (values.Any(part => !Char.IsDigit(part)))
                return Error.New("Cannot calculate checksum for incorrect entry");

            var checksumValue =
                values
                    .Map(entryPart => System.Int32.Parse(entryPart.ToString()))
                    .Map((positionNumber, entryPartValue) => entryPartValue * (9 - positionNumber))
                    .Fold(0, (state, checksumPart) => state += checksumPart);

            return new Checksum(checksumValue);
        }
    }
}