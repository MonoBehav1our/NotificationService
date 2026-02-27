using NotificationService.AppServices.Contracts.Emails.Models;
using ApiContractRequests = NotificationService.Api.Contracts.Emails.Requests;
using Entity = NotificationService.Entities.Emails;

namespace NotificationService.AppServices.Emails.Mappers.Contracts
{
    /// <summary>
    /// Маппер в <see cref="EmailMessage"/>.
    /// </summary>
    public interface IEmailMessageMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="request"><see cref="ApiContractRequests.SendEmailRequest"/>.</param>
        /// <returns>Отмапленное сообщение электронной почты.</returns>
        EmailMessage Map(ApiContractRequests.SendEmailRequest request);

        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="entity"><see cref="Entity.EmailMessageEntity"/>.</param>
        /// <returns>Отмапленное сообщение электронной почты.</returns>
        EmailMessage Map(Entity.EmailMessageEntity entity);
    }
}