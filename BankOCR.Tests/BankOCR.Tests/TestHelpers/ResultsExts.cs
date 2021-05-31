using System.Text;
using FluentAssertions;
using FluentAssertions.Execution;
using LanguageExt;
using LanguageExt.Common;

namespace BankOCR.Tests
{
    public static class EitherBasedExtensions
    {
        public static EitherAssertions<TValue> Should<TValue>(this Either<Error, TValue> actualValue)
        {
            return new EitherAssertions<TValue>(actualValue);
        }
    }

    public class EitherAssertions<TValue>
    {
        private readonly Either<Error, TValue> _assertedResult;

        public EitherAssertions(Either<Error, TValue> assertedResult)
        {
            _assertedResult = assertedResult;
        }

        public AndConstraint<Either<Error, TValue>> BeFailure(string because = "", params object[] becauseArgs)
        {
            Execute
                .Assertion
                .ForCondition(_assertedResult.IsLeft)
                .BecauseOf($"!!{because}!!", becauseArgs)
                .FailWith("Expected FAILURE{reason}, but found SUCCESS!");

            return new AndConstraint<Either<Error, TValue>>(_assertedResult);
        }

        public AndConstraint<Either<Error, TValue>> BeSuccess(string because = "", params object[] becauseArgs)
        {
            Execute
                .Assertion
                .ForCondition(_assertedResult.IsRight)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected success{reason}, but found failure:\r\n" +
                    "═════════════════════════════════════════════════════════════════════════" +
                    "\r\n" +
                    "\r\n" +
                    GetFailureDescription() +
                    "\r\n" +
                    "═════════════════════════════════════════════════════════════════════════");

            return new AndConstraint<Either<Error, TValue>>(_assertedResult);
        }

        public string GetFailureDescription()
        {
            var a = _assertedResult.Match(
                Right: _ => "no error",
                Left: error => error.Message
            );
            return a;
        }

        public AndConstraint<Either<Error,TValue>> HaveError(Error error, string because = "",
            params object[] becauseArgs)
        {
            Execute
                .Assertion
                .ForCondition(
                    _assertedResult.Match(
                        Left: actualError => actualError == error,
                        Right: _ => false))
                .BecauseOf(because, becauseArgs)
                .FailWith("Error message \"{0}\" was excpected{reason}, but found {1}.", error, _assertedResult);

            return new AndConstraint<Either<Error,TValue>>(_assertedResult);
        }
        
        public AndConstraint<Either<Error,TValue>> HaveErrorThatContainsMessage(string msg, string because = "",
            params object[] becauseArgs)
        {
            Execute
                .Assertion
                .ForCondition(
                    _assertedResult.Match(
                        Left: actualError => actualError.Message.Contains(msg),
                        Right: _ => false))
                .BecauseOf(because, becauseArgs)
                .FailWith("Error message \"{0}\" was excpected{reason}, but found {1}.", msg, _assertedResult);

            return new AndConstraint<Either<Error,TValue>>(_assertedResult);
        }
    }
}