using NotificationService.Api.Contracts.Emails.Models;
using NotificationService.Api.Contracts.Emails.Types;

namespace NotificationService.Api.Contracts.Emails.Requests
{
    /// <summary>
    /// Запрос на отправку email сообщения.
    /// </summary>
    public class SendEmailRequest
    {
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
    }
}
