using NotificationService.Api.Contracts.Emails.Responses;
using NotificationService.AppServices.Contracts.Emails.Handlers;
using NotificationService.AppServices.Contracts.Emails.Mappers.Api;
using NotificationService.AppServices.Emails.Mappers.Contracts;
using NotificationService.AppServices.Emails.Repositories.Contracts;

namespace NotificationService.AppServices.Emails.Handlers
{
    /// <summary>
    /// Хендлер для получения сообщений.
    /// </summary>
    internal class GetEmailsHandler : IGetEmailsHandler
    {
        private readonly IEmailRepository _repository;
        private readonly IEmailMessageMapper _emailMessageMapper;
        private readonly IGetEmailMessageResponseMapper _getEmailMessageResponseMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEmailsHandler"/> class.
        /// </summary>
        /// <param name="repository"><see cref="IEmailRepository"/>.</param>
        /// <param name="emailMessageMapper"><see cref="IEmailMessageMapper"/>.</param>
        /// <param name="getEmailMessageResponseMapper"><see cref="IGetEmailMessageResponseMapper"/>.</param>
        public GetEmailsHandler(
            IEmailRepository repository,
            IEmailMessageMapper emailMessageMapper,
            IGetEmailMessageResponseMapper getEmailMessageResponseMapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _emailMessageMapper = emailMessageMapper ?? throw new ArgumentNullException(nameof(emailMessageMapper));
            _getEmailMessageResponseMapper = getEmailMessageResponseMapper ?? throw new ArgumentNullException(nameof(getEmailMessageResponseMapper));
        }

        /// <summary>
        /// Получения всех email сообщений.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список email сообщений.</returns>
        public async Task<List<GetEmailMessageResponse>> Handle(CancellationToken cancellationToken)
        {
            var emailEntities = await _repository.GetEmails(cancellationToken);

            var messages = emailEntities.Select(x => _emailMessageMapper.Map(x)).ToList();

            return messages.Select(x => _getEmailMessageResponseMapper.Map(x)).ToList();
        }
    }
}