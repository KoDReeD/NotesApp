using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApi.DbModels;

public class Note
{
    [Key]
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Text { get; set; }
    
    [Required]
    public DateTime CreatedDate { get; set; }
    
    public int WhoCreatedId { get; set; }
    
    [ForeignKey("WhoCreatedId")]
    public virtual Account WhoCreated { get; set; }
    
    public DateTime? UpdatedDate { get; set; }
    public int? WhoUpdatedId { get; set; }
    
    [ForeignKey("WhoUpdatedId")]
    public virtual Account? WhoUpdated { get; set; }
    
    [MaxLength(7)] // Длина строки для HEX цвета (#RRGGBB)
    public string? Color { get; set; }

    public ICollection<NoteTags> Tags { get; set; }
}