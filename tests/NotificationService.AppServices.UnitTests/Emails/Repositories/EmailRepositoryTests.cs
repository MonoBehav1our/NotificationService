using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using NotificationService.AppServices.Emails.Repositories;
using NotificationService.AppServices.UnitTests.Infrastructure;
using NotificationService.AppServices.UnitTests.Infrastructure.Asserts;
using NotificationService.Entities.Contexts;
using NotificationService.Entities.Emails;
using NotificationService.Entities.Emails.Types;
using NSubstitute;
using Xunit;

namespace NotificationService.AppServices.UnitTests.Emails.Repositories
{
    /// <summary>
    /// Тесты для <see cref="EmailRepository"/>.
    /// </summary>
    public class EmailRepositoryTests : IDisposable
    {
        private readonly ILogger<EmailRepository> _logger;
        private readonly Fixture _fixture = new();
        private readonly WelwiseGamesDbContext _db;
        private readonly EmailRepository _emailRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailRepositoryTests"/> class.
        /// </summary>
        public EmailRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<WelwiseGamesDbContext>()
                .UseInMemoryDatabase($"TestDatabaseForEmailRepository-{Guid.NewGuid()}")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _logger = Substitute.For<ILogger<EmailRepository>>();

            _db = new WelwiseGamesDbContext(options);
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
            _emailRepository = new EmailRepository(_db, _logger);
        }

        /// <summary>
        /// Проверяет, что валидная сущность сохраняется в базе данных.
        /// </summary>
        /// <returns>Задача.</returns>
        [Fact]
        public async Task AddEmail_ValidEntity_PersistsInDatabase()
        {
            // Arrange
            var entity = CreateValidEntity();

            // Act
            await _emailRepository.AddEmail(entity, CancellationToken.None);

            // Assert
            var saved = await _db.Emails.AsNoTracking().FirstOrDefaultAsync();
            Assert.NotNull(saved);
            EmailMessageAssert.Equal(entity, saved!);
        }

        /// <summary>
        /// Проверяет, что при наличии сущностей в базе возвращаются все записи.
        /// </summary>
        /// <returns>Задача.</returns>
        [Fact]
        public async Task GetEmails_WithExistingEntities_ReturnsAll()
        {
            // Arrange
            var first = CreateValidEntity();
            var second = CreateValidEntity();
            second.Subject = "Another";
            second.SendTime = new DateTime(2025, 1, 2, 8, 0, 0, DateTimeKind.Utc);
            var third = CreateValidEntity();
            third.Subject = "Third";
            third.EmailTemplateType = EmailTemplateType.WithButton;

            var expectedEmails = new List<EmailMessageEntity> { first, second, third };

            await _db.Emails.AddRangeAsync(expectedEmails);
            await _db.SaveChangesAsync();

            // Act
            var result = await _emailRepository.GetEmails(CancellationToken.None);

            // Assert
            EnumerableAssert.Equals(expectedEmails, result, EmailMessageAssert.Equal);
        }

        /// <summary>
        /// Освобождает ресурсы, используемые тестом.
        /// </summary>
        public void Dispose()
        {
            _db.Database.EnsureDeleted();
            _db.Dispose();
        }

        private EmailMessageEntity CreateValidEntity()
        {
            return _fixture.Build<EmailMessageEntity>()
                .With(x => x.Author, "author@example.com")
                .With(x => x.Subject, "Subject")
                .With(x => x.Recipients, new List<string> { "user1@example.com", "user2@example.com" })
                .With(x => x.EmailTemplateType, EmailTemplateType.WithButton)
                .With(x => x.TemplateParams, new Dictionary<EmailTemplateParamType, EmailParamNode>
                {
                    [EmailTemplateParamType.Header] = new() { Value = "Hello" },
                    [EmailTemplateParamType.Text] = new() { Value = "Click" },
                })
                .With(x => x.SendTime, new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc))
                .Create();
        }
    }
}