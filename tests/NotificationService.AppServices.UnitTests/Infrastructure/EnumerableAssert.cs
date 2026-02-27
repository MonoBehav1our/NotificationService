using Xunit;

namespace NotificationService.AppServices.UnitTests.Infrastructure
{
    /// <summary>
    /// Содержит вспомогательные методы для проверки равенства коллекций.
    /// </summary>
    public static class EnumerableAssert
    {
        /// <summary>
        /// Проверяет равенство двух коллекций с использованием заданного метода сравнения элементов.
        /// </summary>
        /// <typeparam name="T">Тип элементов ожидаемой коллекции.</typeparam>
        /// <typeparam name="TK">Тип элементов актуальной коллекции.</typeparam>
        /// <param name="expected">Ожидаемая коллекция.</param>
        /// <param name="actual">Актуальная коллекция.</param>
        /// <param name="assert">Метод сравнения элементов.</param>
        public static void Equals<T, TK>(IEnumerable<T> expected, IEnumerable<TK> actual, Action<T, TK> assert)
        {
            if (expected is null)
            {
                Assert.Null(actual);
            }

            Assert.NotNull(actual);

            var expectedList = expected.ToList();
            var actualList = actual.ToList();

            Assert.Equal(expectedList.Count, actualList.Count);

            foreach (var (expectedItem, actualItem) in expectedList.Zip(actualList))
            {
                assert(expectedItem, actualItem);
            }
        }
    }
}