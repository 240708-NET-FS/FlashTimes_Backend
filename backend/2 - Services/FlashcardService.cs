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
            return null;
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
        

        //validate user who wants to make update with user who owns Flashcard
            //retrieve the flashcard by id from database
                var existingFlashcard = await _flashcardRepository.GetFlashcardByIdAsync(flashcard.FlashcardId);
                if (existingFlashcard == null){
                    throw new ArgumentException("Could not find Flashcard by Id");
                }
            //compare user id from request to user id of flashcard owner from database
                if(existingFlashcard.UserId != flashcard.UserId){
                    throw new ArgumentException("This user does not own the Flashcard");

                }
              
              //update existing flashcard with new information
              existingFlashcard.Question = flashcard.Question;
              existingFlashcard.Answer = flashcard.Answer;

              //pass updated existing flashcard to update method
              var updatedFlashcard = await _flashcardRepository.UpdateFlashcardAsync(existingFlashcard);

        // Handle case where flashcard could not be updated
        if (updatedFlashcard == null)
        {
            // Optional: log or throw a custom exception
            // throw new NotFoundException($"Flashcard with ID {flashcard.FlashcardId} not found.");
            throw new Exception("Unable to update Flashcard");
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

