using System;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Api.Exceptions.AuthenticatedUserNotFound.Version1;
using Gunnsoft.Api.Exceptions.UnknownSub.Version1;
using Gunnsoft.Common.Caching;
using MongoDB.Driver;
using Stubbl.Api.Caching;
using Stubbl.Api.Data.Collections.Users;

namespace Stubbl.Api.Authentication
{
    public class MongoAuthenticatedUserAccessor : IAuthenticatedUserAccessor
    {
        private readonly Lazy<User> _authenticatedUser;

        public MongoAuthenticatedUserAccessor(ICache cache, ICacheKey cacheKey,
            ISubAccessor subAccessor, IMongoCollection<User> usersCollection)
        {
            _authenticatedUser = new Lazy<User>(() =>
            {
                if (subAccessor.Sub == null)
                {
                    throw new UnknownSubException();
                }

                var user = cache.GetOrSet
                (
                    cacheKey.FindAuthenticatedUser(subAccessor.Sub),
                    () => usersCollection.Find(m => m.Sub == subAccessor.Sub)
                        .SingleOrDefault()
                );

                if (user == null)
                {
                    throw new AuthenticatedUserNotFoundException(subAccessor.Sub);
                }

                return user;
            });
        }

        public User AuthenticatedUser => _authenticatedUser.Value;
    }
}