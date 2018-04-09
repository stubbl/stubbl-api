using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Gunnsoft.Api;
using Gunnsoft.Api.Authentication;
using Gunnsoft.Api.ExceptionHandlers;
using Gunnsoft.Api.Middleware;
using Gunnsoft.Api.Versioning;
using Gunnsoft.CloudflareApi;
using Gunnsoft.Common.Microsoft.WindowsAzure.Storage;
using Gunnsoft.Cqs.Commands;
using Gunnsoft.Cqs.Events;
using Gunnsoft.Cqs.Queries;
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
using Stubbl.Api.Authentication;
using Stubbl.Api.Caching;
using Stubbl.Api.Data;
using Stubbl.Api.Middleware;
using Stubbl.Api.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Stubbl.Api
{
    public class Startup
    {
        private static readonly Assembly s_assembly;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        static Startup()
        {
            s_assembly = typeof(Startup).GetTypeInfo().Assembly;
        }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

            ValidatorOptions.DisplayNameResolver = ValidatorOptions.PropertyNameResolver;
        }

        private static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = s_assembly.GetName().Name + ".xml";

                return Path.Combine(basePath, fileName);
            }
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            app.UseJsonExceptions();

            if (_hostingEnvironment.IsProduction())
            {
                app.UseSecureRequests();
            }

            if (_hostingEnvironment.IsDevelopment() || _hostingEnvironment.IsEnvironment("IntegrationTesting"))
            {
                app.UseSubHeader();
            }

            app.UseAuthentication();

            app.UseStub();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.OAuthClientId("stubbl-api-swagger");
                o.OAuthAppName("Stubbl API Swagger");

                foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    o.SwaggerEndpoint($"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                        apiVersionDescription.GroupName.ToUpperInvariant());
                }
            });
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var identityServerAuthority = _configuration.GetValue<string>("IdentityServer:Authority");

            services.AddApiVersioning(o =>
            {
                o.ApiVersionReader = new AcceptHeaderApiVersionReader();
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(o =>
                {
                    o.ApiName = "stubbl-api";
                    o.Authority = identityServerAuthority;
                    o.RequireHttpsMetadata = _hostingEnvironment.IsProduction();
                });

            services.AddMvc(o =>
                {
                    o.Conventions.Add(new FromBodyRequiredConvention());
                    o.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                        .Build()));
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

            services.AddOptions()
                .Configure<CloudflareOptions>(o => _configuration.GetSection("Cloudflare").Bind(o))
                .Configure<StubblApiOptions>(o => _configuration.GetSection("StubblApi").Bind(o));

            services.AddRouting(o =>
            {
                o.AppendTrailingSlash = false;
                o.ConstraintMap.Add("ObjectId", typeof(ObjectIdRouteConstraint));
                o.LowercaseUrls = true;
            });

            services.AddSwaggerGen(o =>
            {
                o.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    AuthorizationUrl = $"{identityServerAuthority}/connect/authorize",
                    Flow = "implicit",
                    Scopes = new Dictionary<string, string>
                        {
                            {"stubbl-api", "Stubbl API"}
                        },
                    TokenUrl = $"{identityServerAuthority}/connect/token",
                    Type = "oauth2"
                });

                o.CustomSchemaIds(x => x.FullName);
                o.DescribeAllEnumsAsStrings();
                o.OperationFilter<AuthorizeOperationFilter>();
                o.OperationFilter<CancellationTokenOperationFilter>();

                if (_hostingEnvironment.IsDevelopment())
                {
                    o.OperationFilter<SubHeaderOperationFilter>();
                }

                o.OperationFilter<SwaggerDefaultValuesOperationFilter>();

                if (File.Exists(XmlCommentsFilePath))
                {
                    o.IncludeXmlComments(XmlCommentsFilePath);
                }

                var buildServiceProvider = services.BuildServiceProvider()
                    .GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var apiVersionDescription in buildServiceProvider.ApiVersionDescriptions)
                {
                    o.SwaggerDoc(apiVersionDescription.GroupName, CreateInfoForApiVersion(apiVersionDescription));
                }
            });

            services.AddSingleton(_configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(sp =>
                new HttpClient(new LoggingHandler(new HttpClientHandler(),
                    sp.GetRequiredService<ILogger<LoggingHandler>>())));

            var containerBuilder = new ContainerBuilder();

            containerBuilder.AddCaching()
                .AddCloudflareApi(new CloudflareApiSettings
                (
                    _configuration.GetValue<string>("CloudflareApi:BaseUrl"),
                    _configuration.GetValue<string>("CloudflareApi:AuthenticationKey"),
                    _configuration.GetValue<string>("CloudflareApi:AuthenticationEmailAddress")
                ))
                .AddCommandDispatcher()
                .AddCommandHandlerDecorators()
                .AddCommandHandlers()
                .AddEventDispatcher()
                .AddEventHandlerDecorators()
                .AddEventHandlers()
                .AddExceptionHandlers()
                .AddMongo(new MongoSettings
                (
                    _configuration.GetValue<string>("MongoDB:ConnectionString")
                ))
                .AddMongoUserAccessor()
                .AddQueryDispatcher()
                .AddQueryHandlerDecorators()
                .AddQueryHandlers()
                .AddStorageAccount(_configuration.GetValue<string>("StorageAccount:ConnectionString"))
                .AddSubAccessor();

            containerBuilder.RegisterAssemblyModules(s_assembly);
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);

            if (_hostingEnvironment.IsEnvironment("IntegrationTesting"))
            {
                return serviceProvider;
            }

            var mongoDbMigrationsRunner = container.Resolve<MongoMigrationsRunner>();
            mongoDbMigrationsRunner.RunAsync()
                .Wait();

            return serviceProvider;
        }

        private static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info
            {
                Title = $"Stubbl API v{description.ApiVersion}",
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