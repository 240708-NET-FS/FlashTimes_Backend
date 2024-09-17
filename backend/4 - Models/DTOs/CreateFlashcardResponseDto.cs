using System.ComponentModel.DataAnnotations;

namespace FlashTimes.Models.DTOs;
public class CreateFlashcardResponseDto
{
    public int FlashcardId { get; set; }
    public string? Question { get; set; }
    public string? Answer { get; set; }
    public int SetId { get; set; }
    public int UserId { get; set; }

}
