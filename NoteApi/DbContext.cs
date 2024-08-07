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
            .HasOne(n => n.WhoCreated)
            .WithMany(a => a.CreatedNotesList)
            .HasForeignKey(n => n.WhoCreatedId);

        modelBuilder.Entity<Note>()
            .HasOne(n => n.WhoUpdated)
            .WithMany(a => a.UpdatedNotesList)
            .HasForeignKey(n => n.WhoUpdatedId);
    }
}