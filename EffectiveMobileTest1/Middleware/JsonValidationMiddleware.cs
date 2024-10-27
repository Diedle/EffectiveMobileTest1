using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace EffectiveMobileTest1.Middleware
{
    public class JsonValidationMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="JsonValidationMiddleware"/>.
        /// </summary>
        /// <param name="next">Следующий делегат в конвейере обработки запросов.</param>
        public JsonValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Обрабатывает запросы и проверяет валидность JSON данных.
        /// </summary>
        /// <param name="context">Контекст HTTP запроса.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
           /* try
            {
                
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500; // Internal Server Error
                await context.Response.WriteAsync($"An unexpected error occurred: {ex.Message}");
            }*/
        }
    }
}
