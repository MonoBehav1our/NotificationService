using NotificationService.Api.Contracts.Emails.Responses;
using NotificationService.AppServices.Contracts.Emails.Mappers.Api;
using NotificationService.AppServices.Contracts.Emails.Models;
using NotificationService.AppServices.Emails.Mappers.Contracts.Api;

namespace NotificationService.AppServices.Emails.Mappers.Api
{
    /// <summary>
    /// Маппер для <see cref="GetEmailMessageResponse"/>.
    /// </summary>
    internal class GetEmailMessageResponseMapper : IGetEmailMessageResponseMapper
    {
        private readonly IEmailParamNodeMapper _paramNodeMapper;
        private readonly IEmailTemplateTypeMapper _templateTypeMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEmailMessageResponseMapper"/> class.
        /// </summary>
        /// <param name="paramNodeMapper"><see cref="IEmailParamNodeMapper"/>.</param>
        /// <param name="templateTypeMapper"><see cref="IEmailTemplateTypeMapper"/>.</param>
        public GetEmailMessageResponseMapper(IEmailParamNodeMapper paramNodeMapper, IEmailTemplateTypeMapper templateTypeMapper)
        {
            _paramNodeMapper = paramNodeMapper ?? throw new ArgumentNullException(nameof(paramNodeMapper));
            _templateTypeMapper = templateTypeMapper ?? throw new ArgumentNullException(nameof(templateTypeMapper));
        }

        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="emailMessage"><see cref="EmailMessage"/>.</param>
        /// <returns>Отмапленный ответ сообщения электронной почты.</returns>
        public GetEmailMessageResponse Map(EmailMessage emailMessage)
        {
            if (emailMessage == null)
            {
                return null;
            }

            return new GetEmailMessageResponse
            {
                Author = emailMessage.Author,
                Subject = emailMessage.Subject,
                Recipients = emailMessage.Recipients,
                EmailTemplateType = emailMessage.EmailTemplateType,
                TemplateParams = emailMessage.TemplateParams,
                SendTime = emailMessage.SendTime,
            };
        }
    }
}
