using Microsoft.AspNetCore.Mvc;
using FlashTimes.Entities;
using FlashTimes.Services;

namespace FlashTimes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SetController : ControllerBase
{
    private readonly ISetService _setService;

    public SetController(ISetService setService)
    {
        _setService = setService;
    }

    // GET: api/Set
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Set>>> GetSets()
    {
        // Retrieve all sets from the database.
        var sets = await _setService.GetAllSetsAsync();
        return Ok(sets);
    }

    // GET: api/Set/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Set>> GetSet(int id)
    {
        // Retrieve a specific set by its ID.
        var set = await _setService.GetSetByIdAsync(id);

        if (set == null)
        {
            return NotFound(); // Return 404 if set is not found.
        }

        return Ok(set);
    }

    // POST: api/Set
    [HttpPost]
    public async Task<ActionResult<Set>> CreateSet(Set set)
    {
        // Create a new set and save it to the database.
        var createdSet = await _setService.CreateSetAsync(set);
        return CreatedAtAction(nameof(GetSet), new { id = createdSet.SetId }, createdSet);
    }

    // PUT: api/Set/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSet(int id, Set set)
    {
        // Update an existing set identified by its ID.
        if (id != set.SetId)
        {
            return BadRequest(); // Return 400 if ID in the URL doesn't match the ID in the entity.
        }

        var updatedSet = await _setService.UpdateSetAsync(set);

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

