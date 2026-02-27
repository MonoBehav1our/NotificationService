namespace NotificationService.AppServices.Contracts.Emails.Common
{
    /// <summary>
    /// Имена и константы для email-сервиса.
    /// </summary>
    public static class Names
    {
        /// <summary>
        /// Имя автора письма по умолчанию.
        /// </summary>
        public const string MessageAuthorName = "Welwise Games";

        /// <summary>
        /// Корневой путь к встроенным ресурсам razor-шаблонов.
        /// </summary>
        public const string RazorTemplatesResourceRoot = "NotificationService.RazorTemplates.EmailTemplates";

        /// <summary>
        /// Название параметра "получатели" при заполнении email сообщения.
        /// </summary>
        public const string RecipientsParamName = "Recipient";
    }
}