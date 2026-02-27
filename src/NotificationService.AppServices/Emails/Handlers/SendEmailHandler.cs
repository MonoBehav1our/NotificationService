using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Api.Contracts.Emails.Requests;
using NotificationService.AppServices.Contracts.Emails.Common;
using NotificationService.AppServices.Contracts.Emails.Configuration;
using NotificationService.AppServices.Contracts.Emails.Handlers;
using NotificationService.AppServices.Contracts.Emails.Models;
using NotificationService.AppServices.Emails.Exceptions;
using NotificationService.AppServices.Emails.Mappers.Contracts;
using NotificationService.AppServices.Emails.Mappers.Contracts.Entities;
using NotificationService.AppServices.Emails.Repositories.Contracts;
using NotificationService.RazorTemplates.Handlers.Contracts;
using RazorLight;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace NotificationService.AppServices.Emails.Handlers
{
    /// <summary>
    /// Хендлер для отправки сообщений.
    /// </summary>
    internal class SendEmailHandler : ISendEmailHandler
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly SmtpClient _smtpClient;
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailMessageEntityMapper _emailMessageEntityMapper;
        private readonly IEmailMessageMapper _emailMessageMapper;
        private readonly IEmailValidationHandler _emailValidationHandler;
        private readonly IRazorLightEngine _razorLightEngine;
        private readonly ILogger<SendEmailHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmailHandler"/> class.
        /// </summary>
        /// <param name="repository"><see cref="IEmailRepository"/>.</param>
        /// <param name="smtpSettings"><see cref="IOptionsMonitor{SmtpSettings}"/>.</param>
        /// <param name="smtpClient"><see cref="SmtpClient"/>.</param>
        /// <param name="emailMessageEntityMapper"><see cref="IEmailMessageEntityMapper"/>.</param>
        /// <param name="apiEmailMessageMapper"><see cref="IEmailMessageMapper"/>.</param>
        /// <param name="emailValidationHandler"><see cref="IEmailValidationHandler"/>.</param>
        /// <param name="razorLightEngine"><see cref="IRazorLightEngine"/>.</param>
        /// <param name="logger">Логгер.</param>
        public SendEmailHandler(
            IEmailRepository repository,
            IOptionsMonitor<SmtpSettings> smtpSettings,
            SmtpClient smtpClient,
            IEmailMessageEntityMapper emailMessageEntityMapper,
            IEmailMessageMapper apiEmailMessageMapper,
            IEmailValidationHandler emailValidationHandler,
            IRazorLightEngine razorLightEngine,
            ILogger<SendEmailHandler> logger)
        {
            _emailRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _smtpClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
            _emailMessageEntityMapper = emailMessageEntityMapper ?? throw new ArgumentNullException(nameof(emailMessageEntityMapper));
            _emailMessageMapper = apiEmailMessageMapper ?? throw new ArgumentNullException(nameof(apiEmailMessageMapper));
            _emailValidationHandler = emailValidationHandler ?? throw new ArgumentNullException(nameof(emailValidationHandler));
            _smtpSettings = (smtpSettings ?? throw new ArgumentNullException(nameof(smtpSettings))).CurrentValue;
            _razorLightEngine = razorLightEngine ?? throw new ArgumentNullException(nameof(razorLightEngine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Отправляет email сообщение.
        /// </summary>
        /// <param name="sendEmailRequest">Запрос на отправку email.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public async Task Handle(SendEmailRequest sendEmailRequest, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(sendEmailRequest);

            var message = _emailMessageMapper.Map(sendEmailRequest);

            if (!_emailValidationHandler.Validate(message))
            {
                throw new InvalidEmailModelException("Модель email не прошла валидацию");
            }

            var mimeMessage = await BuildMessage(message);

            try
            {
                await _smtpClient.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.SslOnConnect, cancellationToken);
                await _smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password, cancellationToken);
                await _smtpClient.SendAsync(mimeMessage, cancellationToken);
                await _smtpClient.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError("Отправка сообщения провалилась: {Message}. Тело письма: {MessageBody}", exception.Message, mimeMessage.HtmlBody);
                throw;
            }

            var emailEntity = _emailMessageEntityMapper.Map(message);
            await _emailRepository.AddEmail(emailEntity, cancellationToken);
        }

        private async Task<MimeMessage> BuildMessage(EmailMessage message)
        {
            var key = $"{message.EmailTemplateType}.cshtml";
            var messageBody = await _razorLightEngine.CompileRenderAsync(key, message);

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = messageBody,
            };

            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(Names.MessageAuthorName, _smtpSettings.Username));
            mimeMessage.To.AddRange(message.Recipients.Select(x => new MailboxAddress(Names.RecipientsParamName, x)));
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = bodyBuilder.ToMessageBody();
            return mimeMessage;
        }
    }
}