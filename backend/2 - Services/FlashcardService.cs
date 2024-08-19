using FlashTimes.Entities;
using FlashTimes.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlashTimes.Services;

public interface IFlashCardService
{
    Task<Flashcard?> GetFlashcardByIdAsync(int id);
    Task<IEnumerable<Flashcard>> GetAllFlashcardsAsync();
    Task<Flashcard?> AddFlashcardAsync(Flashcard flashcard);
    Task<Flashcard?> UpdateFlashcardAsync(Flashcard flashcard);
    Task<bool> DeleteFlashcardAsync(int id);
}

public class FlashCardService : IFlashCardService
{
    private readonly IFlashCardRepository _flashcardRepository;

    public FlashCardService(IFlashCardRepository flashcardRepository)
    {
        _flashcardRepository = flashcardRepository;
    }

    public async Task<Flashcard?> GetFlashcardByIdAsync(int id)
    {
        // Retrieve the flashcard by ID
        var flashcard = await _flashcardRepository.GetFlashcardByIdAsync(id);

        // Handle null case if needed (e.g., logging or throwing exception)
        if (flashcard == null)
        {
            // Optional: log the null case or throw a custom exception
            // throw new NotFoundException($"Flashcard with ID {id} not found.");
        }

        return flashcard;
    }

    public async Task<IEnumerable<Flashcard>> GetAllFlashcardsAsync()
    {
        return await _flashcardRepository.GetAllFlashcardsAsync();
    }

    public async Task<Flashcard?> AddFlashcardAsync(Flashcard flashcard)
    {
        // Optionally validate flashcard before adding
        if (flashcard == null)
        {
            throw new ArgumentNullException(nameof(flashcard));
        }

        return await _flashcardRepository.AddFlashcardAsync(flashcard);
    }

    public async Task<Flashcard?> UpdateFlashcardAsync(Flashcard flashcard)
    {
        // Validate input
        if (flashcard == null)
        {
            throw new ArgumentNullException(nameof(flashcard));
        }

        var updatedFlashcard = await _flashcardRepository.UpdateFlashcardAsync(flashcard);

        // Handle case where flashcard could not be updated
        if (updatedFlashcard == null)
        {
            // Optional: log or throw a custom exception
            // throw new NotFoundException($"Flashcard with ID {flashcard.FlashcardId} not found.");
        }

        return updatedFlashcard;
    }

    public async Task<bool> DeleteFlashcardAsync(int id)
    {
        // Optionally validate ID
        if (id <= 0)
        {
            throw new ArgumentException("Invalid flashcard ID", nameof(id));
        }

        return await _flashcardRepository.DeleteFlashcardAsync(id);
    }
}

