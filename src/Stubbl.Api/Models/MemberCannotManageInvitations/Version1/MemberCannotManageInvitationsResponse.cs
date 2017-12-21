namespace Stubbl.Api.Models.MemberCannotManageInvitations.Version1
{
   using Error.Version1;

   public class MemberCannotManageInvitationsResponse : ErrorResponse
   {
      public MemberCannotManageInvitationsResponse()
         : base("MemberCannotInvitationsMembers", "The member cannot manage invitations.")
      {
      }
   }
}