using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using NotesApi.DbModels;
using NotesApi.Models.Accounts.Request;
using NotesApi.Models.Jwt.Response;

namespace NotesApi;

public record class JwtOptions(
    string SigningKey,
    int TokenValidityInMinutes,
    int RefreshTokenValidityInDays
);

public class TokenHelper
{
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContext;
    private readonly Account _currentUser;
    private readonly JwtOptions _jwtOptions;
    private readonly ApplicatonDbContext _context;

    public TokenHelper(IMapper mapper, IHttpContextAccessor httpContext, JwtOptions jwtOptions,
        ApplicatonDbContext context)
    {
        _mapper = mapper;
        _httpContext = httpContext;
        _jwtOptions = jwtOptions;
        _context = context;
        _currentUser = (Account)httpContext.HttpContext.Items["Account"];
    }

    public AccessTokenModel CreateJwtToken(JwtOptions jwtOptions, Account account)
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

        var expirationDate = DateTime.Now.AddSeconds(jwtOptions.TokenValidityInMinutes);

        var token = new JwtSecurityToken
        (
            claims: claims,
            expires: expirationDate,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return new AccessTokenModel()
        {
            AccessToken = tokenString,
            CreateAccessTokenDate = DateTime.Now,
            ExpirationDateAccessToken = expirationDate
        };
    }

    public async Task<RefreshToken> GenerateRefreshToken(string ipAddress, string browser, Account account)
    {
        // Создаем случайные байты
        byte[] randomBytes = new byte[32];
        using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
        {
            rngCryptoServiceProvider.GetBytes(randomBytes);
        }

        string refreshToken;
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(randomBytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("X2"));
            }

            refreshToken = sb.ToString();
        }

        var expirationDate = DateTime.Now.AddDays(_jwtOptions.RefreshTokenValidityInDays);
        var dbRefresh = new RefreshToken()
        {
            AccountId = account.Id,
            Token = refreshToken,
            CreatedDate = DateTime.Now,
            ExpiresDate = expirationDate,
            CreatedIp = ipAddress,
            CreatedBrowser = browser,
            Status = RefreshTokenStatus.Created
        };
        await _context.RefreshToken.AddAsync(dbRefresh);
        await _context.SaveChangesAsync();
        return dbRefresh;
    }

    public static TokenValidationParameters TokenValidationParameters;
}