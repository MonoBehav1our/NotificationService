using AutoFixture;
using NotificationService.AppServices.Contracts.Emails.Mappers.Api;
using NotificationService.AppServices.Emails.Handlers;
using NotificationService.AppServices.Emails.Mappers.Contracts;
using NotificationService.AppServices.Emails.Repositories.Contracts;
using NotificationService.AppServices.UnitTests.Infrastructure;
using NotificationService.AppServices.UnitTests.Infrastructure.Asserts;
using NotificationService.Entities.Emails;
using NSubstitute;
using Xunit;
using EntitiesTypes = NotificationService.Entities.Emails.Types;

namespace NotificationService.AppServices.UnitTests.Emails.Handlers
{
    /// <summary>
    /// Тесты для <see cref="GetEmailsHandler"/>.
    /// </summary>
    public class GetEmailsHandlerTests
    {
        private readonly IEmailRepository _repository = Substitute.For<IEmailRepository>();
        private readonly IEmailMessageMapper _emailMessageMapper = MapperProvider.GetMapper<IEmailMessageMapper>();
        private readonly IGetEmailMessageResponseMapper _getEmailMessageResponseMapper = MapperProvider.GetMapper<IGetEmailMessageResponseMapper>();
        private readonly Fixture _fixture = new();

        private readonly GetEmailsHandler _getEmailsHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetEmailsHandlerTests"/> class.
        /// </summary>
        public GetEmailsHandlerTests()
        {
            _getEmailsHandler = new GetEmailsHandler(_repository, _emailMessageMapper, _getEmailMessageResponseMapper);

            _fixture.Customize<EmailMessageEntity>(c => c.FromFactory(() => CreateEmailMessageEntity()));
        }

        /// <summary>
        /// Проверяет, что обработчик возвращает сопоставленные сообщения при возврате сущностей репозиторием.
        /// </summary>
        /// <returns>Задача.</returns>
        [Fact]
        public async Task Handle_ReturnsMappedMessages_WhenRepositoryReturnsEntities()
        {
            // Arrange
            var emailEntities = new List<EmailMessageEntity>
            {
                CreateEmailMessageEntity(),
                CreateEmailMessageEntity(),
                CreateEmailMessageEntity(),
            };

            _repository.GetEmails(Arg.Any<CancellationToken>()).Returns(emailEntities);

            // Act
            var response = await _getEmailsHandler.Handle(CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(emailEntities.Count, response.Count);

            EnumerableAssert.Equals(emailEntities, response, EmailMessageAssert.Equal);
        }

        /// <summary>
        /// Проверяет, что обработчик возвращает пустой список при отсутствии сущностей в репозитории.
        /// </summary>
        /// <returns>Задача.</returns>
        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenRepositoryReturnsNoEntities()
        {
            // Arrange
            _repository.GetEmails(Arg.Any<CancellationToken>()).Returns(new List<EmailMessageEntity>());

            // Act
            var response = await _getEmailsHandler.Handle(CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Empty(response);
        }

        /// <summary>
        /// Проверяет, что обработчик выбрасывает исключение при ошибке в репозитории.
        /// </summary>
        /// <returns>Задача.</returns>
        [Fact]
        public async Task Handle_ThrowsException_WhenRepositoryThrowsException()
        {
            // Arrange
            _repository.GetEmails(Arg.Any<CancellationToken>()).Returns(_ => Task.FromException<List<EmailMessageEntity>>(new InvalidOperationException("Database error")));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _getEmailsHandler.Handle(CancellationToken.None));
        }

        private EmailMessageEntity CreateEmailMessageEntity()
        {
            return new EmailMessageEntity
            {
                Author = _fixture.Create<string>(),
                Subject = _fixture.Create<string>(),
                Recipients = _fixture.CreateMany<string>(2).ToList(),
                EmailTemplateType = EntitiesTypes.EmailTemplateType.WithButton,
                TemplateParams = CreateTemplateParams(),
                SendTime = _fixture.Create<DateTime>(),
            };
        }

        private Dictionary<EntitiesTypes.EmailTemplateParamType, EmailParamNode> CreateTemplateParams()
        {
            return new Dictionary<EntitiesTypes.EmailTemplateParamType, EmailParamNode>
            {
                [EntitiesTypes.EmailTemplateParamType.Header] = new() { Value = _fixture.Create<string>() },
                [EntitiesTypes.EmailTemplateParamType.Body] = new() { Value = _fixture.Create<string>() },
                [EntitiesTypes.EmailTemplateParamType.Button] = new()
                {
                    Childs = new Dictionary<EntitiesTypes.EmailTemplateParamType, EmailParamNode>
                    {
                        [EntitiesTypes.EmailTemplateParamType.Url] = new() { Value = _fixture.Create<string>() },
                        [EntitiesTypes.EmailTemplateParamType.Text] = new() { Value = _fixture.Create<string>() },
                    },
                },
            };
        }
    }
}
