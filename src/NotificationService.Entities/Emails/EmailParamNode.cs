using NotificationService.Entities.Emails.Types;

namespace NotificationService.Entities.Emails
{
    /// <summary>
    /// Рекурсивный узел параметров для email шаблона.
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
