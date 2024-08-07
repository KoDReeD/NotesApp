using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApi.DbModels;
using NotesApi.Middleware;
using NotesApi.Models.Accounts.Request;

namespace NotesApi.Services;

public interface IAccountService
{
    Task Registration(RegisterRequest model, Role role);
    Task<bool> Authorize(AuthorizeRequest model, string ipAddress, string browser);
}

public class AccountServices : IAccountService
{
    private readonly ApplicatonDbContext _context;
    private readonly IMapper _mapper;

    public AccountServices(ApplicatonDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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

    public async Task<bool> Authorize(AuthorizeRequest model, string ipAddress, string browser)
    {
        var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user == null) throw new AppException("Email или пароль введён не верно");
        var isValidData = AccountHelper.VerifyPassword(model.Password, user.PasswordSalt, user.PasswordHash);
        // if(!isValidData) throw new AppException("Email или пароль введён не верно");
        return isValidData;

    }
}