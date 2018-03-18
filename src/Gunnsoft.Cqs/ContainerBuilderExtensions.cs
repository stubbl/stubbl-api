using Autofac;

namespace Gunnsoft.Cqs
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddCqsSettings(this ContainerBuilder extended, CqsSettings cqsSettings)
        {
            extended.RegisterInstance(cqsSettings)
                .AsSelf()
                .SingleInstance();

            return extended;
        }
    }
}
