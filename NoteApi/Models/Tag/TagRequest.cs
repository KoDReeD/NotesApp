using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotesApi.DbModels;

namespace NotesApi.Models.Tag;

public class TagRequest
{
    [Required, MaxLength(255)]
    public string Title { get; set; }
    
    [MaxLength(7)] // Длина строки для HEX цвета (#RRGGBB)
    public string? Color { get; set; }
}
