namespace NotesApi.Models.Accounts.Response;

public class AuthorizeResponse
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public DateTime CreateAccessTokenDate { get; set; }
    public DateTime AccessTokenExpirationDate { get; set; }
    public string RefreshToken { get; set; }
    public DateTime CreateRefreshTokenDate { get; set; }
    public DateTime RefreshTokenExpirationDate { get; set; }
}