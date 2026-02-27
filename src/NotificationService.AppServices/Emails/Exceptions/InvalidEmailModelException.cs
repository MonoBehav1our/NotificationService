namespace NotificationService.AppServices.Emails.Exceptions
{
    /// <summary>
    /// Исключение, сигнализирующее о невалидной модели email.
    /// </summary>
    public class InvalidEmailModelException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEmailModelException"/> class.
        /// </summary>
        /// <param name="message">Текст ошибки.</param>
        public InvalidEmailModelException(string message)
            : base(message)
        {
        }
    }
}