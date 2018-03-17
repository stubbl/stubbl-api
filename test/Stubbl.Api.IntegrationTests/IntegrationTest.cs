using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Gunnsoft.Cqs.Commands;
using Gunnsoft.Cqs.Queries;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace Stubbl.Api.IntegrationTests
{
    public abstract class IntegrationTest
    {
        private readonly Lazy<HttpResponseMessage> _responseMessage;

        protected IntegrationTest(int versionNumber, HttpStatusCode expectedStatusCode, object expectedResponse = null,
            IHeaderDictionary expectedHeaders = null)
        {
            VersionNumber = versionNumber;
            ExpectedStatusCode = expectedStatusCode;

            if (expectedResponse != null)
            {
                ExpectedResponseJson =
                    JsonConvert.SerializeObject(expectedResponse, JsonConstants.JsonSerializerSettings);
            }

            ExpectedHeaders = expectedHeaders;

            CommandDispatcher = Substitute.For<ICommandDispatcher>();
            QueryDispatcher = Substitute.For<IQueryDispatcher>();

            var builder = Program.GetWebHostBuilder(AppContext.BaseDirectory)
                .ConfigureServices(s =>
                {
                    s.AddTransient(cc => CommandDispatcher);
                    s.AddTransient(cc => QueryDispatcher);

                    ConfigureServices(s);
                })
                .UseEnvironment("IntegrationTesting")
                .UseStartup<Startup>();
            var server = new TestServer(builder);
            var httpClient = server.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue($"application/vnd.stubbl.v{VersionNumber}+json"));
            httpClient.DefaultRequestHeaders.Add("X-Sub", "000000000000-0000-0000-0000-00000001");

            _responseMessage = new Lazy<HttpResponseMessage>(() => httpClient.SendAsync(RequestMessage).Result);
        }

        protected ICommandDispatcher CommandDispatcher { get; }
        protected IHeaderDictionary ExpectedHeaders { get; }
        protected string ExpectedResponseJson { get; }
        protected HttpStatusCode ExpectedStatusCode { get; }
        protected abstract HttpRequestMessage RequestMessage { get; }
        protected HttpResponseMessage ResponseMessage => _responseMessage.Value;
        protected IQueryDispatcher QueryDispatcher { get; }
        protected int VersionNumber { get; }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        [Test]
        public async Task Then_the_body_should_be_as_expected()
        {
            var body = await ResponseMessage.Content.ReadAsStringAsync();

            if (ExpectedResponseJson == null)
            {
                body.Should().BeEmpty();
            }
            else
            {
                body.Should().Be(ExpectedResponseJson);
            }
        }

        [Test]
        public void Then_the_headers_should_be_as_expected()
        {
            if (ExpectedHeaders == null)
            {
                return;
            }

            foreach (var expectedHeader in ExpectedHeaders)
            {
                if (!ResponseMessage.Headers.TryGetValues(expectedHeader.Key, out var actualHeader))
                {
                    Assert.Fail($"Header {expectedHeader.Key} is missing");
                }

                actualHeader.Should().Contain(expectedHeader.Value);
            }
        }

        [Test]
        public void Then_the_status_code_should_be_as_expected()
        {
            ResponseMessage.StatusCode.Should().Be(ExpectedStatusCode);
        }
    }
}