using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NotesApi.DbModels;



public class Account
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(255)] public string Firstname { get; set; }
    
    [Required, MaxLength(255)]
    public string Lastname { get; set; }
    
    [Required, MaxLength(255)]
    public string Username { get; set; }
    
    [Required]
    public string PasswordHash { get; set; } 
    
    [Required]
    public byte[] PasswordSalt { get; set; }
    
    [Required, MaxLength(255)]
    public string Email { get; set; }
    
    [Required]
    public Role Role { get; set; }
    
    [Required] 
    public DateTime CreatedDate { get; set; }
    
    public DateTime? UpdatedDate { get; set; }
    public DateTime? LastEntryDate { get; set; }

    public string? AvatarPath { get; set; }
    public bool IsVerified { get; set; }
    
    public virtual ICollection<Note> CreatedNotesList { get; set; } 
    public virtual ICollection<NoteTags> NoteTags { get; set; } 
    public virtual ICollection<RefreshToken> Tokens { get; set; }
    public virtual ICollection<Tag> Tags { get; set; } 
}