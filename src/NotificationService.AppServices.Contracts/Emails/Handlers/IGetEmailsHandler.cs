using NotificationService.Api.Contracts.Emails.Responses;

namespace NotificationService.AppServices.Contracts.Emails.Handlers
{
    /// <summary>
    /// Обработчик для получения email сообщений.
    /// </summary>
    public interface IGetEmailsHandler
    {
        /// <summary>
        /// Получение всех email сообщений.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Ответ с сообщениями.</returns>
        Task<List<GetEmailMessageResponse>> Handle(CancellationToken cancellationToken = default);
    }
}