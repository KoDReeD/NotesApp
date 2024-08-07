using System.ComponentModel.DataAnnotations;

namespace NotesApi.Models.Accounts.Request;

public class RevokeTokenRequest
{
    [Required] public string RefreshToken { get; set; }
}