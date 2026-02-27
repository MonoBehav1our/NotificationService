using NotificationService.Api.Contracts.Emails.Models;
using NotificationService.Api.Contracts.Emails.Types;

namespace NotificationService.RazorTemplates.TemplateModels.Validators.Contracts
{
    /// <summary>
    /// Контракт валидатора параметров модели email-шаблона.
    /// </summary>
    public interface IEmailModelValidator
    {
        /// <summary>
        /// Проверяет корректность параметров модели шаблона письма.
        /// </summary>
        /// <param name="templateParams">Словарь параметров шаблона.</param>
        /// <returns><c>true</c>, если параметры валидны; иначе <c>false</c>.</returns>
        public bool Validate(Dictionary<EmailTemplateParamType, EmailParamNode> templateParams);
    }
}