using System.ComponentModel.DataAnnotations;

namespace FlashTimes.Entities;
public class Set
{
    public int SetId { get; set; }
    public string? SetName { get; set; }
    public int SetLength { get; set; }

    // Foreign key
    public int UserId { get; set; }

    // Navigation property
    public User? Author { get; set; }

}
