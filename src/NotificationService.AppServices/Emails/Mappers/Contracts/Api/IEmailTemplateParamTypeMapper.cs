using NotificationService.Api.Contracts.Emails.Types;
using EntityTypes = NotificationService.Entities.Emails.Types;

namespace NotificationService.AppServices.Emails.Mappers.Contracts.Api
{
    /// <summary>
    /// Маппер для <see cref="EmailTemplateParamType"/>.
    /// </summary>
    public interface IEmailTemplateParamTypeMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="source"><see cref="EntityTypes.EmailTemplateParamType"/>.</param>
        /// <returns>Отмапленный тип параметра шаблона электронной почты.</returns>
        EmailTemplateParamType Map(EntityTypes.EmailTemplateParamType source);
    }
}