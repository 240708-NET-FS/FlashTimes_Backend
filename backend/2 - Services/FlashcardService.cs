using FlashTimes.Entities;
using FlashTimes.Repositories;

namespace FlashTimes.Services;

    public interface IFlashCardService
    {
        Task<IEnumerable<Flashcard>> GetAllFlashcardsAsync();
        Task<Flashcard?> GetFlashcardByIdAsync(int setId, string question);
        Task AddFlashcardAsync(Flashcard flashcard);
        Task UpdateFlashcardAsync(Flashcard flashcard);
        Task DeleteFlashcardAsync(int setId, string question);
    }

    public class FlashCardService : IFlashCardService
    {
        private readonly IFlashCardRepository _flashCardRepository;

        public FlashCardService(IFlashCardRepository flashCardRepository)
        {
            _flashCardRepository = flashCardRepository;
        }

        // Retrieves all flashcards
        public async Task<IEnumerable<Flashcard>> GetAllFlashcardsAsync()
        {
            return await _flashCardRepository.GetAllFlashcardsAsync();
        }

        // Retrieves a specific flashcard by its SetId and Question
        public async Task<Flashcard?> GetFlashcardByIdAsync(int setId, string question)
        {
            return await _flashCardRepository.GetFlashcardByIdAsync(setId, question);
        }

        // Adds a new flashcard
        public async Task AddFlashcardAsync(Flashcard flashcard)
        {
            await _flashCardRepository.AddFlashcardAsync(flashcard);
        }

        // Updates an existing flashcard
        public async Task UpdateFlashcardAsync(Flashcard flashcard)
        {
            await _flashCardRepository.UpdateFlashcardAsync(flashcard);
        }

        // Deletes a flashcard
        public async Task DeleteFlashcardAsync(int setId, string question)
        {
            await _flashCardRepository.DeleteFlashcardAsync(setId, question);
        }
    }

  


