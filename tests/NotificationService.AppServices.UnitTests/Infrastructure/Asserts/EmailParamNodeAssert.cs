using NotificationService.Entities.Emails;
using Xunit;
using ApiContracts = NotificationService.Api.Contracts.Emails.Models;
using ApiTypes = NotificationService.Api.Contracts.Emails.Types;
using EntityTypes = NotificationService.Entities.Emails.Types;

namespace NotificationService.AppServices.UnitTests.Infrastructure.Asserts
{
    /// <summary>
    /// Содержит методы для проверки равенства узлов параметров email.
    /// </summary>
    public static class EmailParamNodeAssert
    {
        /// <summary>
        /// Проверяет равенство двух узлов параметров email.
        /// </summary>
        /// <param name="expected">Ожидаемый узел параметров.</param>
        /// <param name="actual">Актуальный узел параметров.</param>
        public static void Equal(EmailParamNode expected, EmailParamNode actual)
        {
            if (actual.Value == null)
            {
                Assert.Null(expected.Value);
                return;
            }

            Assert.Equal(expected.Value, actual.Value);

            if (expected.Childs is null)
            {
                Assert.Null(actual.Childs);
                return;
            }

            Assert.NotNull(actual.Childs);
            Assert.Equal(expected.Childs.Count, actual.Childs.Count);

            foreach (var (key, expectedChild) in expected.Childs)
            {
                Assert.True(actual.Childs.ContainsKey(key));
                var actualChild = actual.Childs[key];
                Equal(expectedChild, actualChild);
            }
        }

        /// <summary>
        /// Проверяет равенство между узлом параметров email и контрактом API.
        /// </summary>
        /// <param name="expected">Ожидаемый узел параметров.</param>
        /// <param name="actual">Актуальный контракт API.</param>
        public static void Equal(EmailParamNode expected, ApiContracts.EmailParamNode actual)
        {
            if (actual == null)
            {
                Assert.Null(expected);
                return;
            }

            if (actual.Value == null)
            {
                Assert.Null(expected.Value);
                return;
            }

            Assert.Equal(expected.Value, actual.Value);

            if (expected.Childs is null)
            {
                Assert.Null(actual.Childs);
                return;
            }

            Assert.NotNull(actual.Childs);
            Assert.Equal(expected.Childs.Count, actual.Childs.Count);

            foreach (var (key, expectedChild) in expected.Childs)
            {
                var apiKey = ConvertToApiParamType(key);
                Assert.True(actual.Childs.ContainsKey(apiKey));
                var actualChild = actual.Childs[apiKey];
                Equal(expectedChild, actualChild);
            }
        }

        private static ApiTypes.EmailTemplateParamType ConvertToApiParamType(EntityTypes.EmailTemplateParamType entityType)
        {
            return (ApiTypes.EmailTemplateParamType)entityType;
        }
    }
}