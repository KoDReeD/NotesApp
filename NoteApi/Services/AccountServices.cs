using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApi.DbModels;
using NotesApi.Middleware;
using NotesApi.Models.Accounts.Request;
using NotesApi.Models.Accounts.Response;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;

namespace NotesApi.Services;

public interface IAccountService
{
    Task Registration(RegisterRequest model, Role role);
    Task<AuthorizeResponse> Authorize(AuthorizeRequest model, string ipAddress, string browser);
    public Task<AuthorizeResponse> RefreshToken(string refreshToken, string ipAddress, string browser);
    public Task<bool> RevokeToken(string refreshToken, string ipAddress);
    public bool CheckToken(string accessToken);
}

public class AccountServices : IAccountService
{
    private readonly ApplicatonDbContext _context;
    private readonly IMapper _mapper;
    private readonly JwtOptions _jwtOptions;
    private readonly TokenHelper _tokenHelper;

    public AccountServices(ApplicatonDbContext context, IMapper mapper, JwtOptions jwtOptions, TokenHelper tokenHelper)
    {
        _context = context;
        _mapper = mapper;
        _jwtOptions = jwtOptions;
        _tokenHelper = tokenHelper;
    }

    public async Task Registration(RegisterRequest model, Role role)
    {
        if (await _context.Accounts.AnyAsync(x => x.Email.Equals(model.Email)))
        {
            throw new AppException("Данная почта уже зарегистрирована");
        }

        if (await _context.Accounts.AnyAsync(x => x.Username.Equals(model.Username)))
        {
            throw new AppException("Данный username уже зарегистрирован");
        }

        var account = _mapper.Map<Account>(model);
        account.Role = role;
        account.CreatedDate = DateTime.Now.ToUniversalTime();
        var salt = AccountHelper.GenerateSalt();
        account.PasswordSalt = salt;
        account.PasswordHash = AccountHelper.HashPassword(model.Password, salt);

        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
    }

    public async Task<AuthorizeResponse> Authorize(AuthorizeRequest model, string ipAddress, string browser)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == model.Email);
        if (account == null) throw new AppException("Email или пароль введён не верно");
        var isValidData = AccountHelper.VerifyPassword(model.Password, account.PasswordSalt, account.PasswordHash);
        if (!isValidData) throw new AppException("Email или пароль введён не верно");

        return await GetTokenResponse(account, ipAddress, browser);
    }

    private async Task<AuthorizeResponse> GetTokenResponse(Account account, string ipAddress, string browser)
    {
        var tokenData = _tokenHelper.CreateJwtToken(_jwtOptions, account);
        var refreshTokenData = await _tokenHelper.GenerateRefreshToken(ipAddress, browser, account);
        AuthorizeResponse response = _mapper.Map<AuthorizeResponse>(account);
        _mapper.Map(tokenData, response);
        _mapper.Map(refreshTokenData, response);
        return response;
    }

    /// <summary>
    /// Обновить токен доступа
    /// </summary>
    /// <param name="refreshToken"></param>
    public async Task<AuthorizeResponse> RefreshToken(string refreshToken, string ipAddress, string browser)
    {
        var refreshModel = await _context.RefreshToken
                               .Include(x => x.Account)
                               .FirstOrDefaultAsync(x => x.Token == refreshToken)
                           ?? throw new KeyNotFoundException("Токен обновления не найден");
        if (refreshModel.Status != RefreshTokenStatus.Created
            || DateTime.Now > refreshModel.ExpiresDate)
        {
            throw new AppException("Недействительный токен");
        }

        refreshModel.Status = RefreshTokenStatus.Used;
        refreshModel.ChangeStatusDate = DateTime.Now;
        refreshModel.ChangeStatusIp = ipAddress;
        _context.RefreshToken.Update(refreshModel);
        await _context.SaveChangesAsync();

        return await GetTokenResponse(refreshModel.Account, ipAddress, browser);
    }

    /// <summary>
    /// Отозвать токен обновления
    /// </summary>
    /// <param name="refreshToken"></param>
    public async Task<bool> RevokeToken(string refreshToken, string ipAddress)
    {
        var refreshModel = await _context.RefreshToken
                               .Include(x => x.Account)
                               .FirstOrDefaultAsync(x => x.Token == refreshToken)
                           ?? throw new KeyNotFoundException("Токен обновления не найден");
        if (refreshModel.Status != RefreshTokenStatus.Created
            || DateTime.Now > refreshModel.ExpiresDate)
        {
            throw new AppException("Недействительный токен");
        }

        refreshModel.Status = RefreshTokenStatus.Revoked;
        refreshModel.ChangeStatusDate = DateTime.Now;
        refreshModel.ChangeStatusIp = ipAddress;
        _context.RefreshToken.Update(refreshModel);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Проверить токен доступа
    /// </summary>
    public bool CheckToken(string accessToken)
    {
        try
        {
            var tokenValidateParameters = TokenHelper.TokenValidationParameters;
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(accessToken, tokenValidateParameters, out SecurityToken validatedToken);

            return true;
        }
        catch (Exception e)
        {
            throw new AppException("Недействительный токен");
        }
    }
}