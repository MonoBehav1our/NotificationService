using NotificationService.Api.Contracts.Emails.Types;
using NotificationService.AppServices.Emails.Mappers.Contracts.Api;
using EntityTypes = NotificationService.Entities.Emails.Types;

namespace NotificationService.AppServices.Emails.Mappers.Api
{
    /// <summary>
    /// Маппер для <see cref="EmailTemplateParamType"/>.
    /// </summary>
    internal class EmailTemplateParamTypeMapper : IEmailTemplateParamTypeMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="source"><see cref="EntityTypes.EmailTemplateParamType"/>.</param>
        /// <returns>Отмапленный тип параметра шаблона электронной почты.</returns>
        public EmailTemplateParamType Map(EntityTypes.EmailTemplateParamType source)
        {
            return source switch
            {
                EntityTypes.EmailTemplateParamType.Header => EmailTemplateParamType.Header,
                EntityTypes.EmailTemplateParamType.Body => EmailTemplateParamType.Body,
                EntityTypes.EmailTemplateParamType.Button => EmailTemplateParamType.Button,
                EntityTypes.EmailTemplateParamType.Url => EmailTemplateParamType.Url,
                EntityTypes.EmailTemplateParamType.Text => EmailTemplateParamType.Text,
                EntityTypes.EmailTemplateParamType.Code => EmailTemplateParamType.Code,
                EntityTypes.EmailTemplateParamType.ExpirationTime => EmailTemplateParamType.ExpirationTime,
                _ => EmailTemplateParamType.Unknown,
            };
        }
    }
}