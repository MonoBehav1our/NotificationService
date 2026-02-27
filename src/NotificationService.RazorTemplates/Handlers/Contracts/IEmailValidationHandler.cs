using NotificationService.AppServices.Contracts.Emails.Models;

namespace NotificationService.RazorTemplates.Handlers.Contracts
{
    /// <summary>
    /// Контракт обработчика валидации моделей шаблонов писем.
    /// </summary>
    public interface IEmailValidationHandler
    {
        /// <summary>
        /// Валидирует параметры шаблона для указанного письма.
        /// </summary>
        /// <param name="source">Сообщение электронной почты, содержащее параметры шаблона.</param>
        /// <returns><c>true</c>, если параметры валидны; иначе <c>false</c>.</returns>
        public bool Validate(EmailMessage source);
    }
}