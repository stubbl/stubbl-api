namespace Stubbl.Api.Core.Data
{
    using System;

    public class MongoSettings
    {
        public MongoSettings(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public string ConnectionString { get; }
    }
}
