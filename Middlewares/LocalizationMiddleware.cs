using System.Globalization;
using Examen.Resources;
using Microsoft.Extensions.Localization;
namespace Examen.Middlewares;

    public class LocalizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer _localizer;

        public LocalizationMiddleware (RequestDelegate next, IStringLocalizerFactory localizerFactory)
        {
            _next = next;
            _localizer = localizerFactory.Create("Messages", typeof(Program).Assembly.FullName);        }


        public async Task InvokeAsync(HttpContext context)
        {
            var language = context.Request.Headers["Accept-Language"].ToString();

            if (!string.IsNullOrEmpty(language))
            {
                try
                {
                    var culture = new CultureInfo(language);
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                }
                catch (CultureNotFoundException)
                {
                    var defaultCulture = new CultureInfo("fr");
                    CultureInfo.CurrentCulture = defaultCulture;
                    CultureInfo.CurrentUICulture = defaultCulture;
                }
            }

            await _next(context);
        }
    }
