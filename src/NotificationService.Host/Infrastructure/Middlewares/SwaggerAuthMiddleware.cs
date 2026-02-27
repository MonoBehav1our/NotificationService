using System.Text;

namespace NotificationService.Host.Infrastructure.Middlewares
{
    /// <summary>
    /// Middleware для защиты сваггера паролем.
    /// </summary>
    public class SwaggerAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerAuthMiddleware"/> class.
        /// </summary>
        /// <param name="next"><see cref="RequestDelegate"/>.</param>
        /// <param name="configuration"><see cref="IConfiguration"/>.</param>
        public SwaggerAuthMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        /// <summary>
        /// Вызвать middleware.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/>.</param>
        /// <returns>Задача.</returns>
        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant();
            if (path != null && path.StartsWith("/notification-service/swagger"))
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic "))
                {
                    context.Response.Headers["WWW-Authenticate"] = "Basic";
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

                var encodedCredentials = authHeader["Basic ".Length..].Trim();
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials)).Split(':');

                var username = _configuration["SwaggerAuth:Username"];
                var password = _configuration["SwaggerAuth:Password"];

                if (credentials.Length != 2 || credentials[0] != username || credentials[1] != password)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            }

            await _next(context);
        }
    }
}