using NotificationService.AppServices.Emails.Mappers.Contracts.Entities;
using NotificationService.Entities.Emails;
using NotificationService.Entities.Emails.Types;
using ApiContractsModels = NotificationService.Api.Contracts.Emails.Models;
using ApiContractsTypes = NotificationService.Api.Contracts.Emails.Types;

namespace NotificationService.AppServices.Emails.Mappers.Entities
{
    /// <summary>
    /// Маппер для <see cref="EmailParamNode"/>.
    /// </summary>
    internal class EmailParamNodeMapper : IEmailParamNodeMapper
    {
        private readonly IEmailTemplateParamTypeMapper _emailTemplateParamTypeMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailParamNodeMapper"/> class.
        /// </summary>
        /// <param name="emailTemplateParamTypeMapper"><see cref="IEmailTemplateParamTypeMapper"/>.</param>
        public EmailParamNodeMapper(IEmailTemplateParamTypeMapper emailTemplateParamTypeMapper)
        {
            _emailTemplateParamTypeMapper = emailTemplateParamTypeMapper;
        }

        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="apiNode"><see cref="ApiContractsModels.EmailParamNode"/>.</param>
        /// <returns>Отмапленный узел параметра электронной почты.</returns>
        public EmailParamNode Map(ApiContractsModels.EmailParamNode apiNode)
        {
            if (apiNode == null)
            {
                return null;
            }

            return new EmailParamNode
            {
                Value = apiNode.Value,
                Childs = apiNode.Childs != null ? Map(apiNode.Childs) : null,
            };
        }

        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="apiParams"><see cref="Dictionary{TKey,TValue}"/>.</param>
        /// <returns>Отмапленный словарь параметров шаблона электронной почты.</returns>
        public Dictionary<EmailTemplateParamType, EmailParamNode> Map(Dictionary<ApiContractsTypes.EmailTemplateParamType, ApiContractsModels.EmailParamNode> apiParams)
        {
            if (apiParams == null)
            {
                return null;
            }

            var result = new Dictionary<EmailTemplateParamType, EmailParamNode>();

            foreach (var kvp in apiParams)
            {
                result[_emailTemplateParamTypeMapper.Map(kvp.Key)] = Map(kvp.Value);
            }

            return result;
        }
    }
}