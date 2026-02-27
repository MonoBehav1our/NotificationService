using NotificationService.AppServices.Contracts.Emails.Models;
using NotificationService.Entities.Emails;

namespace NotificationService.AppServices.Emails.Mappers.Contracts.Entities
{
    /// <summary>
    /// Маппер для <see cref="EmailMessageEntity"/>.
    /// </summary>
    public interface IEmailMessageEntityMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="message"><see cref="EmailMessage"/>.</param>
        /// <returns>Отмапленная сущность сообщения электронной почты.</returns>
        EmailMessageEntity Map(EmailMessage message);
    }
}