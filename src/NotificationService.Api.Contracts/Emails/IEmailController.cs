using NotificationService.Api.Contracts.Emails.Requests;
using NotificationService.Api.Contracts.Emails.Responses;
using NotificationService.Api.Contracts.Emails.Routes;
using RestEase;

namespace NotificationService.Api.Contracts.Emails
{
    /// <summary>
    /// Контроллер для работы с email.
    /// </summary>
    [BasePath(EmailRoutes.BasePath)]
    public interface IEmailController
    {
        /// <summary>
        /// Отправка email сообщения.
        /// </summary>
        /// <param name="request"><see cref="SendEmailRequest"/>.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача.</returns>
        [Post(EmailRoutes.SendEmailPath)]
        Task SendEmail([Body] SendEmailRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение списка всех отправленных email сообщений.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список email сообщений.</returns>
        [Get(EmailRoutes.GetEmailsPath)]
        Task<List<GetEmailMessageResponse>> GetEmails(CancellationToken cancellationToken = default);
    }
}