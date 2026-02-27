using NotificationService.Entities.Emails;

namespace NotificationService.AppServices.Emails.Repositories.Contracts
{
    /// <summary>
    /// Репозиторий для работы с email сообщениями.
    /// </summary>
    public interface IEmailRepository
    {
        /// <summary>
        /// Сохранить информацию об отправленном сообщении.
        /// </summary>
        /// <param name="message"><see cref="EmailMessageEntity"/>.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Задача.</returns>
        Task AddEmail(EmailMessageEntity message, CancellationToken token);

        /// <summary>
        /// Получить данные о всех отправленных email сообщениях.
        /// </summary>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Список сущностей.</returns>
        Task<List<EmailMessageEntity>> GetEmails(CancellationToken token);
    }
}