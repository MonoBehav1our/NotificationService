using System.Net;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Api.Contracts.Emails.Requests;
using NotificationService.AppServices.Contracts.Emails.Configuration;
using NotificationService.AppServices.Contracts.Emails.Models;
using NotificationService.AppServices.Emails.Exceptions;
using NotificationService.AppServices.Emails.Handlers;
using NotificationService.AppServices.Emails.Mappers.Contracts;
using NotificationService.AppServices.Emails.Mappers.Contracts.Entities;
using NotificationService.AppServices.Emails.Repositories.Contracts;
using NotificationService.AppServices.UnitTests.Infrastructure;
using NotificationService.Entities.Emails;
using NSubstitute;
using RazorLight;
using Xunit;
using ApiContracts = NotificationService.Api.Contracts.Emails.Models;
using ApiTypes = NotificationService.Api.Contracts.Emails.Types;
using EntitiesTypes = NotificationService.Entities.Emails.Types;
using RazorContracts = NotificationService.RazorTemplates.Handlers.Contracts;

namespace NotificationService.AppServices.UnitTests.Emails.Handlers
{
    /// <summary>
    /// Тесты для <see cref="SendEmailHandler"/>.
    /// </summary>
    public class SendEmailHandlerTests
    {
        private readonly IEmailRepository _repository = Substitute.For<IEmailRepository>();
        private readonly SmtpClient _smtpClient = Substitute.For<SmtpClient>();
        private readonly IEmailMessageEntityMapper _entityMapper = MapperProvider.GetMapper<IEmailMessageEntityMapper>();
        private readonly IEmailMessageMapper _requestMapper = MapperProvider.GetMapper<IEmailMessageMapper>();
        private readonly RazorContracts.IEmailValidationHandler _validator = Substitute.For<RazorContracts.IEmailValidationHandler>();
        private readonly IRazorLightEngine _razorEngine = Substitute.For<IRazorLightEngine>();
        private readonly ILogger<SendEmailHandler> _logger = Substitute.For<ILogger<SendEmailHandler>>();
        private readonly SmtpSettings _settings;

        private readonly SendEmailHandler _sut;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendEmailHandlerTests"/> class.
        /// </summary>
        public SendEmailHandlerTests()
        {
            _settings = new SmtpSettings { Host = "smtp.test", Port = 2525, Username = "user@test", Password = "pass" };
            var options = CreateOptions(_settings);

            _sut = new SendEmailHandler(_repository, options, _smtpClient, _entityMapper, _requestMapper, _validator, _razorEngine, _logger);
        }

        /// <summary>
        /// Проверяет, что обработчик корректно отправляет email при валидном запросе.
        /// </summary>
        /// <returns>Задача.</returns>
        [Fact]
        public async Task Handle_ValidRequest_SendsEmail()
        {
            // Arrange
            var request = CreateValidRequest();

            _razorEngine.CompileRenderAsync(Arg.Any<string>(), Arg.Any<EmailMessage>()).Returns("<html></html>");
            _validator.Validate(Arg.Any<EmailMessage>()).Returns(true);

            // Act
            await _sut.Handle(request, CancellationToken.None);

            // Assert
            await _smtpClient.Received(1).ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, Arg.Any<CancellationToken>());
            await _smtpClient.Received(1).AuthenticateAsync(
                Arg.Any<Encoding>(),
                Arg.Is<NetworkCredential>(c =>
                    c.UserName == _settings.Username &&
                    c.Password == _settings.Password),
                Arg.Any<CancellationToken>());

            await _smtpClient.Received(1).SendAsync(Arg.Any<MimeMessage>(), Arg.Any<CancellationToken>());
            await _smtpClient.Received(1).DisconnectAsync(true, Arg.Any<CancellationToken>());

            _validator.Received(1).Validate(Arg.Any<EmailMessage>());

            await _repository.Received(1).AddEmail(
                Arg.Is<EmailMessageEntity>(e =>
                    e.Author == "Welwise Games" &&
                    e.Subject == request.Subject &&
                    e.EmailTemplateType == EntitiesTypes.EmailTemplateType.WithButton
                    && e.Recipients.Count == request.Recipients.Count
                    && e.TemplateParams.Count == request.TemplateParams.Count),
                Arg.Any<CancellationToken>());

            await _razorEngine.Received(1).CompileRenderAsync(Arg.Is<string>(k => k == $"{request.EmailTemplateType}.cshtml"), Arg.Any<EmailMessage>());
        }

        /// <summary>
        /// Проверяет, что обработчик выбрасывает исключение при невалидной модели email.
        /// </summary>
        /// <returns>Задача.</returns>
        [Fact]
        public async Task Handle_InvalidModel_ThrowsInvalidEmailModelException()
        {
            // Arrange
            var request = CreateValidRequest();

            _validator.Validate(Arg.Any<EmailMessage>()).Returns(false);

            // Act + Assert
            await Assert.ThrowsAsync<InvalidEmailModelException>(() => _sut.Handle(request, CancellationToken.None));

            await _repository.DidNotReceive().AddEmail(Arg.Any<EmailMessageEntity>(), Arg.Any<CancellationToken>());
            await _smtpClient.DidNotReceive().ConnectAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<SecureSocketOptions>(), Arg.Any<CancellationToken>());
            await _smtpClient.DidNotReceive().SendAsync(Arg.Any<MimeMessage>(), Arg.Any<CancellationToken>());
            await _smtpClient.DidNotReceive().DisconnectAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Проверяет, что обработчик выбрасывает исключение при ошибке SMTP и не сохраняет данные.
        /// </summary>
        /// <returns>Задача.</returns>
        [Fact]
        public async Task Handle_SmtpFailure_ThrowsInvalidOperationException_AndDoesNotPersist()
        {
            // Arrange
            var request = CreateValidRequest();

            _razorEngine.CompileRenderAsync(Arg.Any<string>(), Arg.Any<EmailMessage>()).Returns("<html></html>");
            _validator.Validate(Arg.Any<EmailMessage>()).Returns(true);

            _smtpClient
                .ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, Arg.Any<CancellationToken>())
                .Returns(_ => Task.FromException(new InvalidOperationException()));

            // Act + Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Handle(request, CancellationToken.None));

            await _smtpClient.Received(1).ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, Arg.Any<CancellationToken>());
            await _razorEngine.Received(1).CompileRenderAsync(Arg.Is<string>(k => k.EndsWith(".cshtml")), Arg.Any<EmailMessage>());

            await _repository.DidNotReceive().AddEmail(Arg.Any<EmailMessageEntity>(), Arg.Any<CancellationToken>());
            await _smtpClient.DidNotReceive().SendAsync(Arg.Any<MimeMessage>(), Arg.Any<CancellationToken>());
            await _smtpClient.DidNotReceive().DisconnectAsync(Arg.Any<bool>(), Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Проверяет, что обработчик выбрасывает исключение при ошибке в репозитории.
        /// </summary>
        /// <returns>Задача.</returns>
        [Fact]
        public async Task Handle_RepositoryFailure_ThrowsInvalidOperationException()
        {
            // Arrange
            var request = CreateValidRequest();

            _razorEngine.CompileRenderAsync(Arg.Any<string>(), Arg.Any<EmailMessage>()).Returns("<html></html>");
            _validator.Validate(Arg.Any<EmailMessage>()).Returns(true);

            _repository.AddEmail(Arg.Any<EmailMessageEntity>(), Arg.Any<CancellationToken>())
                .Returns(_ => Task.FromException(new InvalidOperationException()));

            // Act + Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Handle(request, CancellationToken.None));

            await _smtpClient.Received(1).ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, Arg.Any<CancellationToken>());
            await _smtpClient.Received(1).AuthenticateAsync(
                Arg.Any<Encoding>(),
                Arg.Is<NetworkCredential>(c =>
                    c.UserName == _settings.Username &&
                    c.Password == _settings.Password),
                Arg.Any<CancellationToken>());

            await _smtpClient.Received(1).SendAsync(Arg.Any<MimeMessage>(), Arg.Any<CancellationToken>());
            await _smtpClient.Received(1).DisconnectAsync(true, Arg.Any<CancellationToken>());
        }

        private static SendEmailRequest CreateValidRequest()
        {
            return new SendEmailRequest
            {
                Subject = "Subject",
                Recipients = new List<string> { "user1@example.com", "user2@example.com" },
                EmailTemplateType = ApiTypes.EmailTemplateType.WithButton,
                TemplateParams = new Dictionary<ApiTypes.EmailTemplateParamType, ApiContracts.EmailParamNode>
                {
                    [ApiTypes.EmailTemplateParamType.Header] = new() { Value = "Header" },
                    [ApiTypes.EmailTemplateParamType.Body] = new() { Value = "Body" },
                    [ApiTypes.EmailTemplateParamType.Button] = new()
                    {
                        Childs = new Dictionary<ApiTypes.EmailTemplateParamType, ApiContracts.EmailParamNode>
                        {
                            [ApiTypes.EmailTemplateParamType.Url] = new() { Value = "https://example.com" },
                            [ApiTypes.EmailTemplateParamType.Text] = new() { Value = "Click me" },
                        },
                    },
                },
            };
        }

        private static IOptionsMonitor<SmtpSettings> CreateOptions(SmtpSettings settings)
        {
            var options = Substitute.For<IOptionsMonitor<SmtpSettings>>();
            options.CurrentValue.Returns(settings);
            return options;
        }
    }
}