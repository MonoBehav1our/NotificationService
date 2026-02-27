using NotificationService.Entities.Emails.Types;
using Npgsql;

namespace NotificationService.Entities.Infrastructure
{
    /// <summary>
    /// Класс со статическими методами для настройки ef-postgres.
    /// </summary>
    public static class NpgsqlStaticSettings
    {
        private const string EmailTemplateType = "email_template";

        /// <summary>
        /// Включить маппинг c# enum -> postgres enum.
        /// </summary>
        public static void EnableEnumMapping()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EmailTemplateType>(EmailTemplateType);
        }
    }
}