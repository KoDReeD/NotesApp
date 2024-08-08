using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApi.DbModels;
using NotesApi.Middleware;
using NotesApi.Models.Accounts.Request;
using NotesApi.Models.Jwt.Response;

namespace NotesApi.Services;

public interface IAccountService
{
    Task Registration(RegisterRequest model, Role role);
    Task<JwtResponse> Authorize(AuthorizeRequest model, string ipAddress, string browser);
}

public class AccountServices : IAccountService
{
    private readonly ApplicatonDbContext _context;
    private readonly IMapper _mapper;
    private readonly JwtOptions _jwtOptions;
    private readonly JwtHelper _jwtHelper;

    public AccountServices(ApplicatonDbContext context, IMapper mapper, JwtOptions jwtOptions, JwtHelper jwtHelper)
    {
        _context = context;
        _mapper = mapper;
        _jwtOptions = jwtOptions;
        _jwtHelper = jwtHelper;
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

    public async Task<JwtResponse> Authorize(AuthorizeRequest model, string ipAddress, string browser)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == model.Email);
        if (account == null) throw new AppException("Email или пароль введён не верно");
        var isValidData = AccountHelper.VerifyPassword(model.Password, account.PasswordSalt, account.PasswordHash);
        if(!isValidData) throw new AppException("Email или пароль введён не верно");
        var token = _jwtHelper.CreateToken(_jwtOptions, account);
        return token;
    }
}