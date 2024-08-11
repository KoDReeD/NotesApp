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
    
    public int AccountCreatedId { get; set; }
    
    [ForeignKey("AccountCreatedId")]
    public virtual Account AccountCreated { get; set; }
    
    public DateTime? UpdatedDate { get; set; }
    
    [MaxLength(7)] // Длина строки для HEX цвета (#RRGGBB)
    public string? Color { get; set; }
    
    public ICollection<NoteTags> NoteTags { get; set; }
}