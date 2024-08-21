using FlashTimes.Entities;
using FlashTimes.Repositories;
using FlashTimes.Models.DTOs;
using FlashTimes.Services;

namespace FlashTimes.Services;

public interface ISetService
{
    Task<IEnumerable<Set>> GetAllSetsAsync();
    Task<Set?> GetSetByIdAsync(int id);
    Task<Set?> CreateSetAsync(CreateSetDto dto);
    Task<Set?> UpdateSetAsync(Set set);
    Task<bool> DeleteSetAsync(int id);
}

public class SetService : ISetService
{
    private readonly ISetRepository _setRepository;
    private readonly IUserRepository _userRepository;

    public SetService(ISetRepository setRepository, IUserRepository userRepository)
    {
        _setRepository = setRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Set>> GetAllSetsAsync()
    {
        // Retrieve all sets from the repository.
        return await _setRepository.GetAllSetsAsync();
    }

    public async Task<Set?> GetSetByIdAsync(int id)
    {
        // Retrieve a set by its ID from the repository.
        return await _setRepository.GetSetByIdAsync(id);
    }


    public async Task<Set?> CreateSetAsync(CreateSetDto dto)
    {
        var userExists = await _userRepository.GetUserByIdAsync(dto.UserId);
        if (userExists == null)
        {
            return null;
        }

        var set = new Set
        {
            SetName = dto.SetName,
            UserId = dto.UserId

        };

        return await _setRepository.CreateSetAsync(set);
    }


    public async Task<Set?> UpdateSetAsync(Set set)
    {
        //Map DTO to Set
        /*
                var set = new Set
                {
                    SetName = dto.SetName,
                    UserId = dto.UserId

                };
        */

        // Update an existing set in the repository.
        return await _setRepository.UpdateSetAsync(set);
    }

    public async Task<bool> DeleteSetAsync(int id)
    {
        // Remove a set by its ID from the repository.
        return await _setRepository.DeleteSetAsync(id);
    }
}

