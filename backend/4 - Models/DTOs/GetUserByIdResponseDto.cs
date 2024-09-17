using System.ComponentModel.DataAnnotations;

namespace FlashTimes.Models.DTOs;

public class GetUserByIdResponseDto
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime CreatedAt { get; set; }


}

