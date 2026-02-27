using NotificationService.Api.Contracts.Emails.Models;
using NotificationService.Api.Contracts.Emails.Types;
using NotificationService.RazorTemplates.TemplateModels.Validators.Contracts;

namespace NotificationService.RazorTemplates.TemplateModels.Validators
{
    /// <summary>
    /// Валидатор модели параметров для шаблона письма <c>ResetPassword</c>.
    /// </summary>
    internal class ResetPasswordEmailModelValidator : IEmailModelValidator
    {
        /// <summary>
        /// Проверяет корректность набора параметров шаблона письма.
        /// </summary>
        /// <param name="templateParams">Словарь параметров шаблона.</param>
        /// <returns><c>true</c>, если все обязательные параметры присутствуют и валидны; иначе <c>false</c>.</returns>
        public bool Validate(Dictionary<EmailTemplateParamType, EmailParamNode> templateParams)
        {
            if (!templateParams.TryGetValue(EmailTemplateParamType.Code, out var code) || string.IsNullOrEmpty(code.Value))
            {
                return false;
            }

            if (!templateParams.TryGetValue(EmailTemplateParamType.ExpirationTime, out var expirationTime) || string.IsNullOrEmpty(expirationTime.Value))
            {
                return false;
            }

            return true;
        }
    }
}
