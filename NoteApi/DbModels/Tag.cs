using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApi.DbModels;

public class Tag
{
    [Key] 
    public int Id { get; set; }
    
    [Required, MaxLength(255)]
    public string Title { get; set; }
    
    public int WhoCreatedId { get; set; }
    
    [ForeignKey("WhoCreatedId")]
    public virtual Account WhoCreated { get; set; }
    
    [MaxLength(7)] // Длина строки для HEX цвета (#RRGGBB)
    public string? Color { get; set; }
    
    public ICollection<NoteTags> Notes { get; set; }
}