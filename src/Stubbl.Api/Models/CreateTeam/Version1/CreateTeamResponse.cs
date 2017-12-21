namespace Stubbl.Api.Models.CreateTeam.Version1
{
   public class CreateTeamResponse
   {
      public CreateTeamResponse(string teamId)
      {
         TeamId = teamId;
      }

      public string TeamId { get; }
   }
}