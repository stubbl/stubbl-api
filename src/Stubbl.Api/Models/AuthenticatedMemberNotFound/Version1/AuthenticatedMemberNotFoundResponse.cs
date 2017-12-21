namespace Stubbl.Api.Models.AuthenticatedMemberNotFound.Version1
{
   using Error.Version1;

   public class AuthenticatedMemberNotFoundResponse : ErrorResponse
   {
      public AuthenticatedMemberNotFoundResponse()
         : base("AuthenticatedMemberNotFound", "The authenticted member cannot be found.")
      {
      }
   }
}