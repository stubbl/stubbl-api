namespace Stubbl.Api.Models.MemberCannotManageStubs.Version1
{
   using Error.Version1;

   public class MemberCannotManageStubsResponse : ErrorResponse
   {
      public MemberCannotManageStubsResponse()
         : base("MemberCannotManageStubs", "The member cannot manage stubs.")
      {
      }
   }
}