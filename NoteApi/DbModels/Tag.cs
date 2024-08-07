using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NotesApi.DbModels;

public class Tag
{
    [Key] 
    public int Id { get; set; }
    
    [Required, MaxLength(255)]
    public string Title { get; set; }
    
    [MaxLength(7)] // Длина строки для HEX цвета (#RRGGBB)
    public string? Color { get; set; }
    
    public List<NoteTags> Notes { get; set; } = new List<NoteTags>();
}