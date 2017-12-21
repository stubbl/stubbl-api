namespace Stubbl.Api.Models.RoleAlreadyExists.Version1
{
   using Error.Version1;

   public class RoleAlreadyExistsResponse : ErrorResponse
   {
      public RoleAlreadyExistsResponse()
         : base("RoleAlreadyExists", "The role already exists.")
      {
      }
   }
}