using Carting.Core.Exceptions;
using Carting.Web.Exceptions;

namespace Carting.Web.Middlewares;

public class GlobalExceptionHandlerMiddleware(RequestDelegate _next, ILogger<GlobalExceptionHandlerMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogInformation(ex, ex.Message);
            await HandleExceptionAsync(context, ex, StatusCodes.Status404NotFound);
        }
        catch (EntityExistsException ex)
        {
            _logger.LogWarning(ex, ex.Message);
            await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var errorDetails = new ErrorDetails
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message
        };

        return context.Response.WriteAsync(errorDetails.ToString());
    }
}