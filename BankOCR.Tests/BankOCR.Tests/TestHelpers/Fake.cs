using Bogus;

namespace BankOCR.Tests.InputDataTests
{
    public static class Fake
    {
        public static Faker Any()
        {
            var faker = new Faker();
            return faker;
        }

        public static Faker<T> Any<T>() where T : class
        {
            var faker = new Faker<T>();
            return faker;
        }

        public static int NumberIndivisibleByFour()
        {
            var number = Fake.Any().Random.UInt(1, 100);
            var numberDivisableByFour = number * 4;
            var numberInDivisableByFour = numberDivisableByFour + Fake.Any().Random.Number(1, 3);
            return (int) numberInDivisableByFour;
        }
    }
}