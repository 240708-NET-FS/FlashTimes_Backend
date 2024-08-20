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

        // Configuring entity properties using Fluent API
        modelBuilder.Entity<User>(entity =>
        {
            // Set the primary key
            entity.HasKey(e => e.UserId);

            // Configure UserName property
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(50);

            // Configure FirstName property
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            // Configure LastName property
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);

            // Configure PasswordHash property
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            // Configure Salt property
            entity.Property(e => e.Salt)
                .HasMaxLength(50); // Adjust as needed

            // Configure CreatedAt property
            entity.Property(e => e.CreatedAt)
                .IsRequired();

            // Configure relationships and navigation properties

            // Set cascade delete behavior to restrict deletion cycles
            entity.HasMany(e => e.Sets)
                .WithOne(s => s.Author)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            // Set cascade delete behavior to restrict deletion cycles
            entity.HasMany(e => e.Flashcards)
                .WithOne(f => f.Author)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => e.SetId); // Primary key
            entity.Property(e => e.SetName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.SetLength)
                .IsRequired();

            // Set cascade delete behavior to restrict deletion cycles
            entity.HasOne(e => e.Author)
                .WithMany(u => u.Sets)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
        });

        modelBuilder.Entity<Flashcard>(entity =>
        {
            entity.HasKey(e => new { e.SetId, e.Question }); // Composite key
            entity.Property(e => e.Answer)
                .IsRequired();

            // Set cascade delete behavior to restrict deletion cycles
            entity.HasOne(e => e.Author) // Author gets resolved to User as in Author is of type User
                .WithMany(u => u.Flashcards) // The User Entity will contain the navigation property for Flashcards
                .HasForeignKey(e => e.UserId) // Foreign key relationship
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
        });

        // Seed data or additional configurations will go here later on.
    }
}
