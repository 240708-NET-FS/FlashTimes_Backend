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
        return await _context.Sets.Include(s => s.Author).ToListAsync();
    }

    public async Task<Set?> GetSetByIdAsync(int id)
    {
        // Retrieve a specific set by its ID, including the author information.
        return await _context.Sets.Include(s => s.Author).FirstOrDefaultAsync(s => s.SetId == id);
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
        // Update an existing set in the database.
        var existingSet = await _context.Sets.FindAsync(set.SetId);
        if (existingSet == null)
        {
            return null; // Return null if the set doesn't exist.
        }

        // Update the fields of the existing set with the new values.
        existingSet.SetName = set.SetName;
        existingSet.UserId = set.UserId;

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

