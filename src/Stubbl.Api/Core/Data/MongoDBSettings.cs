namespace Stubbl.Api.Core.Data
{
    using System;

    public class MongoDBSettings
    {
        public MongoDBSettings(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public string ConnectionString { get; }
    }
}
