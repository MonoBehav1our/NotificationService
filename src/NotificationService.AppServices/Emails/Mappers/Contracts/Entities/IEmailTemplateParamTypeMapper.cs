using NotificationService.Entities.Emails.Types;
using ApiContractsTypes = NotificationService.Api.Contracts.Emails.Types;

namespace NotificationService.AppServices.Emails.Mappers.Contracts.Entities
{
    /// <summary>
    /// Маппер для <see cref="EmailTemplateParamType"/>.
    /// </summary>
    public interface IEmailTemplateParamTypeMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="source"><see cref="ApiContractsTypes.EmailTemplateParamType"/>.</param>
        /// <returns>Отмапленный тип параметра шаблона электронной почты.</returns>
        EmailTemplateParamType Map(ApiContractsTypes.EmailTemplateParamType source);
    }
}