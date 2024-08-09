using System.ComponentModel.DataAnnotations;
using NotesApi.DbModels;

namespace NotesApi.Models.Notes.Request;

public class NoteRequest
{
    public string Title { get; set; }
    
    public string Text { get; set; }
    
    [MaxLength(7)] // Длина строки для HEX цвета (#RRGGBB)
    public string? Color { get; set; }
    
    public ICollection<NoteTagItemsRequest> Tags { get; set; }
}

public class NoteTagItemsRequest
{
    [Required]
    public int Id { get; set; }
}