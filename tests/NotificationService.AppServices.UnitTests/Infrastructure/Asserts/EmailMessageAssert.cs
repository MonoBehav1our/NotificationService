using NotificationService.Api.Contracts.Emails.Responses;
using NotificationService.Entities.Emails;
using Xunit;
using ApiContracts = NotificationService.Api.Contracts.Emails.Types;
using EntityTypes = NotificationService.Entities.Emails.Types;

namespace NotificationService.AppServices.UnitTests.Infrastructure.Asserts
{
    /// <summary>
    /// Содержит методы для проверки равенства email сообщений.
    /// </summary>
    public static class EmailMessageAssert
    {
        /// <summary>
        /// Проверяет равенство двух сущностей email сообщений.
        /// </summary>
        /// <param name="expected">Ожидаемая сущность email сообщения.</param>
        /// <param name="actual">Актуальная сущность email сообщения.</param>
        public static void Equal(EmailMessageEntity expected, EmailMessageEntity actual)
        {
            Assert.Equal(expected.Author, actual.Author);
            Assert.Equal(expected.Subject, actual.Subject);
            Assert.Equal(expected.EmailTemplateType, actual.EmailTemplateType);
            Assert.Equal(expected.SendTime, actual.SendTime);

            EnumerableAssert.Equals(expected.Recipients, actual.Recipients, (e, a) => Assert.Equal(e, a));

            Assert.Equal(expected.TemplateParams.Count, actual.TemplateParams.Count);
            foreach (var (key, expectedNode) in expected.TemplateParams)
            {
                Assert.True(actual.TemplateParams.ContainsKey(key));
                var actualNode = actual.TemplateParams[key];
                EmailParamNodeAssert.Equal(expectedNode, actualNode);
            }
        }

        /// <summary>
        /// Проверяет равенство между сущностью email сообщения и ответом API.
        /// </summary>
        /// <param name="expected">Ожидаемая сущность email сообщения.</param>
        /// <param name="actual">Актуальный ответ API.</param>
        public static void Equal(EmailMessageEntity expected, GetEmailMessageResponse actual)
        {
            Assert.Equal(expected.Author, actual.Author);
            Assert.Equal(expected.Subject, actual.Subject);
            Assert.Equal(expected.SendTime, actual.SendTime);

            EmailTemplateTypeAssert.Equal(expected.EmailTemplateType, actual.EmailTemplateType);

            EnumerableAssert.Equals(expected.Recipients, actual.Recipients, (e, a) => Assert.Equal(e, a));

            Assert.Equal(expected.TemplateParams.Count, actual.TemplateParams.Count);
            foreach (var (key, expectedNode) in expected.TemplateParams)
            {
                var apiKey = ConvertToApiParamType(key);
                Assert.True(actual.TemplateParams.ContainsKey(apiKey));
                var actualNode = actual.TemplateParams[apiKey];
                EmailParamNodeAssert.Equal(expectedNode, actualNode);
            }
        }

        private static ApiContracts.EmailTemplateParamType ConvertToApiParamType(EntityTypes.EmailTemplateParamType entityType)
        {
            return (ApiContracts.EmailTemplateParamType)entityType;
        }
    }
}