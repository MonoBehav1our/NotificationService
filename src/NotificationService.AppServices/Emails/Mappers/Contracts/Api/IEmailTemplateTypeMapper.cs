using NotificationService.Api.Contracts.Emails.Types;
using EntityTypes = NotificationService.Entities.Emails.Types;

namespace NotificationService.AppServices.Emails.Mappers.Contracts.Api
{
    /// <summary>
    /// Маппер для <see cref="EmailTemplateType"/>.
    /// </summary>
    public interface IEmailTemplateTypeMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="emailTemplateType"><see cref="EntityTypes.EmailTemplateType"/>.</param>
        /// <returns>Отмапленный тип шаблона электронной почты.</returns>
        public EmailTemplateType Map(EntityTypes.EmailTemplateType emailTemplateType);
    }
}