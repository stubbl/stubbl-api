namespace Stubbl.Api.Models.MemberNotInvitedToTeam.Version1
{
   using Error.Version1;

   public class MemberNotInvitedToTeamResponse : ErrorResponse
   {
      public MemberNotInvitedToTeamResponse()
         : base("MemberNotInvitedToTeam", "The member hasn't been invited to the team.")
      {
      }
   }
}