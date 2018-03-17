using System;
using Gunnsoft.Common.Caching;
using MongoDB.Driver;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Members;
using Stubbl.Api.Exceptions.AuthenticatedUserNotFound.Version1;

namespace Stubbl.Api.Authentication
{
    public class MongoDbAuthenticatedUserAccessor : IAuthenticatedUserAccessor
    {
        private readonly Lazy<Member> _authenticatedUser;

        public MongoDbAuthenticatedUserAccessor(ICache cache, ICacheKey cacheKey,
            IIdentityIdAccessor identityIdAccessor, IMongoCollection<Member> membersCollection)
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