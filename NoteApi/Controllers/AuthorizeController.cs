using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NotesApi.DbModels;
using NotesApi.Models.Accounts.Request;
using NotesApi.Services;

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
    public async Task<IActionResult> Authorize(AuthorizeRequest model, string ipAddress, string browser)
    {
        var result = await _accountService.Authorize(model, ipAddress, browser);
        return Ok(result);
    }
}