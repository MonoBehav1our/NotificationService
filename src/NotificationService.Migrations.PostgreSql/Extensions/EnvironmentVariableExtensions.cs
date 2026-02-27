namespace NotificationService.Migrations.PostgreSql.Extensions
{
    /// <summary>
    /// Класс с расширениями для подстановки перменных окружения.
    /// </summary>
    internal static class EnvironmentVariableExtensions
    {
        /// <summary>
        /// Заменить переменные окружения в строке.
        /// </summary>
        /// <param name="input">Строка с плейсхолдерами.</param>
        /// <returns>Строка с подставленными переменными окружения.</returns>
        public static string ReplaceEnvironmentVariables(this string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var variables = Environment.GetEnvironmentVariables().Cast<System.Collections.DictionaryEntry>();

            foreach (var envVar in variables)
            {
                var key = envVar.Key.ToString();
                var value = envVar.Value?.ToString();

                if (key == "DB_PORT")
                {
                    value = "5432";
                }

                input = input.Replace($"${{{key}}}", value);
            }

            return input;
        }
    }
}