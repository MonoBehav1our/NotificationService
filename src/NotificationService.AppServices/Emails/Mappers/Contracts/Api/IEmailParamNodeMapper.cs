using NotificationService.Api.Contracts.Emails.Models;
using NotificationService.Api.Contracts.Emails.Types;
using Entity = NotificationService.Entities.Emails;
using EntityTypes = NotificationService.Entities.Emails.Types;

namespace NotificationService.AppServices.Emails.Mappers.Contracts.Api
{
    /// <summary>
    /// Маппер для <see cref="EmailParamNode"/>.
    /// </summary>
    public interface IEmailParamNodeMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="emailParamNode"><see cref="Entity.EmailParamNode"/>.</param>
        /// <returns>Отмапленный узел параметра электронной почты.</returns>
        public EmailParamNode Map(Entity.EmailParamNode emailParamNode);

        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="appServicesParams"><see cref="Dictionary{TKey,TValue}"/>.</param>
        /// <returns>Отмапленный словарь параметров шаблона электронной почты.</returns>
        public Dictionary<EmailTemplateParamType, EmailParamNode> Map(Dictionary<EntityTypes.EmailTemplateParamType, Entity.EmailParamNode> appServicesParams);
    }
}