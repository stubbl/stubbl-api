using System;

namespace Stubbl.Api.Data
{
    public class MongoSettings
    {
        public MongoSettings(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public string ConnectionString { get; }
    }
}