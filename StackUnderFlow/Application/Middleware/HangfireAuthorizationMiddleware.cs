using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace StackUnderFlow.Application.Middleware;

public class HangfireAuthorizationMiddleware : IDashboardAuthorizationFilter
{
    private const string Username = "Admin"; 
    private const string Password = "AdminPass"; 

    public bool Authorize([NotNull] DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        var header = httpContext.Request.Headers.Authorization;

        if (header.Count == 0)
        {
            return ChallengeAuth(httpContext);
        }

        var authValues = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header!);

        if (!"Basic".Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase))
        {
            return ChallengeAuth(httpContext);
        }

        var parameter = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter!));
        var parts = parameter.Split(':');

        if (parts.Length < 2)
        {
            return ChallengeAuth(httpContext);
        }

        var username = parts[0];
        var password = parts[1];

        if (username == Username && password == Password)
        {
            return true;
        }

        return ChallengeAuth(httpContext);
    }

    private static bool ChallengeAuth(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = 401;
        httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
        httpContext.Response.WriteAsync("Authentication is required.");

        return false;
    }
}