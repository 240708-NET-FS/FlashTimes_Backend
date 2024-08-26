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
    private readonly IUserRepository _userRepository;
    private readonly ISetRepository _setRepository;

    public FlashCardService(IFlashCardRepository flashcardRepository, IUserRepository userRepository, ISetRepository setRepository)
    {
        _flashcardRepository = flashcardRepository;
        _userRepository = userRepository;
        _setRepository = setRepository;
    }

    public async Task<Flashcard?> GetFlashcardByIdAsync(int id)
    {
        // Retrieve the flashcard by ID
        var flashcard = await _flashcardRepository.GetFlashcardByIdAsync(id);

        // Handle null case
        if (flashcard == null)
        {
            return null;
        }

        return flashcard;
    }

    public async Task<IEnumerable<Flashcard>> GetAllFlashcardsAsync()
    {
        return await _flashcardRepository.GetAllFlashcardsAsync();
    }

    public async Task<Flashcard?> AddFlashcardAsync(Flashcard flashcard)
    {
        // Validate flashcard is not null before adding
        if (flashcard == null)
        {
            throw new ArgumentNullException(nameof(flashcard));
        }

        // Check to see if the user who wants to add the flashcard exists
        var user = await _userRepository.GetUserByIdAsync(flashcard.UserId);
        if (user == null)
        {
            throw new InvalidOperationException("User does not exist");
        }

        //Check to see if the set exists before adding
        var set = await _setRepository.GetSetByIdAsync(flashcard.SetId);
        if (set == null)
        {
            throw new InvalidOperationException("Set does not exist");
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


        //Validate user who wants to make update with user who owns Flashcard
        //Retrieve the flashcard by id from database
        var existingFlashcard = await _flashcardRepository.GetFlashcardByIdAsync(flashcard.FlashcardId);
        if (existingFlashcard == null)
        {
            throw new ArgumentException("Could not find Flashcard by Id");
        }

        //Compare user id from request to user id of flashcard owner from database
        if (existingFlashcard.UserId != flashcard.UserId)
        {
            throw new ArgumentException("This user does not own the Flashcard");

        }

        //Update existing flashcard with new information
        existingFlashcard.Question = flashcard.Question;
        existingFlashcard.Answer = flashcard.Answer;

        //Pass updated existing flashcard to update method
        var updatedFlashcard = await _flashcardRepository.UpdateFlashcardAsync(existingFlashcard);

        // Handle case where flashcard could not be updated
        if (updatedFlashcard == null)
        {

            throw new Exception("Unable to update Flashcard");
        }

        return updatedFlashcard;
    }

    public async Task<bool> DeleteFlashcardAsync(int id)
    {
        // Validate ID
        if (id <= 0)
        {
            throw new ArgumentException("Invalid flashcard ID", nameof(id));
        }

        return await _flashcardRepository.DeleteFlashcardAsync(id);
    }
}

