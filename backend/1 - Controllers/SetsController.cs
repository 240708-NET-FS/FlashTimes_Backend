using Microsoft.AspNetCore.Mvc;
using FlashTimes.Entities;
using FlashTimes.Services;
using FlashTimes.Models.DTOs;

namespace FlashTimes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SetsController : ControllerBase
{
    private readonly ISetService _setService;

    public SetsController(ISetService setService)
    {
        _setService = setService;
    }

    // GET: api/Set
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SetDto>>> GetSets()
    {
        // Retrieve all sets from the database.
        var sets = await _setService.GetAllSetsAsync();

        if (sets == null)
        {
            return NotFound(); // Return 404 if set is not found.
        }



        // Map each Set entity to SetDto
        var setDtos = sets.Select(set => new SetDto
        {
            SetId = set.SetId,
            SetName = set.SetName,
            SetLength = set.Flashcards.Count,
            Author = set.Author != null ? new SetDtoUserDto
            {
                UserId = set.Author.UserId,
                UserName = set.Author.UserName,
                FirstName = set.Author.FirstName,
                LastName = set.Author.LastName

            } : null,
            Flashcards = set.Flashcards.Select(f => new SetDtoFlashcardDto
            {
                FlashcardId = f.FlashcardId,
                Question = f.Question,
                Answer = f.Answer,

            }).ToList()
        }).ToList();

        return Ok(setDtos);



    }

    // GET: api/Set/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SetDto>> GetSet(int id)
    {
        // Retrieve a specific set by its ID.
        var set = await _setService.GetSetByIdAsync(id);

        if (set == null)
        {
            return NotFound(); // Return 404 if set is not found.
        }


        // Map the Set entity to SetDto
        var setDto = new SetDto
        {
            SetId = set.SetId,
            SetName = set.SetName,
            SetLength = set.Flashcards.Count,
            Author = set.Author != null ? new SetDtoUserDto
            {
                UserId = set.Author.UserId,
                UserName = set.Author.UserName,
                FirstName = set.Author.FirstName,
                LastName = set.Author.LastName
            } : null,
            Flashcards = set.Flashcards.Select(f => new SetDtoFlashcardDto
            {
                FlashcardId = f.FlashcardId,
                Question = f.Question,
                Answer = f.Answer,
            }).ToList()
        };

        return Ok(setDto);
    }

    // POST: api/Set
    [HttpPost]
    public async Task<IActionResult> CreateSet([FromBody] CreateSetDto dto)
    {
        if (dto == null || dto.UserId <= 0 || string.IsNullOrEmpty(dto.SetName))
        {
            return BadRequest("Invalid data.");
        }

        //Map CreateSetDto to Set entity
        var set = new Set
        {
            SetName = dto.SetName,
            UserId = dto.UserId

        };

        var result = await _setService.CreateSetAsync(set);
        if (result == null)
        {
            return BadRequest("Failed to create set.");
        }

        //Return DTO response instead of whole Set entity because Set entity has sensitive info and could be too large
        //Reusing CreateSetDto for response
        var responseDto = new CreateSetDto
        {
            UserId = result.UserId,
            SetName = result.SetName

        };

        return CreatedAtAction(nameof(GetSet), new
        {
            id = result.SetId
        }, responseDto);
    }



    // PUT: api/Set/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSet(int id, UpdateSetDto set)
    {

        //Map UpdateSetDto to Set entity
        var setToUpdate = new Set
        {
            SetId = id, //use the id from the URL
            SetName = set.SetName,
            UserId = set.UserId
        };

        var updatedSet = await _setService.UpdateSetAsync(setToUpdate);


        if (updatedSet == null)
        {
            return NotFound(); // Return 404 if the set to update doesn't exist.
        }

        return NoContent();
    }

    // DELETE: api/Set/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSet(int id)
    {
        // Delete a set by its ID.
        var success = await _setService.DeleteSetAsync(id);

        if (!success)
        {
            return NotFound(); // Return 404 if the set to delete doesn't exist.
        }

        return NoContent();
    }
}

