namespace NotificationService.Api.Contracts.Emails.Routes
{
    /// <summary>
    /// Пути для EmailController.
    /// </summary>
    public class EmailRoutes
    {
        /// <summary>
        /// Базовый путь.
        /// </summary>
        public const string BasePath = "notification-service/email";

        /// <summary>
        /// Путь для отправки email.
        /// </summary>
        public const string SendEmailPath = "send";

        /// <summary>
        /// Путь для получения email сообщений.
        /// </summary>
        public const string GetEmailsPath = "get";
    }
}