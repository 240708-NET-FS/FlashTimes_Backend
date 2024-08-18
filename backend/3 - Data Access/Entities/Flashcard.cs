using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashTimes.Entities;

public class Flashcard
{
        [Key]
        public int FlashcardId { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Question { get; set; }

        [Required]
        public string? Answer { get; set; }

        // Foreign key to the Set entity
        public int SetId { get; set; }

        // Foreign key to the User entity
        public int UserId { get; set; }

        // Navigation property for the related Set
        [ForeignKey("SetId")]
        public Set? Set { get; set; }

        // Navigation property for the related User (Author)
        [ForeignKey("UserId")]
        public User? Author { get; set; }

}

