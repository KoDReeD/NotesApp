using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApi.DbModels;

public class Tag
{
    [Key] public int Id { get; set; }
    [Required, MaxLength(255)] public string Title { get; set; }
    [Required] public DateTime CreatedDate { get; set; }
    [Required] public DateTime UpdatedDate { get; set; }
    public int AccountCreatedId { get; set; }

    [ForeignKey("AccountCreatedId")]
    public virtual Account AccountCreated { get; set; }

    [MaxLength(7)] // Длина строки для HEX цвета (#RRGGBB)
    public string? Color { get; set; }

    public ICollection<NoteTags> NoteTags { get; set; }
}