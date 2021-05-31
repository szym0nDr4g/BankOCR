using LanguageExt.ClassInstances;
using LanguageExt.TypeClasses;
using Xunit;

namespace BankOCR.Tests.InputDataTests
{
    public class InputDataUnitTests
    {
        [AutofixturedTest]
        [Theory]
        public void Given_NumberOfLinesIsNotDivisableByFour_When_LoadingInputData_Then_ErrorIsReturned(
            InputDataLoading inputDataLoading)
        {
            //a
            var numberOfRows = Fake.NumberIndivisibleByFour();
            var datasetWithIncorrectNumberOfLines =
                Fake.Any().Make(numberOfRows, (_) => Fake.Any().Random.String()).ToArr();
            var test = inputDataLoading with {DataLinesToLoadFrom = datasetWithIncorrectNumberOfLines};

            //aa
            var result = test.Run();

            //aaa
            result.Should()
                .BeFailure(
                    $"each entry in contains 4 lines so file containing {datasetWithIncorrectNumberOfLines} is wrong")
                .And.Should().HaveErrorThatContainsMessage("Incorrect number of lines");
        }

        [AutofixturedTest]
        [Theory]
        public void Given_ThereAreLinesThatContainsIncorrectNumberOfLines_When_LoadingInputData_Then_ErrorIsReturned(
            InputDataLoading inputDataLoading)
        {
            //a
            var dataSetWithIncorrectLines = 
                Fake.Any().Make(
                    count: Fake.Any().Random.Number(min: 1),
                    action: (_) => Fake.Any().Random.String(minLength: 1, maxLength: 26));
            
            var test = inputDataLoading with {DataLinesToLoadFrom = dataSetWithIncorrectLines.ToArr()};

            //aa
            var result = test.Run();

            //aaa
            result.Should()
                .BeFailure(
                    $"each entry should contain 0 or 27 characters")
                .And.Should().HaveErrorThatContainsMessage("Line should contain exactly");
        }
    }
}