namespace Stubbl.Api.Models.MemberCannotManageTeams.Version1
{
   using Error.Version1;

   public class MemberCannotManageTeamsResponse : ErrorResponse
   {
      public MemberCannotManageTeamsResponse()
         : base("MemberCannotManageTeams", "The member cannot manage teams.")
      {
      }
   }
}