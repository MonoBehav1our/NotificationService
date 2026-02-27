using NotificationService.Api.Contracts.Emails.Models;
using NotificationService.Api.Contracts.Emails.Types;
using NotificationService.AppServices.Emails.Mappers.Contracts.Api;
using Entity = NotificationService.Entities.Emails;
using EntityTypes = NotificationService.Entities.Emails.Types;

namespace NotificationService.AppServices.Emails.Mappers.Api
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
        /// <param name="emailParamNode"><see cref="Entity.EmailParamNode"/>.</param>
        /// <returns>Отмапленный узел параметра электронной почты.</returns>
        public EmailParamNode Map(Entity.EmailParamNode emailParamNode)
        {
            if (emailParamNode == null)
            {
                return null;
            }

            return new EmailParamNode
            {
                Value = emailParamNode.Value,
                Childs = emailParamNode.Childs != null ? Map(emailParamNode.Childs) : null,
            };
        }

        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="appServicesParams"><see cref="Dictionary{TKey,TValue}"/>.</param>
        /// <returns>Отмапленный словарь параметров шаблона электронной почты.</returns>
        public Dictionary<EmailTemplateParamType, EmailParamNode> Map(Dictionary<EntityTypes.EmailTemplateParamType, Entity.EmailParamNode> appServicesParams)
        {
            if (appServicesParams == null)
            {
                return null;
            }

            var result = new Dictionary<EmailTemplateParamType, EmailParamNode>();

            foreach (var kvp in appServicesParams)
            {
                result[_emailTemplateParamTypeMapper.Map(kvp.Key)] = Map(kvp.Value);
            }

            return result;
        }
    }
}
