using System;
using System.Threading.Tasks;
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
        private Lazy<User> _authenticatedUser;
        private readonly Func<User> _authenticatedUserFactory;

        public MongoAuthenticatedUserAccessor(ICache cache, ICacheKey cacheKey,
            ISubAccessor subAccessor, IMongoCollection<User> usersCollection)
        {
            _authenticatedUserFactory = () =>
            {
                if (subAccessor.Sub == null)
                {
                    throw new UnknownSubException();
                }

                var user = cache.GetOrSet
                (
                    cacheKey.FindAuthenticatedUser(subAccessor.Sub),
                    () => usersCollection.Find(m => m.Sub == subAccessor.Sub)
                        .SortByDescending(u => u.Id)
                        .FirstOrDefault()
                );

                if (user == null)
                {
                    throw new AuthenticatedUserNotFoundException(subAccessor.Sub);
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