using Autofac;
using Microsoft.WindowsAzure.Storage;

namespace Gunnsoft.Common.Microsoft.WindowsAzure.Storage
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddStorageAccount(this ContainerBuilder extended, string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            extended.RegisterInstance(storageAccount)
                .AsSelf()
                .SingleInstance();

            return extended;
        }
    }
}