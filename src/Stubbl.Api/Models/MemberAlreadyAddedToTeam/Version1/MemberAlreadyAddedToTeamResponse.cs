namespace Stubbl.Api.Models.MemberAlreadyAddedToTeam.Version1
{
   using Error.Version1;

   public class MemberAlreadyAddedToTeamResponse : ErrorResponse
   {
      public MemberAlreadyAddedToTeamResponse()
         : base("MemberAlreadyAddedToTeam", "The member has already been added to the team.")
      {
      }
   }
}