using NotificationService.Entities.Emails.Types;
using ApiContractsTypes = NotificationService.Api.Contracts.Emails.Types;

namespace NotificationService.AppServices.Emails.Mappers.Contracts.Entities
{
    /// <summary>
    /// Маппер для <see cref="EmailTemplateType"/>.
    /// </summary>
    public interface IEmailTemplateTypeMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="source"><see cref="ApiContractsTypes.EmailTemplateType"/>.</param>
        /// <returns>Отмапленный тип шаблона электронной почты.</returns>
        EmailTemplateType Map(ApiContractsTypes.EmailTemplateType source);
    }
}