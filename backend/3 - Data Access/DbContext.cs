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
    // Setting FlashcardId as the primary key
    entity.HasKey(e => e.FlashcardId);

    // Configuring the Question property without any max length or required constraints
    entity.Property(e => e.Question);

    // Configuring the Answer property without any required constraints
    entity.Property(e => e.Answer);

    // Configuring the relationship with the Set entity
    entity.HasOne(e => e.Set)
        .WithMany(s => s.Flashcards) // A Set can have many Flashcards
        .HasForeignKey(e => e.SetId) // Flashcard references Set via SetId
        .OnDelete(DeleteBehavior.Cascade); // If a Set is deleted, all associated Flashcards are also deleted

    // Configuring the relationship with the User entity (Author)
    entity.HasOne(e => e.Author)
        .WithMany(u => u.Flashcards) // A User (Author) can have many Flashcards
        .HasForeignKey(e => e.UserId) // Flashcard references User via UserId
        .OnDelete(DeleteBehavior.Restrict); // Prevents deletion of a User if they have associated Flashcards
});

        // Seed data or additional configurations will go here later on.
    }
}
