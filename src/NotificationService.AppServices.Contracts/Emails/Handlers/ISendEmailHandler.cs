using NotificationService.Api.Contracts.Emails.Requests;

namespace NotificationService.AppServices.Contracts.Emails.Handlers
{
    /// <summary>
    /// Обработчик для отправки email сообщений.
    /// </summary>
    public interface ISendEmailHandler
    {
        /// <summary>
        /// Отправляет email сообщение.
        /// </summary>
        /// <param name="sendEmailRequest">Запрос на отправку email.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        Task Handle(SendEmailRequest sendEmailRequest, CancellationToken cancellationToken = default);
    }
}