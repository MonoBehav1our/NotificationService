using NotificationService.Api.Contracts.Emails.Types;
using NotificationService.AppServices.Contracts.Emails.Models;
using NotificationService.RazorTemplates.Handlers.Contracts;
using NotificationService.RazorTemplates.TemplateModels.Validators;
using NotificationService.RazorTemplates.TemplateModels.Validators.Contracts;

namespace NotificationService.RazorTemplates.Handlers
{
    /// <summary>
    /// Обработчик валидации моделей шаблонов писем.
    /// </summary>
    internal class EmailValidationHandler : IEmailValidationHandler
    {
        /// <summary>
        /// Реестр валидаторов по типу шаблона письма.
        /// </summary>
        private readonly Dictionary<EmailTemplateType, IEmailModelValidator> _validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailValidationHandler"/> class.
        /// </summary>
        public EmailValidationHandler()
        {
            _validators = new Dictionary<EmailTemplateType, IEmailModelValidator>
            {
                { EmailTemplateType.WithButton, new WithButtonEmailModelValidator() },
                { EmailTemplateType.ResetPassword, new ResetPasswordEmailModelValidator() },
                { EmailTemplateType.EmailConfirmation, new EmailConfirmationEmailModelValidator() },
            };
        }

        /// <summary>
        /// Валидирует параметры шаблона для указанного письма.
        /// </summary>
        /// <param name="source">Сообщение электронной почты, содержащее параметры шаблона.</param>
        /// <returns><c>true</c>, если параметры валидны; иначе <c>false</c>.</returns>
        public bool Validate(EmailMessage source)
        {
            if (_validators.TryGetValue(source.EmailTemplateType, out var validator))
            {
                return validator.Validate(source.TemplateParams);
            }

            return false;
        }
    }
}