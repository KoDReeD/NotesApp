using System.ComponentModel.DataAnnotations;

namespace NotesApi.Models.Accounts.Request;

public class RegisterRequest
{
    [Required] public string Username { get; set; }

    [Required, EmailAddress] public string Email { get; set; }

    [Required, MaxLength(255)] public string Firstname { get; set; }

    [Required, MaxLength(255)] public string Lastname { get; set; }

    [Required] public string Password { get; set; }
}