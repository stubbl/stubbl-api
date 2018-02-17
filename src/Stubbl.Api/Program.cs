namespace Stubbl.Api
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using MongoDB.Bson;
    using Serilog;
    using Serilog.Core.Enrichers;
    using Serilog.Events;
    using Serilog.Exceptions;
    using System;
    using System.IO;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "stubbl-api";

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
            .CaptureStartupErrors(true)
            .ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                var hostingEnvironment = hostingContext.HostingEnvironment;

                configuration.SetBasePath(hostingEnvironment.ContentRootPath)
                   .AddJsonFile("appsettings.json", true, true)
                   .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true, true)
                   .AddEnvironmentVariables()
                   .AddUserSecrets<Startup>();
            })
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseKestrel()
            .UseIISIntegration()
            .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.MinimumLevel.Is(hostingContext.Configuration.GetValue<LogEventLevel>("Serilog:LogEventLevel"))
                .Destructure.AsScalar<ObjectId>()
                .Enrich.FromLogContext()
                .Enrich.With(new PropertyEnricher("Component", "stubbl-api"))
                .Enrich.With(new PropertyEnricher("Environment", hostingContext.HostingEnvironment.EnvironmentName))
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(hostingContext.Configuration.GetValue<LogEventLevel>("Serilog:LogEventLevel"))
                .WriteTo.Seq
                (
                    hostingContext.Configuration.GetValue<string>("Seq:Url"),
                    apiKey: hostingContext.Configuration.GetValue<string>("Seq:ApiKey"),
                    restrictedToMinimumLevel: hostingContext.Configuration.GetValue<LogEventLevel>("Serilog:LogEventLevel")
                )
            )
            .UseStartup<Startup>()
            .Build();
    }
}