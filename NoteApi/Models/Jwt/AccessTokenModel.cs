namespace NotesApi.Models.Jwt.Response;

public class AccessTokenModel
{
    public string AccessToken { get; set; }
    public DateTime CreateAccessTokenDate { get; set; }
    public DateTime ExpirationDateAccessToken { get; set; }
}