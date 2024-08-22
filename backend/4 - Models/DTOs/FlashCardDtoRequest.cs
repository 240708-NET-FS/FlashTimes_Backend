using System.ComponentModel.DataAnnotations;
namespace FlashTimes.Models.DTOs;
public class FlashcardDtoRequest
{
    public int UserId { get; set; }
    public int SetId { get; set; }
    public string? Question { get; set; }
    public string? Answer { get; set; }
}
