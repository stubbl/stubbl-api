namespace Stubbl.Api.Models.MemberNotFound.Version1
{
   using Error.Version1;

   public class MemberNotFoundResponse : ErrorResponse
   {
      public MemberNotFoundResponse()
         : base("MemberNotFound", "The member cannot be found.")
      {
      }
   }
}