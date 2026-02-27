using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NotificationService.Entities.Emails;
using NotificationService.Entities.Emails.Types;

namespace NotificationService.Entities.Contexts
{
    /// <summary>
    /// Контекст для работы с базой данных.
    /// </summary>
    public class WelwiseGamesDbContext : DbContext
    {
        /// <summary>
        /// DbSet email записей.
        /// </summary>
        public DbSet<EmailMessageEntity> Emails { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WelwiseGamesDbContext"/> class.
        /// </summary>
        /// <param name="options">Настройки для контекста базы данных.</param>
        public WelwiseGamesDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Конфигурация ef.
        /// </summary>
        /// <param name="modelBuilder"><see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
            };

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                modelBuilder.Entity<EmailMessageEntity>(entity =>
                {
                    entity.Property(e => e.TemplateParams)
                        .HasColumnType("jsonb")
                        .HasConversion(
                            v => JsonSerializer.Serialize(v, jsonOptions),
                            v => JsonSerializer.Deserialize<Dictionary<EmailTemplateParamType, EmailParamNode>>(v, jsonOptions));

                    entity.Property(e => e.Recipients)
                        .HasConversion(
                            v => JsonSerializer.Serialize(v, jsonOptions),
                            v => JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>());
                });
            }
            else
            {
                modelBuilder.Entity<EmailMessageEntity>(entity =>
                {
                    entity.Property(e => e.TemplateParams)
                        .HasColumnType("jsonb")
                        .HasConversion(
                            v => JsonSerializer.Serialize(v, jsonOptions),
                            v => JsonSerializer.Deserialize<Dictionary<EmailTemplateParamType, EmailParamNode>>(v, jsonOptions));

                    entity.Property(e => e.Recipients)
                        .HasColumnType("text[]");
                });
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}