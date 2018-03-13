namespace Stubbl.Api.IntegrationTests
{
   using System.Text.RegularExpressions;
   using FluentValidation;
   using NUnit.Framework;

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
