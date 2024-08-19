using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlashTimes.Entities;

public class User
{
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string? UserName { get; set; }


        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }


        [Required]
        [StringLength(255)]
        public string? PasswordHash { get; set; }

        public string? Salt { get; set; }

        public DateTime CreatedAt { get; set; }


        // Navigation property
        public ICollection<Set>? Sets { get; set; }

        // Navigation property for Flashcards
        public ICollection<Flashcard>? Flashcards { get; set; }

}