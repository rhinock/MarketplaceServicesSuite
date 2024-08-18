using Microsoft.AspNetCore.Authentication;

namespace Carting.Web.Middlewares;

public class LogTokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogTokenMiddleware> _logger;

    public LogTokenMiddleware(RequestDelegate next, ILogger<LogTokenMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = await context.GetTokenAsync("access_token");

        if (!string.IsNullOrEmpty(token))
        {
            _logger.LogInformation($"Token: {token}");
        }

        var user = context.User;

        if (user != null && user.Identity?.IsAuthenticated == true)
        {
            _logger.LogInformation($"Authenticated user: {user.Identity.Name}");

            var claims = user.Claims;

            foreach (var claim in claims)
            {
                _logger.LogInformation($"Claim: {claim.Type}, Value: {claim.Value}");
            }
        }

        // pass to the next middleware
        await _next(context);
    }
}