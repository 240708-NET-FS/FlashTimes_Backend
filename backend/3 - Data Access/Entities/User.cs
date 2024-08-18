using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlashTimes.Entities;

public class User
{
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        // Navigation property
        public ICollection<Set>? Sets { get; set; }

        // Navigation property for Flashcards
        public ICollection<Flashcard>? Flashcards { get; set; }

}