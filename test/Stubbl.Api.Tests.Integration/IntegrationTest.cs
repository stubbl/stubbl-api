namespace Stubbl.Api.Tests.Integration
{
   using Common.Commands;
   using Common.Queries;
   using FluentAssertions;
   using Microsoft.AspNetCore.Hosting;
   using Microsoft.AspNetCore.TestHost;
   using Microsoft.Extensions.DependencyInjection;
   using Newtonsoft.Json;
   using NSubstitute;
   using NUnit.Framework;
   using System;
   using System.Net;
   using System.Net.Http;
   using System.Net.Http.Headers;
   using System.Threading.Tasks;

   public abstract class IntegrationTest
   {
      private Lazy<HttpResponseMessage> _responseMessage;

      protected IntegrationTest(int versionNumber, HttpStatusCode expectedStatusCode)
         : this(versionNumber, expectedStatusCode, null)
      {
      }

      protected IntegrationTest(int versionNumber, HttpStatusCode expectedStatusCode, object expectedResponse)
      {
         VersionNumber = versionNumber;
         ExpectedStatusCode = expectedStatusCode;

         if (expectedResponse != null)
         {
            ExpectedResponseJson = JsonConvert.SerializeObject(expectedResponse, JsonConstants.JsonSerializerSettings);
         }

         CommandDispatcher = Substitute.For<ICommandDispatcher>();
         QueryDispatcher = Substitute.For<IQueryDispatcher>();

         var builder = new WebHostBuilder()
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
         httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue($"application/vnd.stubbl.v{VersionNumber}+json"));
         httpClient.DefaultRequestHeaders.Add("X-Sub", "000000000000-0000-0000-0000-00000001");

         _responseMessage = new Lazy<HttpResponseMessage>(() => httpClient.SendAsync(RequestMessage).Result);
      }

      protected ICommandDispatcher CommandDispatcher { get; }
      protected string ExpectedResponseJson { get; }
      protected HttpStatusCode ExpectedStatusCode { get; }
      protected abstract HttpRequestMessage RequestMessage { get; }
      protected HttpResponseMessage ResponseMessage { get { return _responseMessage.Value; } }
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
      public void Then_the_status_code_should_be_as_expected() => ResponseMessage.StatusCode.Should().Be(ExpectedStatusCode);
   }
}