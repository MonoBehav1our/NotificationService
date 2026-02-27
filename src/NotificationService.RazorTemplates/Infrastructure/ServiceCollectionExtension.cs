using Microsoft.Extensions.DependencyInjection;
using NotificationService.RazorTemplates.Handlers;
using NotificationService.RazorTemplates.Handlers.Contracts;

namespace NotificationService.RazorTemplates.Infrastructure
{
    /// <summary>
    /// Методы расширения для регистрации валидаторов шаблонов писем.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Регистрирует службы валидаторов моделей писем в контейнере DI.
        /// </summary>
        /// <param name="services">Коллекция служб.</param>
        public static void AddEmailModelsValidator(this IServiceCollection services)
        {
            services.AddScoped<IEmailValidationHandler, EmailValidationHandler>();
        }
    }
}