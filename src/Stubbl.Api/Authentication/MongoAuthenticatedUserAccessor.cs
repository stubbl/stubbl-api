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
        private readonly ICache _cache;
        private readonly ICacheKey _cacheKey;
        private readonly ISubAccessor _subAccessor;
        private User _user;
        private readonly IMongoCollection<User> _usersCollection;

        public MongoAuthenticatedUserAccessor(ICache cache, ICacheKey cacheKey,
            ISubAccessor subAccessor, IMongoCollection<User> usersCollection)
        {
            _cache = cache;
            _cacheKey = cacheKey;
            _subAccessor = subAccessor;
            _usersCollection = usersCollection;
        }

        public User AuthenticatedUser
        {
            get
            {
                if (_user != null)
                {
                    return _user;
                }

                if (_subAccessor.Sub == null)
                {
                    throw new UnknownSubException();
                }

                _user = _cache.GetOrSet
                (
                    _cacheKey.FindAuthenticatedUser(_subAccessor.Sub),
                    () => _usersCollection.Find(m => m.Sub == _subAccessor.Sub)
                        .SingleOrDefault()
                );

                if (_user == null)
                {
                    throw new AuthenticatedUserNotFoundException(_subAccessor.Sub);
                }

                return _user;
            }
        }
    }
}