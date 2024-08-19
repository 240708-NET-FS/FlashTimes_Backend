using FlashTimes.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlashTimes.Repositories;

public interface IFlashCardRepository
{
    Task<Flashcard?> GetFlashcardByIdAsync(int id);
    Task<IEnumerable<Flashcard>> GetAllFlashcardsAsync();
    Task<Flashcard> AddFlashcardAsync(Flashcard flashcard);
    Task<Flashcard?> UpdateFlashcardAsync(Flashcard flashcard);
    Task<bool> DeleteFlashcardAsync(int id);
}

public class FlashCardRepository : IFlashCardRepository
{
    private readonly FlashTimesDbContext _dbContext;

    public FlashCardRepository(FlashTimesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Flashcard?> GetFlashcardByIdAsync(int id)
    {
        // Use FindAsync, which returns null if not found
        return await _dbContext.Flashcards.FindAsync(id);
    }

    public async Task<IEnumerable<Flashcard>> GetAllFlashcardsAsync()
    {
        return await _dbContext.Flashcards.ToListAsync();
    }

    public async Task<Flashcard> AddFlashcardAsync(Flashcard flashcard)
    {
        _dbContext.Flashcards.Add(flashcard);
        await _dbContext.SaveChangesAsync();
        return flashcard;
    }

    public async Task<Flashcard?> UpdateFlashcardAsync(Flashcard flashcard)
    {
        // Attach the entity to the context
        _dbContext.Flashcards.Update(flashcard);
        // Ensure the entity is found in the database before saving
        var existingFlashcard = await _dbContext.Flashcards.FindAsync(flashcard.FlashcardId);

        if (existingFlashcard == null)
        {
            return null; // Return null if the flashcard was not found
        }

        // Update the entity
        existingFlashcard.Question = flashcard.Question;
        existingFlashcard.Answer = flashcard.Answer;
        existingFlashcard.SetId = flashcard.SetId;
        existingFlashcard.UserId = flashcard.UserId;

        await _dbContext.SaveChangesAsync();
        return existingFlashcard;
    }

    public async Task<bool> DeleteFlashcardAsync(int id)
    {
        var flashcard = await _dbContext.Flashcards.FindAsync(id);
        if (flashcard == null)
            return false;

        _dbContext.Flashcards.Remove(flashcard);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}

