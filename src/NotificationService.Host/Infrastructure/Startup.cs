using System.Reflection;
using System.Text.Json.Serialization;
using MailKit.Net.Smtp;
using Microsoft.OpenApi.Models;
using NotificationService.AppServices.Infrastructure;
using NotificationService.Entities.Infrastructure;
using NotificationService.Host.Infrastructure.Extensions;
using NotificationService.Host.Infrastructure.Middlewares;
using NotificationService.RazorTemplates.Infrastructure;

namespace NotificationService.Host.Infrastructure
{
    /// <summary>
    /// Стартап приложения.
    /// </summary>
    public class Startup
    {
        private IConfiguration Configuration { get; }

        private IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        /// <param name="webHostEnvironment"><see cref="IWebHostEnvironment"/>.</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Сконфигурировать сервисы в проекте.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddAppServices(Configuration);

            services.AddDbContext(Configuration);

            services.AddLoggerService(Configuration);

            services.AddTransient<SmtpClient>();

            services.AddEmailModelsValidator();

            var swaggerSettings = Configuration.GetSection(nameof(SwaggerDescription)).Get<SwaggerDescription>();

            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.SwaggerDoc(swaggerSettings.Version, new OpenApiInfo
                {
                    Title = swaggerSettings.Title,
                    Description = swaggerSettings.Description,
                    Version = swaggerSettings.Version,
                });

                options.UseInlineDefinitionsForEnums();
            });

            NpgsqlStaticSettings.EnableEnumMapping();
        }

        /// <summary>
        /// Сконфигурировать приложение.
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
        /// <param name="env"><see cref="IWebHostEnvironment"/>.</param>
        /// <param name="lifetime"><see cref="IHostApplicationLifetime"/>.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            var swaggerEnabled = Configuration.GetValue<bool>("Swagger:Enabled");

            if (swaggerEnabled)
            {
                app.UseMiddleware<SwaggerAuthMiddleware>();

                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "notification-service/swagger/{documentName}/swagger.json";
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/notification-service/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = "notification-service/swagger";
                });
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}