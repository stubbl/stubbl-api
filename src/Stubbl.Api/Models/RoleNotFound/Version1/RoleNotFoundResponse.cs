namespace Stubbl.Api.Models.RoleNotFound.Version1
{
   using Error.Version1;

   public class RoleNotFoundResponse : ErrorResponse
   {
      public RoleNotFoundResponse()
         : base("RoleNotFound", "The role cannot be found.")
      {
      }
   }
}