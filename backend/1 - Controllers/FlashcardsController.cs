using FlashTimes.Entities;
using FlashTimes.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlashTimes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlashCardsController : ControllerBase
{
    private readonly IFlashCardService _flashCardService;

    public FlashCardsController(IFlashCardService flashCardService)
    {
        _flashCardService = flashCardService;
    }

    // GET: api/FlashCard
    // Retrieves all flashcards
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Flashcard>>> GetAllFlashcards()
    {
        var flashcards = await _flashCardService.GetAllFlashcardsAsync();
        return Ok(flashcards);
    }

    // GET: api/FlashCard/5?question=What is OOP?
    // Retrieves a specific flashcard by its SetId and Question
    [HttpGet("{setId}")]
    public async Task<ActionResult<Flashcard>> GetFlashcard(int setId, [FromQuery] string question)
    {
        var flashcard = await _flashCardService.GetFlashcardByIdAsync(setId, question);
        if (flashcard == null)
        {
            return NotFound();
        }
        return Ok(flashcard);
    }

    // POST: api/FlashCard
    // Adds a new flashcard
    [HttpPost]
    public async Task<ActionResult> AddFlashcard(Flashcard flashcard)
    {
        await _flashCardService.AddFlashcardAsync(flashcard);
        return CreatedAtAction(nameof(GetFlashcard), new { setId = flashcard.SetId, question = flashcard.Question }, flashcard);
    }

    // PUT: api/FlashCard/5?question=What is OOP?
    // Updates an existing flashcard
    [HttpPut("{setId}")]
    public async Task<IActionResult> UpdateFlashcard(int setId, [FromQuery] string question, Flashcard flashcard)
    {
        if (setId != flashcard.SetId || question != flashcard.Question)
        {
            return BadRequest();
        }

        await _flashCardService.UpdateFlashcardAsync(flashcard);
        return NoContent();
    }

    // DELETE: api/FlashCard/5?question=What is OOP?
    // Deletes a flashcard
    [HttpDelete("{setId}")]
    public async Task<IActionResult> DeleteFlashcard(int setId, [FromQuery] string question)
    {
        var flashcard = await _flashCardService.GetFlashcardByIdAsync(setId, question);
        if (flashcard == null)
        {
            return NotFound();
        }

        await _flashCardService.DeleteFlashcardAsync(setId, question);
        return NoContent();
    }
}

