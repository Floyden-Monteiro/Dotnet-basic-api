using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Text.Json;

namespace BagAPI.Middleware
{
    public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new AuthorizationMiddlewareResultHandler();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Forbidden)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                var response = new
                {
                    Success = false,
                    Message = "Access denied. You do not have permission to perform this action."
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return; // Short-circuit the pipeline
            }

            if (authorizeResult.Challenged)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                var response = new
                {
                    Success = false,
                    Message = "Unauthorized. Please provide a valid token."
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return; // Short-circuit the pipeline
            }

            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
