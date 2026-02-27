using NotificationService.Entities.Emails;
using NotificationService.Entities.Emails.Types;
using ApiContractsModels = NotificationService.Api.Contracts.Emails.Models;
using ApiContractsTypes = NotificationService.Api.Contracts.Emails.Types;

namespace NotificationService.AppServices.Emails.Mappers.Contracts.Entities
{
    /// <summary>
    /// Маппер в <see cref="EmailParamNode"/>.
    /// </summary>
    public interface IEmailParamNodeMapper
    {
        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="apiNode"><see cref="ApiContractsModels.EmailParamNode"/>.</param>
        /// <returns>Отмапленный узел параметра электронной почты.</returns>
        EmailParamNode Map(ApiContractsModels.EmailParamNode apiNode);

        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="apiParams"><see cref="Dictionary{TKey,TValue}"/>.</param>
        /// <returns>Отмапленный словарь параметров шаблона электронной почты.</returns>
        Dictionary<EmailTemplateParamType, EmailParamNode> Map(Dictionary<ApiContractsTypes.EmailTemplateParamType, ApiContractsModels.EmailParamNode> apiParams);
    }
}
