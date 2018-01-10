namespace Stubbl.Api.Core.Data
{
    public static class MongoDBConfig
    {
        static MongoDBConfig()
        {
            CollectionNames = new CollectionNames();
        }

        public static string DatabaseName = "stubbl-api";
        public static CollectionNames CollectionNames { get; }
    }
}