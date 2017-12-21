namespace Stubbl.Api.Core.Authentication
{
   using System;
   using Caching;
   using Common.Caching;
   using Data.Collections.Members;
   using Exceptions.AuthenticatedMemberNotFound.Version1;
   using MongoDB.Driver;

    public class MongoDbAuthenticatedMemberAccessor : IAuthenticatedMemberAccessor
   {
      private readonly Lazy<Member> _authenticatedMember;

      public MongoDbAuthenticatedMemberAccessor(ICache cache, ICacheKey cacheKey, IIdentityIdAccessor identityIdAccessor, IMongoCollection<Member> membersCollection)
      {
         _authenticatedMember = new Lazy<Member>(() =>
         {
            if (identityIdAccessor.IdentityId == null)
            {
               throw new UnknownIdentityIdException();
            }

            var member = cache.GetOrSet
            (
               cacheKey.FindAuthenticatedMember(identityIdAccessor.IdentityId), 
               () => membersCollection.Find(m => m.IdentityId == identityIdAccessor.IdentityId)
                  .SingleOrDefault()
            );

            if (member == null)
            {
               throw new AuthenticatedMemberNotFoundException(identityIdAccessor.IdentityId);
            }
            
            return member;
         });
      }

      public Member AuthenticatedMember => _authenticatedMember.Value;
   }
}