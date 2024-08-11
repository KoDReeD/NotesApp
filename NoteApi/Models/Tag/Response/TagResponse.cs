using System.Text.Json.Serialization;
using NotesApi.DbModels;

namespace NotesApi.Models.Tag.Response;

public class TagResponse
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string? Color { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public int WhoCreatedId { get; set; }
    
    [JsonIgnore]
    public ICollection<NoteTags> Notes { get; set; }
}