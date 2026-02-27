using NotificationService.Api.Contracts.Emails.Models;
using NotificationService.Api.Contracts.Emails.Types;

namespace NotificationService.Api.Contracts.Emails.Responses
{
    /// <summary>
    /// Модель ответа для получения email сообщений.
    /// </summary>
    public class GetEmailMessageResponse
    {
        /// <summary>
        /// Автор письма.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Тема письма.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Список получателей.
        /// </summary>
        public List<string> Recipients { get; set; }

        /// <summary>
        /// Шаблон письма.
        /// </summary>
        public EmailTemplateType EmailTemplateType { get; set; }

        /// <summary>
        /// Параметры для шаблона.
        /// </summary>
        public Dictionary<EmailTemplateParamType, EmailParamNode> TemplateParams { get; set; }

        /// <summary>
        /// Время отправки сообщения.
        /// </summary>
        public DateTime SendTime { get; set; }
    }
}