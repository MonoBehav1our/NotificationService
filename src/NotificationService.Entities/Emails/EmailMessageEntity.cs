using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotificationService.Entities.Common;
using NotificationService.Entities.Emails.Types;

namespace NotificationService.Entities.Emails
{
    /// <summary>
    /// Представляет запись в базе данных.
    /// </summary>
    [Table(Names.EmailMessages, Schema = Names.Schema)]
    public class EmailMessageEntity
    {
        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>
        /// Автор письма.
        /// </summary>
        [Column("author")]
        [Required]
        public string Author { get; set; }

        /// <summary>
        /// Тема письма.
        /// </summary>
        [Column("subject")]
        [Required]
        public string Subject { get; set; }

        /// <summary>
        /// Список получателей.
        /// </summary>
        [Column("recipients")]
        [Required]
        public List<string> Recipients { get; set; }

        /// <summary>
        /// Шаблон письма.
        /// </summary>
        [Column("email_template")]
        [Required]
        public EmailTemplateType EmailTemplateType { get; set; }

        /// <summary>
        /// Параметры для шаблона(подробнее в confluence).
        /// </summary>
        [Column("template_params")]
        [Required]
        public Dictionary<EmailTemplateParamType, EmailParamNode> TemplateParams { get; set; }

        /// <summary>
        /// Время отправки сообщения.
        /// </summary>
        [Column("send_time")]
        [Required]
        public DateTime SendTime { get; set; }
    }
}