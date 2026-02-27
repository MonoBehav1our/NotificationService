using NotificationService.Api.Contracts.Emails.Types;
using NotificationService.AppServices.Emails.Mappers.Contracts.Api;
using EntityTypes = NotificationService.Entities.Emails.Types;

namespace NotificationService.AppServices.Emails.Mappers.Api
{
    /// <summary>
    /// Маппер для <see cref="EmailTemplateType"/>.
    /// </summary>
    internal class EmailTemplateTypeMapper : IEmailTemplateTypeMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="emailTemplateType"><see cref="EntityTypes.EmailTemplateType"/>.</param>
        /// <returns>Отмапленный тип шаблона электронной почты.</returns>
        public EmailTemplateType Map(EntityTypes.EmailTemplateType emailTemplateType)
        {
            return emailTemplateType switch
            {
                EntityTypes.EmailTemplateType.WithButton => EmailTemplateType.WithButton,
                EntityTypes.EmailTemplateType.ResetPassword => EmailTemplateType.ResetPassword,
                EntityTypes.EmailTemplateType.EmailConfirmation => EmailTemplateType.EmailConfirmation,
                _ => EmailTemplateType.Unknown,
            };
        }
    }
}
