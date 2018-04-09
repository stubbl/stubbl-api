using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Func<User> _authenticatedUserFactory;
        private Lazy<User> _authenticatedUser;

        public MongoAuthenticatedUserAccessor(ICache cache, ICacheKey cacheKey,
            IReadOnlyCollection<ISubAccessor> subAccessors, IMongoCollection<User> usersCollection)
        {
            _authenticatedUserFactory = () =>
            {
                var sub = subAccessors.FirstOrDefault(sa => sa.Sub != null)?.Sub;

                if (sub == null)
                {
                    throw new UnknownSubException();
                }

                var user = cache.GetOrSet
                (
                    cacheKey.FindAuthenticatedUser(sub),
                    () => usersCollection.Find(m => m.Sub == sub)
                        .SortByDescending(u => u.Id)
                        .FirstOrDefault()
                );

                if (user == null)
                {
                    throw new AuthenticatedUserNotFoundException(sub);
                }

                return user;
            };

            _authenticatedUser = new Lazy<User>(_authenticatedUserFactory);
        }

        public User AuthenticatedUser => _authenticatedUser.Value;

        public void Reload()
        {
            _authenticatedUser = new Lazy<User>(_authenticatedUserFactory);
        }
    }
}