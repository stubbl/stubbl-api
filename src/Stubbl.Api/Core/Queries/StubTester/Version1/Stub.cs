namespace Stubbl.Api.Core.Queries.StubTester.Version1
{
   using Shared.Version1;

   public class Stub
   {
      public Stub(string stubId, Shared.Version1.Request request, Response response)
      {
         StubId = stubId;
         Request = request;
         Response = response;
      }

      public Shared.Version1.Request Request { get; }
      public Response Response { get; }
      public string StubId { get; }
   }
}