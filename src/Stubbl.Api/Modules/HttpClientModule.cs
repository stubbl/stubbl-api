namespace Stubbl.Api.Modules
{
   using System.Net.Http;
   using Autofac;
   using Microsoft.Extensions.Logging;

   public class HttpClientModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.Register(cc => new HttpClient(new LoggingHandler(new HttpClientHandler(), cc.Resolve<ILogger<LoggingHandler>>())))
            .AsSelf()
            .SingleInstance();
      }
   }
}
