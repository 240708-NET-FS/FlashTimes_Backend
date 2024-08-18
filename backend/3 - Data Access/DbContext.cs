using Microsoft.EntityFrameworkCore;

namespace FlashTimes.Entities;

public class FlashTimesDbContext : DbContext
{
    public FlashTimesDbContext(DbContextOptions<FlashTimesDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Set> Sets { get; set; }
    public DbSet<Flashcard> Flashcards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Configuring entity properties using Fluent API
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId); // Primary key
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => e.SetId); // Primary key
            entity.Property(e => e.SetName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.SetLength)
                .IsRequired();
            entity.HasOne(e => e.Author)
                .WithMany(u => u.Sets)
                .HasForeignKey(e => e.UserId); // Foreign key relationship
        });

        modelBuilder.Entity<Flashcard>(entity =>
        {
            entity.HasKey(e => new { e.SetId, e.Question }); // Composite key
            entity.Property(e => e.Answer)
                .IsRequired();
            entity.HasOne(e => e.Author) //Author gets resolved to User as in Author is of type User
                .WithMany(u => u.Flashcards) //The User Entity will contain the navigation property for Flashcards
                .HasForeignKey(e => e.UserId); // Foreign key relationship
        });

        // Seed data or additional configurations will go here later on.
    }
}

