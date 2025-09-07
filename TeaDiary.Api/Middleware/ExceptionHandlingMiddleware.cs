using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using TeaDiary.Api.Exceptions;
using TeaDiary.Api.Models;

namespace TeaDiary.Api.Middleware
{
    /// <summary>
    /// Middleware для обработки исключений.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiErrorResponse
                {
                    Code = "not_found",
                    Message = ex.Message
                }));
            }
            catch (ForbiddenException ex)
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiErrorResponse
                {
                    Code = "forbidden",
                    Message = ex.Message
                }));
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiErrorResponse
                {
                    Code = "internal_error",
                    Message = "Внутренняя ошибка сервера."
                }));
            }
        }

    }
}
