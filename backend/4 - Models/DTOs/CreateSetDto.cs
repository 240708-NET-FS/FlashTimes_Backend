// CreateSetDto.cs
namespace FlashTimes.Models.DTOs;
public class CreateSetDto
{
    public int UserId { get; set; }
    public string SetName { get; set; }
    public int SetLength { get; set; }
}
