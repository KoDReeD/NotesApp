using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace NotesApi.Middleware;

public class AccountMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtOptions _jwtOptions;

    public AccountMiddleware(RequestDelegate next, JwtOptions jwtOptions)
    {
        _next = next;
        _jwtOptions = jwtOptions;
    }

    public async Task Invoke(HttpContext httpContext, ApplicatonDbContext context)
    {
        var jwtToken = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrWhiteSpace(jwtToken))
        {
            await SetUser(jwtToken,context, httpContext);
        }
        await _next(httpContext);
    }

    private async Task SetUser(string jwtToken, ApplicatonDbContext context, HttpContext httpContext)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(jwtToken).Claims;

        var userId = decodedToken.FirstOrDefault(x =>
            x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                         ?.Value 
                     ?? throw new KeyNotFoundException("Токен не валидный");
        
        var account = await context.Accounts.FirstOrDefaultAsync(x => x.Id == int.Parse(userId));
        if (account != null)
        {
            httpContext.Items["Account"] = account;
        }
    }
}