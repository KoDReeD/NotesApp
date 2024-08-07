using System.ComponentModel.DataAnnotations;

namespace NotesApi.Models.Accounts.Request;

public class AuthorizeRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}