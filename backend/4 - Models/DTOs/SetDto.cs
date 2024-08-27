namespace FlashTimes.Models.DTOs;
public class SetDto
{
    public int SetId { get; set; }
    public string? SetName { get; set; }
    public int SetLength { get; set; }


    // Navigation property for the User (Author)
    public SetDtoUserDto? Author { get; set; }

    // Navigation property for the collection of Flashcards
    public ICollection<SetDtoFlashcardDto> Flashcards { get; set; } = new List<SetDtoFlashcardDto>();

}
