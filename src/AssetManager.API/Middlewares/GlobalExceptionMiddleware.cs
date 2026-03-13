using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using FluentValidation;

namespace AssetManager.API.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Varsayılan hata: 500 Internal Server Error
        var statusCode = (int)HttpStatusCode.InternalServerError;
        object response;

        // EĞER HATA BİR VALIDASYON HATASIYSA:
        if (exception is FluentValidation.ValidationException validationException)
        {
            statusCode = (int)HttpStatusCode.BadRequest; // 400
            response = new
            {
                StatusCode = statusCode,
                Message = "Validation Failed",
                Errors = validationException.Errors.Select(x => new
                {
                    Property = x.PropertyName,
                    Error = x.ErrorMessage
                })
            };
        }
        else
        {
            // Genel hatalar için
            response = new
            {
                StatusCode = statusCode,
                Message = "An internal server error occurred.",
                Detail = "An unexpected error occurred." 
            };
        }

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}