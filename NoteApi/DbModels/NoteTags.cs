using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApi.DbModels;

public class NoteTags
{
    public int Id { get; set; }

    public int NoteId { get; set; }
    public virtual Note Note { get; set; }

    public int TagId { get; set; }

    public virtual Tag Tag { get; set; }

    public int AccountCreatedId { get; set; }

    public virtual Account AccountCreated { get; set; }
}