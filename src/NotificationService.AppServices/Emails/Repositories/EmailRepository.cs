using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotificationService.AppServices.Emails.Repositories.Contracts;
using NotificationService.Entities.Contexts;
using NotificationService.Entities.Emails;

namespace NotificationService.AppServices.Emails.Repositories
{
    /// <summary>
    /// Репозиторий для работы с email сообщениями.
    /// </summary>
    internal class EmailRepository : IEmailRepository
    {
        private readonly WelwiseGamesDbContext _dbContext;
        private readonly ILogger<EmailRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailRepository"/> class.
        /// </summary>
        /// <param name="dbContext"><see cref="WelwiseGamesDbContext"/>.</param>
        /// <param name="logger">Логгер.</param>
        public EmailRepository(
            WelwiseGamesDbContext dbContext,
            ILogger<EmailRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Сохранить информацию об отправленном сообщении.
        /// </summary>
        /// <param name="message"><see cref="EmailMessageEntity"/>.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Задача.</returns>
        public async Task AddEmail(EmailMessageEntity message, CancellationToken token)
        {
            try
            {
                await _dbContext.Emails.AddAsync(message, token);
                await _dbContext.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка сохранения: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Получить данные о всех отправленных email сообщениях.
        /// </summary>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Список сущностей.</returns>
        public async Task<List<EmailMessageEntity>> GetEmails(CancellationToken token)
        {
            try
            {
                return await _dbContext.Emails.AsNoTracking().ToListAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения сообщений из базы: {Message}", ex.Message);
                throw;
            }
        }
    }
}