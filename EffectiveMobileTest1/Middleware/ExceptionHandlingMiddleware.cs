using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EffectiveMobileTest1.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions in the HTTP request pipeline.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        // <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next delegate in the HTTP request pipeline.</param>
        /// <param name="logger">The logger instance.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to handle exceptions and log errors.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "An unexpected error occurred at {Time}", DateTime.Now);
                context.Response.StatusCode = 500; // Internal Server Error
                await context.Response.WriteAsync($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
