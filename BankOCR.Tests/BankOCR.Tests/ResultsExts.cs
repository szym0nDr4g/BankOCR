using System.Text;
using FluentAssertions;
using FluentAssertions.Execution;
using LanguageExt;
using LanguageExt.Common;

namespace BankOCR.Tests
{
    public static class EitherBasedExtensions
    {
        public static EitherAssertions<TValue> Should<TValue>(this Either<TValue, Error> actualValue)
        {
            return new EitherAssertions<TValue>(actualValue);
        }
    }

    public class EitherAssertions<TValue>
    {
        private readonly Either<TValue, Error> _assertedResult;

        public EitherAssertions(Either<TValue, Error> assertedResult)
        {
            _assertedResult = assertedResult;
        }

        public AndConstraint<Either<TValue, Error>> BeFailure(string because = "", params object[] becauseArgs)
        {
            Execute
                .Assertion
                .ForCondition(_assertedResult.IsRight)
                .BecauseOf($"!!{because}!!", becauseArgs)
                .FailWith("Expected FAILURE{reason}, but found SUCCESS!");

            return new AndConstraint<Either<TValue, Error>>(_assertedResult);
        }

        public AndConstraint<Either<TValue, Error>> BeSuccess(string because = "", params object[] becauseArgs)
        {
            Execute
                .Assertion
                .ForCondition(_assertedResult.IsLeft)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected success{reason}, but found failure:\r\n" +
                    "═════════════════════════════════════════════════════════════════════════" +
                    "\r\n" +
                    "\r\n" +
                    GetFailureDescription() +
                    "\r\n" +
                    "═════════════════════════════════════════════════════════════════════════");

            return new AndConstraint<Either<TValue, Error>>(_assertedResult);
        }

        public string GetFailureDescription()
        {
            var a = _assertedResult.Match(
                Right: error => error.Message,
                Left: _ => "no error");
            return a;
        }

        public AndConstraint<Either<TValue, Error>> HaveError(Error error, string because = "",
            params object[] becauseArgs)
        {
            Execute
                .Assertion
                .ForCondition(
                    _assertedResult.Match(
                            Right: actualError => actualError == error,
                            Left: _ => false))
                .BecauseOf(because, becauseArgs)
                .FailWith("Error message \"{0}\" was excpected{reason}, but found {1}.", error, _assertedResult);

            return new AndConstraint<Either<TValue, Error>>(_assertedResult);
        }
    }
}