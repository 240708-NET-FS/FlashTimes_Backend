using FlashTimes.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlashTimes.Repositories;

public interface ISetRepository
{
    Task<IEnumerable<Set>> GetAllSetsAsync();
    Task<Set?> GetSetByIdAsync(int id);
    Task<Set> CreateSetAsync(Set set);
    Task<Set?> UpdateSetAsync(Set set);
    Task<bool> DeleteSetAsync(int id);
}

public class SetRepository : ISetRepository
{
    private readonly FlashTimesDbContext _context;

    public SetRepository(FlashTimesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Set>> GetAllSetsAsync()
    {
        // Retrieve all sets from the database.
        return await _context.Sets
            .Include(s => s.Author) // Eagerly load the related Author for each Set
            .Include(s => s.Flashcards) // Eagerly load the related Flashcards for each Set
                .ThenInclude(f => f.Author) // Eagerly load the Author related to each Flashcard
            .ToListAsync();

    }

    public async Task<Set?> GetSetByIdAsync(int id)
    {
        // Retrieve a specific set by its ID, including the author information.
        return await _context.Sets
             .Include(s => s.Flashcards) // Eagerly load the related Flashcards
             .Include(s => s.Author) // Eagerly load the related Author
             .FirstOrDefaultAsync(s => s.SetId == id);
    }

    public async Task<Set> CreateSetAsync(Set set)
    {
        // Add a new set to the database.
        _context.Sets.Add(set);
        await _context.SaveChangesAsync();
        return set;
    }

    public async Task<Set?> UpdateSetAsync(Set set)
    {
        // Check if the set exists in the database.
        var existingSet = await _context.Sets.FindAsync(set.SetId);
        if (existingSet == null)
        {
            return null; // Return null if the set doesn't exist.
        }

        // Check if the user exists in the database.
        var userExists = await _context.Users.AnyAsync(u => u.UserId == set.UserId);
        if (!userExists)
        {
            throw new Exception("The specified user does not exist."); // Throw an exception if the user doesn't exist.
        }

        //Check if user has permission
        if (existingSet.UserId != set.UserId)
        {
            throw new Exception("The user does not own the set.");

        }

        // Update the fields of the existing set with the new values.
        existingSet.SetName = set.SetName;

        _context.Sets.Update(existingSet);
        await _context.SaveChangesAsync();

        return existingSet;
    }

    public async Task<bool> DeleteSetAsync(int id)
    {
        // Find the set to delete by its ID.
        var set = await _context.Sets.FindAsync(id);
        if (set == null)
        {
            return false; // Return false if the set doesn't exist.
        }

        _context.Sets.Remove(set);
        await _context.SaveChangesAsync();
        return true;
    }
}

