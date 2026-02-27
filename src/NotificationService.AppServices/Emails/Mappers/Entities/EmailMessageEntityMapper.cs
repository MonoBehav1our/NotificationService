using NotificationService.AppServices.Contracts.Emails.Models;
using NotificationService.AppServices.Emails.Mappers.Contracts.Entities;
using NotificationService.Entities.Emails;

namespace NotificationService.AppServices.Emails.Mappers.Entities
{
    /// <summary>
    /// Маппер для <see cref="EmailMessageEntity"/>.
    /// </summary>
    internal class EmailMessageEntityMapper : IEmailMessageEntityMapper
    {
        private readonly IEmailParamNodeMapper _emailParamNodeMapper;
        private readonly IEmailTemplateTypeMapper _emailTemplateTypeMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessageEntityMapper"/> class.
        /// </summary>
        /// <param name="emailParamNodeMapper"><see cref="IEmailParamNodeMapper"/>.</param>
        /// <param name="emailTemplateTypeMapper"><see cref="IEmailTemplateTypeMapper"/>.</param>
        public EmailMessageEntityMapper(IEmailParamNodeMapper emailParamNodeMapper, IEmailTemplateTypeMapper emailTemplateTypeMapper)
        {
            _emailParamNodeMapper = emailParamNodeMapper ?? throw new ArgumentNullException(nameof(emailParamNodeMapper));
            _emailTemplateTypeMapper = emailTemplateTypeMapper ?? throw new ArgumentNullException(nameof(emailTemplateTypeMapper));
        }

        /// <summary>
        /// Произвести маппинг.
        /// </summary>
        /// <param name="message"><see cref="EmailMessage"/>.</param>
        /// <returns>Отмапленная сущность сообщения электронной почты.</returns>
        public EmailMessageEntity Map(EmailMessage message)
        {
            if (message == null)
            {
                return null;
            }

            return new EmailMessageEntity
            {
                Author = message.Author,
                Subject = message.Subject,
                Recipients = message.Recipients,
                EmailTemplateType = _emailTemplateTypeMapper.Map(message.EmailTemplateType),
                TemplateParams = _emailParamNodeMapper.Map(message.TemplateParams),
                SendTime = message.SendTime,
            };
        }
    }
}