using FlashTimes.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlashTimes.Repositories;

    public interface IFlashCardRepository
    {
        Task<IEnumerable<Flashcard>> GetAllFlashcardsAsync();
        Task<Flashcard?> GetFlashcardByIdAsync(int setId, string question);
        Task AddFlashcardAsync(Flashcard flashcard);
        Task UpdateFlashcardAsync(Flashcard flashcard);
        Task DeleteFlashcardAsync(int setId, string question);
    }

    public class FlashCardRepository : IFlashCardRepository
    {
        private readonly FlashTimesDbContext _dbContext;

        public FlashCardRepository(FlashTimesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Retrieves all flashcards from the database
        public async Task<IEnumerable<Flashcard>> GetAllFlashcardsAsync()
        {
            return await _dbContext.Flashcards.ToListAsync();
        }

        // Retrieves a specific flashcard by its SetId and Question
        public async Task<Flashcard?> GetFlashcardByIdAsync(int setId, string question)
        {
            return await _dbContext.Flashcards
                .FirstOrDefaultAsync(fc => fc.SetId == setId && fc.Question == question);
        }

        // Adds a new flashcard to the database
        public async Task AddFlashcardAsync(Flashcard flashcard)
        {
            _dbContext.Flashcards.Add(flashcard);
            await _dbContext.SaveChangesAsync();
        }

        // Updates an existing flashcard in the database
        public async Task UpdateFlashcardAsync(Flashcard flashcard)
        {
            _dbContext.Flashcards.Update(flashcard);
            await _dbContext.SaveChangesAsync();
        }

        // Deletes a flashcard from the database
        public async Task DeleteFlashcardAsync(int setId, string question)
        {
            var flashcard = await GetFlashcardByIdAsync(setId, question);
            if (flashcard != null)
            {
                _dbContext.Flashcards.Remove(flashcard);
                await _dbContext.SaveChangesAsync();
            }
        }
    }




