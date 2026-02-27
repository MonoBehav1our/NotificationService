using DbUp;
using Microsoft.Extensions.Configuration;
using NotificationService.Migrations.PostgreSql.Extensions;

namespace NotificationService.Migrations.PostgreSql
{
    /// <summary>
    /// Класс старта приложения.
    /// </summary>
    public class Program
    {
        private const string DatabaseConnectionStringSection = "DatabaseConnectionString";
        private const string DefaultConnection = "DefaultConnection";
        private const string Scripts = "Scripts";

        /// <summary>
        /// Метод старта приложения.
        /// </summary>
        /// <param name="args">Параметры.</param>
        public static void Main(string[] args)
        {
            var configuration = LoadConfiguration();

            var connectionString = configuration.GetSection(DatabaseConnectionStringSection)[DefaultConnection].ReplaceEnvironmentVariables();

            EnsureSchemaExists(connectionString, "notification_service");

            DeployDatabase(connectionString);
        }

        private static IConfiguration LoadConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var appSettingsFile = environment switch
            {
                "Development" => "appsettings.Development.json",
                _ => "appsettings.json",
            };

            try
            {
                return new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile(appSettingsFile, optional: false, reloadOnChange: true)
                    .AddJsonFile(appSettingsFile, optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки конфигурации: {ex.Message}");
                throw;
            }
        }

        private static void DeployDatabase(string connectionString)
        {
            DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsFromFileSystem(Scripts)
                .JournalToPostgresqlTable("notification_service", "schemaversions")
                .LogToConsole()
                .Build()
                .PerformUpgrade();
        }

        private static void EnsureSchemaExists(string connectionString, string schemaName)
        {
            using var connection = new Npgsql.NpgsqlConnection(connectionString);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = $"CREATE SCHEMA IF NOT EXISTS {schemaName};";
            cmd.ExecuteNonQuery();
        }
    }
}