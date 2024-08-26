using FlashTimes.Entities;
using FlashTimes.Services;
using FlashTimes.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlashTimes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlashCardsController : ControllerBase
{
    private readonly IFlashCardService _flashcardService;

    public FlashCardsController(IFlashCardService flashcardService)
    {
        _flashcardService = flashcardService;
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetFlashcardByIdAsync))] //This needs to be added so CreatedAtAction method can be used in AddFlashcardAsync response
    public async Task<ActionResult<Flashcard>> GetFlashcardByIdAsync(int id)

    {
        var flashcard = await _flashcardService.GetFlashcardByIdAsync(id);

        if (flashcard == null)
            return NotFound();

        return Ok(flashcard);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Flashcard>>> GetAllFlashcardsAsync()
    {
        var flashcards = await _flashcardService.GetAllFlashcardsAsync();
        return Ok(flashcards);
    }

    [HttpPost]
    public async Task<ActionResult<Flashcard>> AddFlashcardAsync(FlashcardDtoRequest flashcardDto)
    {

        //Map FlashCardDtoRequest to Flashcard entity
        var flashcard = new Flashcard
        {
            SetId = flashcardDto.SetId,
            UserId = flashcardDto.UserId,
            Question = flashcardDto.Question,
            Answer = flashcardDto.Answer
        };

        //Pass Flashcard entity to the Service layer
        var createdFlashcard = await _flashcardService.AddFlashcardAsync(flashcard);

        //Check to see if Flashcard was created
        if (createdFlashcard == null)
        {
            return Problem("Failed to create flashcard.");
        }

        return CreatedAtAction(nameof(GetFlashcardByIdAsync), new { id = createdFlashcard.FlashcardId }, createdFlashcard);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Flashcard>> UpdateFlashcardAsync(int id, UpdateFlashcardRequestDto flashcardDto)
    {



        var flashcard = new Flashcard
        {
            FlashcardId = id, // Ensure the ID matches the ID in the route
            SetId = flashcardDto.SetId,
            UserId = flashcardDto.UserId,
            Question = flashcardDto.Question,
            Answer = flashcardDto.Answer
        };


        // Pass the updated Flashcard entity to the Service layer
        var updatedFlashcard = await _flashcardService.UpdateFlashcardAsync(flashcard);

        // Check to see if Flashcard was updated
        if (updatedFlashcard == null)
        {
            return NotFound(); // Flashcard with given ID not found
        }

        return Ok(updatedFlashcard); // Return the updated Flashcard
    }



    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFlashcardAsync(int id)
    {
        var result = await _flashcardService.DeleteFlashcardAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}

