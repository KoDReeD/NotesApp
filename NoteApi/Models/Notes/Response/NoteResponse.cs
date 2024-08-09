namespace NotesApi.Models.Notes.Response;

public class NoteResponse
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Text { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public int WhoCreatedId { get; set; }
    
    public DateTime? UpdatedDate { get; set; }
    public int? WhoUpdatedId { get; set; }
    
    public string? Color { get; set; }
}