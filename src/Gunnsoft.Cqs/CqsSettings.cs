namespace Gunnsoft.Cqs
{
    public class CqsSettings
    {
        public CqsSettings(string storageConnectionString)
        {
            StorageConnectionString = storageConnectionString;
        }

        public string StorageConnectionString { get; }
    }
}