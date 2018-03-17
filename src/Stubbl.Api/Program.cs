using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core.Enrichers;
using Serilog.Events;
using Serilog.Exceptions;

namespace Stubbl.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "stubbl-api";

            BuildWebHost(Directory.GetCurrentDirectory(), args).Run();
        }

        public static IWebHost BuildWebHost(string appRootPath, string[] args)
        {
            return GetWebHostBuilder(appRootPath, args).Build();
        }

        public static IWebHostBuilder GetWebHostBuilder(string appRootPath, string[] args = null)
        {
            return WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(true)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    var hostingEnvironment = hostingContext.HostingEnvironment;

                    configuration.AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables()
                        .AddUserSecrets<Startup>();
                })
                .UseContentRoot(appRootPath)
                .UseKestrel()
                .UseIISIntegration()
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.MinimumLevel
                        .Is(hostingContext.Configuration.GetValue<LogEventLevel>("Serilog:LogEventLevel"))
                        .Enrich.FromLogContext()
                        .Enrich.With(new PropertyEnricher("Component", "stubbl-identity"))
                        .Enrich.With(new PropertyEnricher("Environment",
                            hostingContext.HostingEnvironment.EnvironmentName))
                        .Enrich.WithExceptionDetails()
                        .WriteTo.Console(hostingContext.Configuration.GetValue<LogEventLevel>("Serilog:LogEventLevel"));

                    var seqUrl = hostingContext.Configuration.GetValue<string>("Seq:Url");

                    if (!string.IsNullOrWhiteSpace(seqUrl))
                    {
                        loggerConfiguration.WriteTo.Seq
                        (
                            seqUrl,
                            apiKey: hostingContext.Configuration.GetValue<string>("Seq:ApiKey"),
                            restrictedToMinimumLevel: hostingContext.Configuration.GetValue<LogEventLevel>(
                                "Serilog:LogEventLevel")
                        );
                    }
                })
                .UseStartup<Startup>();
        }
    }
}