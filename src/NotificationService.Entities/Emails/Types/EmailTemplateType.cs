using NpgsqlTypes;

namespace NotificationService.Entities.Emails.Types
{
    /// <summary>
    /// Варианты названий шаблонов email сообщения.
    /// </summary>
    [PgName("email_template")]
    public enum EmailTemplateType
    {
        /// <summary>
        /// Неопределенный (служебный тип, не использовать).
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Шаблон с кнопкой.
        /// </summary>
        [PgName("with_button")]
        WithButton = 1,

        /// <summary>
        /// Шаблон для восстановления пароля.
        /// </summary>
        [PgName("reset_password")]
        ResetPassword = 2,

        /// <summary>
        /// Шаблон для подтверждения почты.
        /// </summary>
        [PgName("email_confirmation")]
        EmailConfirmation = 3,
    }
}
