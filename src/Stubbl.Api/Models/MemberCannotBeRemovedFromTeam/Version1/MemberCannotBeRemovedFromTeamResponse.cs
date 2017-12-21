namespace Stubbl.Api.Models.MemberCannotBeRemovedFromTeam.Version1
{
   using Error.Version1;

   public class MemberCannotBeRemovedFromTeamResponse : ErrorResponse
   {
      public MemberCannotBeRemovedFromTeamResponse()
         : base("MemberCannotBeRemovedFromTeam", "The member cannot be removed from the team.")
      {
      }
   }
}