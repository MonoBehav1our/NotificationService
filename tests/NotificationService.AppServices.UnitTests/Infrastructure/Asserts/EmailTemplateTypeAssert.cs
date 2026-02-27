using Xunit;
using ApiTypes = NotificationService.Api.Contracts.Emails.Types;
using EntitiesTypes = NotificationService.Entities.Emails.Types;

namespace NotificationService.AppServices.UnitTests.Infrastructure.Asserts
{
    /// <summary>
    /// Содержит методы для проверки равенства типов email шаблонов.
    /// </summary>
    public static class EmailTemplateTypeAssert
    {
        /// <summary>
        /// Проверяет равенство между типом шаблона email сущности и типом API.
        /// </summary>
        /// <param name="expected">Ожидаемый тип шаблона email сущности.</param>
        /// <param name="actual">Актуальный тип API.</param>
        public static void Equal(EntitiesTypes.EmailTemplateType expected, ApiTypes.EmailTemplateType actual)
        {
            var emailTemplateTypeExpected = expected switch
            {
                EntitiesTypes.EmailTemplateType.WithButton => ApiTypes.EmailTemplateType.WithButton,
                _ => ApiTypes.EmailTemplateType.Unknown,
            };

            Assert.Equal(emailTemplateTypeExpected, actual);
        }
    }
}