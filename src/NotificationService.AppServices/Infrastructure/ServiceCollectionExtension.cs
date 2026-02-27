using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NotificationService.AppServices.Contracts.Emails.Common;
using NotificationService.AppServices.Contracts.Emails.Configuration;
using NotificationService.AppServices.Contracts.Emails.Handlers;
using NotificationService.AppServices.Emails.Handlers;
using NotificationService.AppServices.Emails.Repositories;
using NotificationService.AppServices.Emails.Repositories.Contracts;
using NotificationService.RazorTemplates;
using RazorLight;

// API mapper implementations
using ApiEmailParamNodeMapper = NotificationService.AppServices.Emails.Mappers.Api.EmailParamNodeMapper;
using ApiEmailTemplateParamTypeMapper = NotificationService.AppServices.Emails.Mappers.Api.EmailTemplateParamTypeMapper;
using ApiEmailTemplateTypeMapper = NotificationService.AppServices.Emails.Mappers.Api.EmailTemplateTypeMapper;

// API mapper contracts
using ApiIEmailParamNodeMapper = NotificationService.AppServices.Emails.Mappers.Contracts.Api.IEmailParamNodeMapper;
using ApiIEmailTemplateParamTypeMapper = NotificationService.AppServices.Emails.Mappers.Contracts.Api.IEmailTemplateParamTypeMapper;
using ApiIEmailTemplateTypeMapper = NotificationService.AppServices.Emails.Mappers.Contracts.Api.IEmailTemplateTypeMapper;

// Entity mapper implementations
using EmailMessageEntityMapper = NotificationService.AppServices.Emails.Mappers.Entities.EmailMessageEntityMapper;
using EmailMessageMapper = NotificationService.AppServices.Emails.Mappers.EmailMessageMapper;
using EntityEmailParamNodeMapper = NotificationService.AppServices.Emails.Mappers.Entities.EmailParamNodeMapper;
using EntityEmailTemplateParamTypeMapper = NotificationService.AppServices.Emails.Mappers.Entities.EmailTemplateParamTypeMapper;
using EntityEmailTemplateTypeMapper = NotificationService.AppServices.Emails.Mappers.Entities.EmailTemplateTypeMapper;

// Entity mapper contracts
using EntityIEmailMessageEntityMapper = NotificationService.AppServices.Emails.Mappers.Contracts.Entities.IEmailMessageEntityMapper;
using EntityIEmailParamNodeMapper = NotificationService.AppServices.Emails.Mappers.Contracts.Entities.IEmailParamNodeMapper;
using EntityIEmailTemplateParamTypeMapper = NotificationService.AppServices.Emails.Mappers.Contracts.Entities.IEmailTemplateParamTypeMapper;
using EntityIEmailTemplateTypeMapper = NotificationService.AppServices.Emails.Mappers.Contracts.Entities.IEmailTemplateTypeMapper;
using GetEmailMessageResponseMapper = NotificationService.AppServices.Emails.Mappers.Api.GetEmailMessageResponseMapper;
using IEmailMessageMapper = NotificationService.AppServices.Emails.Mappers.Contracts.IEmailMessageMapper;
using IGetEmailMessageResponseMapper = NotificationService.AppServices.Contracts.Emails.Mappers.Api.IGetEmailMessageResponseMapper;

namespace NotificationService.AppServices.Infrastructure
{
    /// <summary>
    /// Методы расширения для настройки сервисов в контейнере внедрения зависимостей.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Добавляет сервисы приложения в контейнер внедрения зависимостей.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="configuration">Конфигурация приложения.</param>
        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSmtpSettings(configuration);
            services.AddRepositories();
            services.AddHandlers();
            services.AddMappers();
            services.AddRazorLightEngine();
        }

        /// <summary>
        /// Добавляет сервисы мапперов в контейнер внедрения зависимостей.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        public static void AddMappers(this IServiceCollection services)
        {
            // API mappers (Entities -> API contracts)
            services.AddScoped<ApiIEmailParamNodeMapper, ApiEmailParamNodeMapper>();
            services.AddScoped<ApiIEmailTemplateParamTypeMapper, ApiEmailTemplateParamTypeMapper>();
            services.AddScoped<ApiIEmailTemplateTypeMapper, ApiEmailTemplateTypeMapper>();
            services.AddScoped<IGetEmailMessageResponseMapper, GetEmailMessageResponseMapper>();

            services.AddScoped<IEmailMessageMapper, EmailMessageMapper>();

            // Entity mappers (API contracts -> Entities)
            services.AddScoped<EntityIEmailMessageEntityMapper, EmailMessageEntityMapper>();
            services.AddScoped<EntityIEmailParamNodeMapper, EntityEmailParamNodeMapper>();
            services.AddScoped<EntityIEmailTemplateParamTypeMapper, EntityEmailTemplateParamTypeMapper>();
            services.AddScoped<EntityIEmailTemplateTypeMapper, EntityEmailTemplateTypeMapper>();
        }

        private static void AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<ISendEmailHandler, SendEmailHandler>();
            services.AddScoped<IGetEmailsHandler, GetEmailsHandler>();
        }

        private static void AddSmtpSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddSingleton<IOptionsMonitor<SmtpSettings>, OptionsMonitor<SmtpSettings>>();

            services.PostConfigure<SmtpSettings>(options =>
            {
                var envHost = Environment.GetEnvironmentVariable("SMTP_HOST");
                if (!string.IsNullOrWhiteSpace(envHost))
                {
                    options.Host = envHost;
                }

                var envPort = Environment.GetEnvironmentVariable("SMTP_PORT");
                if (!string.IsNullOrWhiteSpace(envPort) && int.TryParse(envPort, out var port))
                {
                    options.Port = port;
                }

                var envUser = Environment.GetEnvironmentVariable("SMTP_USERNAME");
                if (!string.IsNullOrWhiteSpace(envUser))
                {
                    options.Username = envUser;
                }

                var envPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
                if (!string.IsNullOrWhiteSpace(envPassword))
                {
                    options.Password = envPassword;
                }
            });
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEmailRepository, EmailRepository>();
        }

        private static void AddRazorLightEngine(this IServiceCollection services)
        {
            services.AddSingleton<IRazorLightEngine>(_ =>
            {
                var assembly = typeof(Dummy).Assembly;
                return new RazorLightEngineBuilder()
                    .UseEmbeddedResourcesProject(assembly, Names.RazorTemplatesResourceRoot)
                    .UseMemoryCachingProvider()
                    .EnableDebugMode()
                    .Build();
            });
        }
    }
}
