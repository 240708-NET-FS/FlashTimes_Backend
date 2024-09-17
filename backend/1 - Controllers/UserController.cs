using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FlashTimes.Models.DTOs;
using FlashTimes.Services;

namespace FlashTimes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // POST: api/users/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userRegistrationDTO)
    {
        // Registration logic
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userRegistrationResponseDTO = await _userService.RegisterUserAsync(userRegistrationDTO);
            return CreatedAtAction(nameof(GetUserById), new { id = userRegistrationResponseDTO.UserId }, userRegistrationResponseDTO);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        //Get user by ID logic
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        // Map the user entity to the DTO
        var userResponse = new GetUserByIdResponseDto
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt
        };

        // Return the mapped DTO
        return Ok(userResponse);

    }




}