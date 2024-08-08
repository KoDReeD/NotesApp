using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using NotesApi.DbModels;
using NotesApi.Models.Accounts.Request;
using NotesApi.Models.Jwt.Response;

namespace NotesApi;

public record class JwtOptions(
    string SigningKey,
    int ExpirationMinutes
);

public class JwtHelper
{
    private readonly IMapper _mapper;

    public JwtHelper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public JwtResponse CreateToken(JwtOptions jwtOptions, Account account)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var roleString = AccountHelper.GetStringRole(account.Role);
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.Name, account.Username),
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Role, roleString)
        };

        var expirationDate = DateTime.Now.AddSeconds(jwtOptions.ExpirationMinutes);

        var token = new JwtSecurityToken
        (
            claims: claims,
            expires: expirationDate,
            signingCredentials: credentials
        );
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        var response = _mapper.Map<JwtResponse>(account);
        response.CreateDate = DateTime.Now;
        response.ExpirationDate = expirationDate;
        response.Token = tokenString;
        return response;
    }
}