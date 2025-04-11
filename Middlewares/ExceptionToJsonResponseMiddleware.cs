using System.Net;
using System.Text.Json;
using Examen.Exceptions;
using Microsoft.Extensions.Localization;

namespace Examen.Middlewares
{
    public class ExceptionToJsonResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionToJsonResponseMiddleware> _logger;
        private readonly IStringLocalizer _localizer;

        private static readonly Dictionary<Type[], (HttpStatusCode, string)> ExceptionMappings = new()
        {
            { new[] { typeof(ArgumentNullException), typeof(ArgumentException) }, (HttpStatusCode.BadRequest, "InvalidRequest") },
            { new[] { typeof(UnauthorizedAccessException), typeof(AccessViolationException) }, (HttpStatusCode.Unauthorized, "UnauthorizedAccess") },
            { new[] { typeof(InvalidOperationException) }, (HttpStatusCode.BadRequest, "InvalidOrMissingData") },            { new[] { typeof(KeyNotFoundException), typeof(NullReferenceException) }, (HttpStatusCode.NotFound, "ResourceNotFound") },
            { new[] { typeof(ExpiredOrDeactivatedToken)}, (HttpStatusCode.Unauthorized, "ExpiredOrDeactivatedToken") },
            { new[] { typeof(RegistrationFailedException)}, (HttpStatusCode.InternalServerError, "RegistrationFailed") },
            { new[] { typeof(FailedToAssignRoleException) }, (HttpStatusCode.InternalServerError, "FailedToAssignRole") },
            { new[] { typeof(InvalidLoginException) }, (HttpStatusCode.Unauthorized, "InvalidLogin") }
        };

        public ExceptionToJsonResponseMiddleware(RequestDelegate next, ILogger<ExceptionToJsonResponseMiddleware> logger, IStringLocalizerFactory localizerFactory)
        {
            _localizer = localizerFactory.Create("Messages", typeof(Program).Assembly.FullName);
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing the request.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogInformation("Handling exception: {Exception}", exception.GetType());
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string messageKey = "UnexpectedError";

            foreach (var entry in ExceptionMappings)
            {
                if (entry.Key.Any(type => type.IsInstanceOfType(exception)))
                {
                    (statusCode, messageKey) = entry.Value;
                    break;
                }
            }

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var errorMessage = _localizer.GetString(messageKey).Value;
            var result = JsonSerializer.Serialize(new { message = errorMessage, statusCode = (int)statusCode });

            await context.Response.WriteAsync(result);
        }
    }
}
