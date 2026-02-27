using NotificationService.Api.Contracts.Emails.Requests;
using NotificationService.AppServices.Contracts.Emails.Models;
using NotificationService.AppServices.Emails.Mappers.Contracts;
using NotificationService.AppServices.Emails.Mappers.Contracts.Api;
using NotificationService.Entities.Emails;

namespace NotificationService.AppServices.Emails.Mappers
{
    /// <summary>
    /// Маппер для <see cref="EmailMessage"/>.
    /// </summary>
    internal class EmailMessageMapper : IEmailMessageMapper
    {
        private readonly IEmailParamNodeMapper _emailParamNodeMapper;
        private readonly IEmailTemplateTypeMapper _emailTemplateTypeMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessageMapper"/> class.
        /// </summary>
        /// <param name="emailParamNodeMapper"><see cref="IEmailParamNodeMapper"/>.</param>
        /// <param name="emailTemplateTypeMapper"><see cref="IEmailTemplateTypeMapper"/>.</param>
        public EmailMessageMapper(IEmailParamNodeMapper emailParamNodeMapper, IEmailTemplateTypeMapper emailTemplateTypeMapper)
        {
            _emailParamNodeMapper = emailParamNodeMapper ?? throw new ArgumentNullException(nameof(emailParamNodeMapper));
            _emailTemplateTypeMapper = emailTemplateTypeMapper ?? throw new ArgumentNullException(nameof(emailTemplateTypeMapper));
        }

        /// <summary>
        /// Произвести маппинг.
        /// Задает значению поля Author "Welwise Games".
        /// Задает значению поля SenTime текущее время.
        /// </summary>
        /// <param name="request"><see cref="SendEmailRequest"/>.</param>
        /// <returns>Отмапленное сообщение электронной почты.</returns>
        public EmailMessage Map(SendEmailRequest request)
        {
            if (request == null)
            {
                return null;
            }

            return new EmailMessage
            {
                Author = "Welwise Games",
                Subject = request.Subject,
                Recipients = request.Recipients,
                EmailTemplateType = request.EmailTemplateType,
                TemplateParams = request.TemplateParams,
                SendTime = DateTime.UtcNow,
            };
        }

        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="entity"><see cref="EmailMessageEntity"/>.</param>
        /// <returns>Отмапленное сообщение электронной почты.</returns>
        public EmailMessage Map(EmailMessageEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new EmailMessage
            {
                Author = entity.Author,
                Subject = entity.Subject,
                Recipients = entity.Recipients,
                EmailTemplateType = _emailTemplateTypeMapper.Map(entity.EmailTemplateType),
                TemplateParams = _emailParamNodeMapper.Map(entity.TemplateParams),
                SendTime = entity.SendTime,
            };
        }
    }
}