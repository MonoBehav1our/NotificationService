namespace NotificationService.Api.Contracts.Emails.Types
{
    /// <summary>
    /// Перечень ключей параметров шаблонов email.
    /// </summary>
    public enum EmailTemplateParamType
    {
        /// <summary>
        /// Неопределенный (служебный тип, не использовать).
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Заголовок письма.
        /// </summary>
        Header = 1,

        /// <summary>
        /// Основной текст письма.
        /// </summary>
        Body = 2,

        /// <summary>
        /// Узел кнопки (вложенные параметры хранятся в Childs).
        /// </summary>
        Button = 3,

        /// <summary>
        /// URL для кнопки.
        /// </summary>
        Url = 4,

        /// <summary>
        /// Текст для кнопки.
        /// </summary>
        Text = 5,

        /// <summary>
        /// Код подтверждения.
        /// </summary>
        Code = 6,

        /// <summary>
        /// Время истечения срока действия.
        /// </summary>
        ExpirationTime = 7,
    }
}