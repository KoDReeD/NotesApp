using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApi.DbModels;

public class NoteTags
{
    public int Id { get; set; }
    
    public int NoteId { get; set; }
    
    [ForeignKey("NoteId")]
    public virtual Note Note { get; set; } 
    
    public int TagId { get; set; }
    
    [ForeignKey("TagId")]
    public virtual Tag Tag { get; set; }
    
    public int WhoCreatedId { get; set; }
    
    [ForeignKey("WhoCreatedId")]
    public virtual Account WhoCreated { get; set; }
}