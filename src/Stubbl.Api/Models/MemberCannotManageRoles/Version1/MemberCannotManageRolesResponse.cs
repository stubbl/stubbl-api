namespace Stubbl.Api.Models.MemberCannotManageRoles.Version1
{
   using Error.Version1;

   public class MemberCannotManageRolesResponse : ErrorResponse
   {
      public MemberCannotManageRolesResponse()
         : base("MemberCannotManageRoles", "The member cannot manage roles.")
      {
      }
   }
}