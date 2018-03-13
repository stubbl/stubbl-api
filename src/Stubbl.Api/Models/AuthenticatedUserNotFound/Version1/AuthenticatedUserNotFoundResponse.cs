namespace Stubbl.Api.Models.AuthenticatedUserNotFound.Version1
{
   using Error.Version1;

   public class AuthenticatedUserNotFoundResponse : ErrorResponse
   {
      public AuthenticatedUserNotFoundResponse()
         : base("AuthenticatedUserNotFound", "The authenticted member cannot be found.")
      {
      }
   }
}