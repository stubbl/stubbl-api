using System;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Api.Exceptions.AuthenticatedUserNotFound.Version1;
using Gunnsoft.Api.Exceptions.UnknownIdentityId.Version1;
using Gunnsoft.Common.Caching;
using MongoDB.Driver;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Members;

namespace Stubbl.Api.Authentication
{
    public class MongoAuthenticatedUserAccessor : IAuthenticatedUserAccessor
    {
        private readonly Lazy<Member> _authenticatedUser;

        public MongoAuthenticatedUserAccessor(ICache cache, ICacheKey cacheKey,
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