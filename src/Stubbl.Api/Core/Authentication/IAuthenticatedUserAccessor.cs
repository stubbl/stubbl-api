namespace Stubbl.Api.Core.Authentication
{
   using Data.Collections.Members;

   public interface IAuthenticatedUserAccessor
   {
      Member AuthenticatedUser { get; }
   }
}