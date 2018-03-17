using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using FluentValidation;
using NUnit.Framework;
using Stubbl.Api.Models.ValidationFailed.Version1;

namespace Stubbl.Api.IntegrationTests.Filters.Version1
{
    [TestFixtureSource(typeof(InvalidJsonTextFixtureData))]
    public class WhenSendingInvalidJson : IntegrationTest
    {
        private readonly HttpMethod _httpMethod;
        private readonly string _path;

        public WhenSendingInvalidJson(HttpMethod httpMethod, string path, object instance, Type validatorType)
            : base(1, HttpStatusCode.BadRequest, GenerateExpectedResponse(instance, validatorType))
        {
            _httpMethod = httpMethod;
            _path = path;
        }

        protected override HttpRequestMessage RequestMessage
        {
            get
            {
                const string body = "{}";

                return new HttpRequestMessage(_httpMethod, _path)
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };
            }
        }

        private static object GenerateExpectedResponse(object instance, Type validatorType)
        {
            var validator = Activator.CreateInstance(validatorType) as IValidator;

            if (validator == null)
            {
                return null;
            }

            var validationResult = validator.Validate(instance);
            var validationErrors = validationResult.Errors.OrderBy(ve => ve.PropertyName);

            return new ValidationFailedResponse
            (
                validationErrors.GroupBy(ve => ve.PropertyName)
                    .Select(g => g.First())
                    .Select(ve => new ValidationError(ve.PropertyName, ve.ErrorMessage))
                    .ToList()
            );
        }
    }
}