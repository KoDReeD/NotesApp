using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NotesApi.DbModels;
using NotesApi.Models.Accounts.Request;
using NotesApi.Services;
using UAParser;

namespace NotesApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthorizeController : ControllerBase
{
    private IAccountService _accountService;

    public AuthorizeController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest model)
    {
        await _accountService.Registration(model, Role.User);
        return Ok(new { message = "Вы успешно зарегистрировались" });
    }

    [HttpPost]
    public async Task<IActionResult> Authorize(AuthorizeRequest model)
    {
        var result = await _accountService.Authorize(model, GetIpAddress(), GetBrowser());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> RefreshToken(string refreshToken)
    {
        var result = await _accountService.RefreshToken(refreshToken, GetIpAddress(), GetBrowser());
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> RevokeToken(string refreshToken)
    {
        var result = await _accountService.RevokeToken(refreshToken, GetIpAddress());
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CheckToken(string accessToken)
    {
        var result = _accountService.CheckToken(accessToken);
        return Ok(result);
    }

    private string GetIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        else
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }

    private string GetBrowser()
    {
        var userAgent = HttpContext.Request.Headers["User-Agent"];
        string uaString = Convert.ToString(userAgent[0]);
        var uaParser = Parser.GetDefault();
        ClientInfo c = uaParser.Parse(uaString);
        return c.UA.ToString();
    }
}