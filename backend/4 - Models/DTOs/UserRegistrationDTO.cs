using System.ComponentModel.DataAnnotations;

namespace FlashTimes.Models.DTOs;

public class UserRegistrationDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}