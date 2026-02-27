namespace NotificationService.AppServices.Contracts.Emails.Configuration
{
    /// <summary>
    /// Класс который представляет настройки для smtp.
    /// </summary>
    public class SmtpSettings
    {
        /// <summary>
        /// Хост.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Порт.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }
    }
}