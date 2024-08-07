using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApi.DbModels;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }

    public int AccountId { get; set; }
    
    [ForeignKey("AccountId")]
    public virtual Account Account { get; set; }
    
    public string Token { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpiresDate { get; set; }
    public string CreatedIp { get; set; }
    public string? CreatedBrowser { get; set; }
    
    public int Status { get; set; }
    public DateTime? ChangeStatusDate { get; set; }
    public string? ChangeStatusIp { get; set; }
}

public enum TokenStatus
{
    created,
    revoked,
    user
}