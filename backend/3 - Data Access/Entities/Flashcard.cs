using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlashTimes.Entities;

public class Flashcard
{
        [Key]
        public int FlashcardId { get; set; }

        public string? Question { get; set; }

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

