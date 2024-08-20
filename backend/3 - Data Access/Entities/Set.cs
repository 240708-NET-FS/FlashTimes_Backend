using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FlashTimes.Entities;

public class Set
{
    public int SetId { get; set; }
    public string? SetName { get; set; }
    public int SetLength { get; set; }

    // Foreign key
    public int UserId { get; set; }

    // Navigation property for the User (Author)
    public User? Author { get; set; }

    // Navigation property for the collection of Flashcards
    public ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
}

