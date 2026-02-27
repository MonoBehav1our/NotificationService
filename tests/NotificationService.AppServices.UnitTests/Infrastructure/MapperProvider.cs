using Microsoft.Extensions.DependencyInjection;
using NotificationService.AppServices.Infrastructure;

namespace NotificationService.AppServices.UnitTests.Infrastructure
{
    /// <summary>
    /// Предоставляет доступ к зарегистрированным мапперам для тестирования.
    /// </summary>
    public static class MapperProvider
    {
        private static readonly Lazy<IServiceProvider> _serviceProvider = new(() =>
        {
            var services = new ServiceCollection();
            services.AddMappers();
            return services.BuildServiceProvider();
        });

        /// <summary>
        /// Получает экземпляр маппера из DI контейнера.
        /// </summary>
        /// <typeparam name="T">Тип маппера.</typeparam>
        /// <returns>Экземпляр маппера.</returns>
        public static T GetMapper<T>()
            where T : notnull
        {
            return _serviceProvider.Value.GetRequiredService<T>();
        }
    }
}