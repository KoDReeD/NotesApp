using Microsoft.EntityFrameworkCore;
using NotesApi.DbModels;

namespace NotesApi;

public class ApplicatonDbContext : DbContext
{
    public ApplicatonDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<RefreshToken> RefreshToken { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<NoteTags> NoteTags { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>()
            .HasOne(n => n.AccountCreated)
            .WithMany(a => a.CreatedNotesList)
            .HasForeignKey(n => n.AccountCreatedId);

        modelBuilder.Entity<NoteTags>()
            .HasKey(nt => new { nt.TagId, nt.NoteId });
        
        modelBuilder.Entity<NoteTags>()
            .HasOne(nt => nt.Note)
            .WithMany(n => n.NoteTags)
            .HasForeignKey(nt => nt.NoteId);

        modelBuilder.Entity<NoteTags>()
            .HasOne(nt => nt.Tag)
            .WithMany(t => t.NoteTags)
            .HasForeignKey(nt => nt.TagId);
        
        modelBuilder.Entity<NoteTags>()
            .HasOne(nt => nt.AccountCreated)
            .WithMany(a => a.NoteTags)
            .HasForeignKey(nt => nt.AccountCreatedId);
    }
}