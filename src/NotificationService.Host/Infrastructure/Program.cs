namespace NotificationService.Host.Infrastructure
{
    /// <summary>
    /// Класс запуска приложения.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Метод запуска приложения.
        /// </summary>
        /// <param name="args">Аргументы.</param>
        /// <returns>Задача.</returns>
        public static Task Main(string[] args)
        {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(app =>
                {
                    app.UseStartup<Startup>();
                })
                .Build()
                .RunAsync();
        }
    }
}