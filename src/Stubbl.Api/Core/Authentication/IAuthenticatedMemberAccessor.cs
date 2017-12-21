namespace Stubbl.Api.Core.Authentication
{
   using Data.Collections.Members;

   public interface IAuthenticatedMemberAccessor
   {
      Member AuthenticatedMember { get; }
   }
}