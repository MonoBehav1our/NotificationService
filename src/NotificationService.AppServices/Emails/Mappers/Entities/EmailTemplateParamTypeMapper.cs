using NotificationService.AppServices.Emails.Mappers.Contracts.Entities;
using NotificationService.Entities.Emails.Types;
using ApiContractsTypes = NotificationService.Api.Contracts.Emails.Types;

namespace NotificationService.AppServices.Emails.Mappers.Entities
{
    /// <summary>
    /// Маппер для <see cref="EmailTemplateParamType"/>.
    /// </summary>
    internal class EmailTemplateParamTypeMapper : IEmailTemplateParamTypeMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="source"><see cref="ApiContractsTypes.EmailTemplateParamType"/>.</param>
        /// <returns>Отмапленный тип параметра шаблона электронной почты.</returns>
        public EmailTemplateParamType Map(ApiContractsTypes.EmailTemplateParamType source)
        {
            return source switch
            {
                ApiContractsTypes.EmailTemplateParamType.Header => EmailTemplateParamType.Header,
                ApiContractsTypes.EmailTemplateParamType.Body => EmailTemplateParamType.Body,
                ApiContractsTypes.EmailTemplateParamType.Button => EmailTemplateParamType.Button,
                ApiContractsTypes.EmailTemplateParamType.Url => EmailTemplateParamType.Url,
                ApiContractsTypes.EmailTemplateParamType.Text => EmailTemplateParamType.Text,
                ApiContractsTypes.EmailTemplateParamType.Code => EmailTemplateParamType.Code,
                ApiContractsTypes.EmailTemplateParamType.ExpirationTime => EmailTemplateParamType.ExpirationTime,
                _ => EmailTemplateParamType.Unknown,
            };
        }
    }
}