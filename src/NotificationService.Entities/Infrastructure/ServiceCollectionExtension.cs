using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Entities.Contexts;

namespace NotificationService.Entities.Infrastructure
{
    /// <summary>
    /// Класс, предоставляющий методы для регистрации зависимостей в контейнере DI.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        private const string _databaseConnectionSection = "DatabaseConnectionString";
        private const string _defaultConnection = "DefaultConnection";

        /// <summary>
        /// Зарегистрировать контекст проекта в DI.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <returns>Коллекция сервисов с зарегистрированным контекстом.</returns>
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseConnectionString = configuration
                .GetSection(_databaseConnectionSection)[_defaultConnection] ?? throw new ArgumentNullException(nameof(_databaseConnectionSection));

            services.AddDbContext<WelwiseGamesDbContext>(
                options => options.UseNpgsql(databaseConnectionString.ReplaceEnvironmentVariables()));

            return services;
        }
    }
}
