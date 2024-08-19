using System.ComponentModel.DataAnnotations;

namespace FlashTimes.Models.DTOs;

public class UserResponseDTO
{
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Token { get; set; } // JWT token
}