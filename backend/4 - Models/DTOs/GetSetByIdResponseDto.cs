using System.ComponentModel.DataAnnotations;

namespace FlashTimes.Models.DTOs;
public class GetSetByIdResponseDto
{
    public int SetId { get; set; }
    public string? SetName { get; set; }
    public int SetLength { get; set; }
    public int UserId { get; set; }
    public UserDto Author { get; set; }
    public List<FlashcardDto> Flashcards { get; set; }
}
