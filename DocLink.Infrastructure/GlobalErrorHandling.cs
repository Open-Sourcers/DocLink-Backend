using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DocLink.Infrastructure
{
    public class GlobalErrorHandling : IExceptionHandler
    {
        private readonly ILogger<GlobalErrorHandling> _logger;

        public GlobalErrorHandling(ILogger<GlobalErrorHandling> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception.Message);

            var details = new ProblemDetails()
            {
                Detail = $"API Error {exception.Message}",
                Instance = "API",
                Status = StatusCodes.Status500InternalServerError,
                Title = "API Error",
                Type = "Server Error"
            };

            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;
        }
    }
}
