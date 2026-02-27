using NotificationService.Api.Contracts.Emails.Models;
using NotificationService.Api.Contracts.Emails.Types;
using NotificationService.RazorTemplates.TemplateModels.Validators.Contracts;

namespace NotificationService.RazorTemplates.TemplateModels.Validators
{
    /// <summary>
    /// Валидатор модели параметров для шаблона письма <c>WithButton</c>.
    /// </summary>
    internal class WithButtonEmailModelValidator : IEmailModelValidator
    {
        /// <summary>
        /// Проверяет корректность набора параметров шаблона письма.
        /// </summary>
        /// <param name="templateParams">Словарь параметров шаблона.</param>
        /// <returns><c>true</c>, если все обязательные параметры присутствуют и валидны; иначе <c>false</c>.</returns>
        public bool Validate(Dictionary<EmailTemplateParamType, EmailParamNode> templateParams)
        {
            if (!templateParams.TryGetValue(EmailTemplateParamType.Header, out var header) || string.IsNullOrEmpty(header.Value))
            {
                return false;
            }

            if (!templateParams.TryGetValue(EmailTemplateParamType.Body, out var body) || string.IsNullOrEmpty(body.Value))
            {
                return false;
            }

            if (!templateParams.TryGetValue(EmailTemplateParamType.Button, out var button) || button.Childs == null)
            {
                return false;
            }

            if (!button.Childs.TryGetValue(EmailTemplateParamType.Url, out var url) || string.IsNullOrEmpty(url.Value))
            {
                return false;
            }

            if (!button.Childs.TryGetValue(EmailTemplateParamType.Text, out var text) || string.IsNullOrEmpty(text.Value))
            {
                return false;
            }

            return true;
        }
    }
}