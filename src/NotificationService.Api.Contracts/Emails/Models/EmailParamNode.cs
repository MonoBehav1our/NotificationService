using NotificationService.Api.Contracts.Emails.Types;

namespace NotificationService.Api.Contracts.Emails.Models
{
    /// <summary>
    /// Рекурсивный узел параметров для email шаблона (API модель).
    /// </summary>
    public class EmailParamNode
    {
        /// <summary>
        /// Значение параметра(если есть).
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Дочерние элементы(если есть).
        /// </summary>
        public Dictionary<EmailTemplateParamType, EmailParamNode>? Childs { get; set; }
    }
}
