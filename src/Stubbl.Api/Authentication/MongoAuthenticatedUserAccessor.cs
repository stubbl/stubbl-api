using System;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Api.Exceptions.AuthenticatedUserNotFound.Version1;
using Gunnsoft.Api.Exceptions.UnknownSub.Version1;
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
            ISubAccessor subAccessor, IMongoCollection<Member> membersCollection)
        {
            _authenticatedUser = new Lazy<Member>(() =>
            {
                if (subAccessor.Sub == null)
                {
                    throw new UnknownSubException();
                }

                var member = cache.GetOrSet
                (
                    cacheKey.FindAuthenticatedUser(subAccessor.Sub),
                    () => membersCollection.Find(m => m.Sub == subAccessor.Sub)
                        .SingleOrDefault()
                );

                if (member == null)
                {
                    throw new AuthenticatedUserNotFoundException(subAccessor.Sub);
                }

                return member;
            });
        }

        public Member AuthenticatedUser => _authenticatedUser.Value;
    }
}