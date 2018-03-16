namespace Stubbl.Api.Core.Authentication
{
   using System;
   using Caching;
   using CodeContrib.Caching;
   using Data.Collections.Members;
   using Exceptions.AuthenticatedUserNotFound.Version1;
   using MongoDB.Driver;

    public class MongoDBAuthenticatedUserAccessor : IAuthenticatedUserAccessor
   {
      private readonly Lazy<Member> _authenticatedUser;

      public MongoDBAuthenticatedUserAccessor(ICache cache, ICacheKey cacheKey, IIdentityIdAccessor identityIdAccessor, IMongoCollection<Member> membersCollection)
      {
         _authenticatedUser = new Lazy<Member>(() =>
         {
            if (identityIdAccessor.IdentityId == null)
            {
               throw new UnknownIdentityIdException();
            }

            var member = cache.GetOrSet
            (
               cacheKey.FindAuthenticatedUser(identityIdAccessor.IdentityId), 
               () => membersCollection.Find(m => m.IdentityId == identityIdAccessor.IdentityId)
                  .SingleOrDefault()
            );

            if (member == null)
            {
               throw new AuthenticatedUserNotFoundException(identityIdAccessor.IdentityId);
            }
            
            return member;
         });
      }

      public Member AuthenticatedUser => _authenticatedUser.Value;
   }
}