using NotificationService.AppServices.Emails.Mappers.Contracts.Entities;
using NotificationService.Entities.Emails.Types;
using ApiContractsTypes = NotificationService.Api.Contracts.Emails.Types;

namespace NotificationService.AppServices.Emails.Mappers.Entities
{
    /// <summary>
    /// Маппер для <see cref="EmailTemplateType"/>.
    /// </summary>
    internal class EmailTemplateTypeMapper : IEmailTemplateTypeMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="source"><see cref="ApiContractsTypes.EmailTemplateType"/>.</param>
        /// <returns>Отмапленный тип шаблона электронной почты.</returns>
        public EmailTemplateType Map(ApiContractsTypes.EmailTemplateType source)
        {
            return source switch
            {
                ApiContractsTypes.EmailTemplateType.WithButton => EmailTemplateType.WithButton,
                ApiContractsTypes.EmailTemplateType.ResetPassword => EmailTemplateType.ResetPassword,
                ApiContractsTypes.EmailTemplateType.EmailConfirmation => EmailTemplateType.EmailConfirmation,
                _ => EmailTemplateType.Unknown,
            };
        }
    }
}