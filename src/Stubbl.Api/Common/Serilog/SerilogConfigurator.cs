namespace Serilog
{
   using MongoDB.Bson;
   using Serilog.Core.Enrichers;
   using Serilog.Events;
   using Serilog.Exceptions;

   public static class SerilogConfigurator
   {
      public static void Configure(LogEventLevel logEventLevel, string componentName, string environmentName, string seqUrl, string seqApiKey)
      {
         var loggerCongiruation = new LoggerConfiguration()
            .MinimumLevel.Is(logEventLevel)
            .Destructure.AsScalar<ObjectId>()
            .Enrich.FromLogContext()
            .Enrich.With(new PropertyEnricher("Component", componentName))
            .Enrich.With(new PropertyEnricher("Environment", environmentName))
            .Enrich.WithExceptionDetails()
            .WriteTo.Console(restrictedToMinimumLevel: logEventLevel);

         if (!string.IsNullOrWhiteSpace(seqUrl))
         {
            loggerCongiruation.WriteTo.Seq(seqUrl, apiKey: seqApiKey, restrictedToMinimumLevel: logEventLevel);
         }

         Log.Logger = loggerCongiruation.CreateLogger();
      }
   }
}
