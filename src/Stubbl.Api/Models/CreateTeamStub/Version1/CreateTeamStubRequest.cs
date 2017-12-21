namespace Stubbl.Api.Models.CreateTeamStub.Version1
{
   using System.Collections.Generic;
   using Shared.Version1;

   public class CreateTeamStubRequest
   {
      public CreateTeamStubRequest()
      {
         Request = new Request();
         Response = new Response();
      }

      public string Name { get; set; }
      public Request Request { get; set; }
      public Response Response { get; set; }
      public IReadOnlyCollection<string> Tags { get; set; }
   }
}