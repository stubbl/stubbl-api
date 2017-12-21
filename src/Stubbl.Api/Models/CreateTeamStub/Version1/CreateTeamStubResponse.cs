namespace Stubbl.Api.Models.CreateTeamStub.Version1
{
   public class CreateTeamStubResponse
   {
      public CreateTeamStubResponse(string stubId)
      {
         StubId = stubId;
      }

      public string StubId { get; }
   }
}