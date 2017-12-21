namespace Stubbl.Api.Models.RoleCannotBeUpdated.Version1
{
   using Error.Version1;

   public class RoleCannotBeUpdatedResponse : ErrorResponse
   {
      public RoleCannotBeUpdatedResponse()
         : base("RoleNotFound", "The role cannot be updated.")
      {
      }
   }
}