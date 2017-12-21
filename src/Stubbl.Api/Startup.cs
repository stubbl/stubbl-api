namespace Stubbl.Api
{
   using Autofac;
   using Autofac.Extensions.DependencyInjection;
   using Common.Smtp;
   using Core.Data;
   using Core.EventHandlers.Cloudflare;
   using FluentValidation;
   using FluentValidation.AspNetCore;
   using IdentityServer4.AccessTokenValidation;
   using Microsoft.AspNetCore.Authorization;
   using Microsoft.AspNetCore.Builder;
   using Microsoft.AspNetCore.Hosting;
   using Microsoft.AspNetCore.Http;
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.AspNetCore.Mvc.ApiExplorer;
   using Microsoft.AspNetCore.Mvc.ApplicationModels;
   using Microsoft.AspNetCore.Mvc.Authorization;
   using Microsoft.AspNetCore.Routing;
   using Microsoft.Extensions.Configuration;
   using Microsoft.Extensions.DependencyInjection;
   using Microsoft.Extensions.Logging;
   using Microsoft.Extensions.PlatformAbstractions;
   using Microsoft.WindowsAzure.Storage;
   using Middleware;
   using Newtonsoft.Json;
   using Serilog;
   using Serilog.Events;
   using Stubbl.Api.Core.Versioning;
   using Swashbuckle.AspNetCore.Swagger;
   using Swashbuckle.AspNetCore.SwaggerGen;
   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Reflection;

   public class Startup
   {
      private readonly static Assembly s_assembly;
      private readonly IConfiguration _configuration;
      private readonly IHostingEnvironment _hostingEnvironment;

      static Startup()
      {
         s_assembly = typeof(Startup).GetTypeInfo().Assembly;
      }

      public Startup(IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
      {
         _hostingEnvironment = hostingEnvironment;

         var configuration = new ConfigurationBuilder()
            .SetBasePath(hostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{_hostingEnvironment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables()
            .AddUserSecrets<Startup>()
            .Build();

         _configuration = configuration;

         MongoDbConfigurator.Configure();

         SerilogConfigurator.Configure
         (
            _configuration.GetValue<LogEventLevel>("Serilog:LogEventLevel"),
            "stubbl-api",
            hostingEnvironment.EnvironmentName,
            _configuration.GetValue<string>("Seq:Url"),
            _configuration.GetValue<string>("Seq:ApiKey")
         );

         loggerFactory.AddSerilog();

         ValidatorOptions.DisplayNameResolver = ValidatorOptions.PropertyNameResolver;
      }

      private static string XmlCommentsFilePath
      {
         get
         {
            string basePath = PlatformServices.Default.Application.ApplicationBasePath;
            string fileName = s_assembly.GetName().Name + ".xml";

            return Path.Combine(basePath, fileName);
         }
      }

      public void Configure(IApplicationBuilder applicationBuilder, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
      {
         if (_hostingEnvironment.IsDevelopment())
         {
            applicationBuilder.UseDeveloperExceptionPage();
         }

         applicationBuilder.UseJsonExceptions();

         if (_hostingEnvironment.IsProduction())
         {
            applicationBuilder.UseSecureRequests();
         }

         if (_hostingEnvironment.IsDevelopment() || _hostingEnvironment.IsIntegrationTesting())
         {
            applicationBuilder.UseFakeUser();
         }

         applicationBuilder.UseAuthentication();

         applicationBuilder.UseStubTester();

         applicationBuilder.UseMvc();

         applicationBuilder.UseSwagger();
         applicationBuilder.UseSwaggerUI(o =>
         {
            o.ConfigureOAuth2
            (
               "stubbl-api",
               "a623eac9-b42a-422a-8856-fbacb6fcb6d9",
               "stubbl-api",
               "stubbl-api"
            );

            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
               o.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
         });
      }

      public IServiceProvider ConfigureServices(IServiceCollection services)
      {
         services.AddApiVersioning(o =>
         {
            o.ApiVersionReader = new AcceptHeaderApiVersionReader();
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(Versions.Latest, 0);
         });

         services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
             .AddIdentityServerAuthentication(options =>
             {
                options.ApiName = _configuration.GetValue<string>("IdentityServer:ApiName");
                options.Authority = _configuration.GetValue<string>("IdentityServer:Authority");
                options.RequireHttpsMetadata = _hostingEnvironment.IsProduction();
             });

         services.AddOptions()
            .Configure<ApiOptions>(o => _configuration.GetSection("Api").Bind(o))
            .Configure<CloudflareOptions>(o => _configuration.GetSection("Cloudflare").Bind(o))
            .Configure<MongoDbOptions>(o => _configuration.GetSection("MongoDb").Bind(o))
            .Configure<SmtpOptions>(o => _configuration.GetSection("Smtp").Bind(o))
            .Configure<StorageOptions>(o => _configuration.GetSection("Storage").Bind(o));

         services.AddMvc(o =>
            {
               o.Conventions.Add(new FromBodyRequiredConvention());
               o.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
            })
            .AddFluentValidation(o =>
            {
               o.ValidatorFactoryType = typeof(ServiceProviderValidatorFactory);
               o.RegisterValidatorsFromAssembly(s_assembly);
            })
            .AddJsonOptions(o =>
            {
               o.SerializerSettings.Converters = JsonConstants.JsonSerializerSettings.Converters;
               o.SerializerSettings.Formatting = JsonConstants.JsonSerializerSettings.Formatting;
            });

         services.AddMvcCore()
            .AddAuthorization()
            .AddJsonFormatters()
            .AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'VVV");

         services.AddMemoryCache();

         services.AddRouting(o =>
         {
            o.AppendTrailingSlash = false;
            o.ConstraintMap.Add("ObjectId", typeof(ObjectIdRouteConstraint));
            o.LowercaseUrls = true;
         });

         services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

         services.AddSwaggerGen(o =>
         {
            o.AddSecurityDefinition("Swagger", new OAuth2Scheme
            {
               Type = "oauth2",
               AuthorizationUrl = $"{_configuration.GetValue<string>("IdentityServer:Authority")}/connect/authorize",
               Flow = "implicit",
               TokenUrl = $"{_configuration.GetValue<string>("IdentityServer:Authority")}/connect/token",
               Scopes = new Dictionary<string, string>
               {
                  { "stubbl-api", "Stubbl API" }
               }
            });
            o.CustomSchemaIds(x => x.FullName);
            o.DescribeAllEnumsAsStrings();
            o.DocumentFilter<LowercaseDocumentOperationFilter>();
            o.OperationFilter<CancellationTokenOperationFilter>();

            if (_hostingEnvironment.IsDevelopment())
            {
               o.OperationFilter<FakeUserOperationFilter>();
            }

            o.OperationFilter<SwaggerDefaultValuesOperationFilter>();

            if (File.Exists(XmlCommentsFilePath))
            {
               o.IncludeXmlComments(XmlCommentsFilePath);
            }

            var provider = services.BuildServiceProvider()
               .GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
               o.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
         });

         var containerBuilder = new ContainerBuilder();
         containerBuilder.RegisterInstance(_configuration)
            .As<IConfiguration>()
            .SingleInstance();
         containerBuilder.RegisterAssemblyModules(s_assembly);
         containerBuilder.Populate(services);
         var container = containerBuilder.Build();
         var serviceProvider = new AutofacServiceProvider(container);

         if (!_hostingEnvironment.IsIntegrationTesting())
         {
            var mongoDbMigrationsRunner = container.Resolve<MongoDbMigrationsRunner>();
            mongoDbMigrationsRunner.Run()
               .Wait();
         }

         return serviceProvider;
      }

      private static Info CreateInfoForApiVersion(ApiVersionDescription description)
      {
         var info = new Info
         {
            Title = $"Stubbl API {description.ApiVersion}",
            Version = description.ApiVersion.ToString()
         };

         if (description.IsDeprecated)
         {
            info.Description += " (deprecated)";
         }

         return info;
      }
   }
}