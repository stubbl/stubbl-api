namespace Stubbl.Api.Models.InvitationNotFound.Version1
{
   using Error.Version1;

   public class InvitationNotFoundResponse : ErrorResponse
   {
      public InvitationNotFoundResponse()
         : base("InvitationNotFound", "The invitation cannot be found.")
      {
      }
   }
}