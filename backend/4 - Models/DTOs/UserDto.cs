using System.ComponentModel.DataAnnotations;

namespace FlashTimes.Models.DTOs;

//Used in GetSetByIdResponseDto
public class UserDto
{
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; }
}
