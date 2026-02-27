namespace NotificationService.Api.Contracts.Emails.Types
{
    /// <summary>
    /// Варианты названий шаблонов email сообщения.
    /// </summary>
    public enum EmailTemplateType
    {
        /// <summary>
        /// Неопределенный (служебный тип, не использовать).
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Шаблон с кнопкой.
        /// </summary>
        WithButton = 1,

        /// <summary>
        /// Шаблон для восстановления пароля.
        /// </summary>
        ResetPassword = 2,

        /// <summary>
        /// Шаблон для подтверждения почты.
        /// </summary>
        EmailConfirmation = 3,
    }
}