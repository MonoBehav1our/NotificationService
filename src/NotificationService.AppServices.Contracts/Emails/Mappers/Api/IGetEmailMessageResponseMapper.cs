using NotificationService.Api.Contracts.Emails.Responses;
using NotificationService.AppServices.Contracts.Emails.Models;

namespace NotificationService.AppServices.Contracts.Emails.Mappers.Api
{
    /// <summary>
    /// Маппер для <see cref="GetEmailMessageResponse"/>.
    /// </summary>
    public interface IGetEmailMessageResponseMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="emailMessage"><see cref="EmailMessage"/>.</param>
        /// <returns><see cref="GetEmailMessageResponse"/>.</returns>
        public GetEmailMessageResponse Map(EmailMessage emailMessage);
    }
}