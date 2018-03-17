using FluentValidation;
using NUnit.Framework;

namespace Stubbl.Api.IntegrationTests
{
    [SetUpFixture]
    public class SetupFixtureWithoutNamespace
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            ValidatorOptions.DisplayNameResolver = ValidatorOptions.PropertyNameResolver;
        }
    }
}