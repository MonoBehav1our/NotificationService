using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Contracts.Emails;
using NotificationService.Api.Contracts.Emails.Requests;
using NotificationService.Api.Contracts.Emails.Responses;
using NotificationService.Api.Contracts.Emails.Routes;
using NotificationService.AppServices.Contracts.Emails.Handlers;

namespace NotificationService.Host.Emails.Controllers
{
    /// <summary>
    /// Контроллер для работы с email.
    /// </summary>
    [ApiController]
    [Route(EmailRoutes.BasePath)]
    public class EmailController : ControllerBase, IEmailController
    {
        private readonly ISendEmailHandler _sendEmailHandler;
        private readonly IGetEmailsHandler _getEmailsHandler;
        private readonly ILogger<EmailController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailController"/> class.
        /// </summary>
        /// <param name="sendEmailHandler"><see cref="ISendEmailHandler"/>.</param>
        /// <param name="getEmailsHandler"><see cref="IGetEmailsHandler"/>.</param>
        /// <param name="logger"><see cref="ILogger"/>.</param>
        public EmailController(ISendEmailHandler sendEmailHandler, IGetEmailsHandler getEmailsHandler, ILogger<EmailController> logger)
        {
            _sendEmailHandler = sendEmailHandler ?? throw new ArgumentNullException(nameof(sendEmailHandler));
            _getEmailsHandler = getEmailsHandler ?? throw new ArgumentNullException(nameof(getEmailsHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Отправка email сообщения.
        /// </summary>
        /// <param name="request">Сообщение.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Задача.</returns>
        [HttpPost]
        [Route(EmailRoutes.SendEmailPath)]
        public async Task SendEmail([FromBody] SendEmailRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                await _sendEmailHandler.Handle(request, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Ошибка отправки email: {Message}", exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Получение списка всех отправленных email сообщений.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Ответ с email сообщениями.</returns>
        [HttpGet]
        [Route(EmailRoutes.GetEmailsPath)]
        public async Task<List<GetEmailMessageResponse>> GetEmails(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _getEmailsHandler.Handle(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения email сообщений: {Message}", ex.Message);
                throw;
            }
        }
    }
}