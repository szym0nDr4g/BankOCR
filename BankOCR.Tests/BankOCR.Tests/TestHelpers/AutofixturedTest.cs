using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace BankOCR.Tests
{
    public class AutofixturedTestAttribute : AutoDataAttribute
    {
        public AutofixturedTestAttribute() :
            base(GetFixture)
        {
        }

        private static IFixture GetFixture()
        {
            var fixture = new Fixture();
  
            var customization = new SupportMutableValueTypesCustomization();
            customization.Customize(fixture);
            return fixture;
        }
    }
}