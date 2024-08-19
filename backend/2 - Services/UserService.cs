using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; // For Task
using FlashTimes.Entities; // For User
using FlashTimes.Models.DTOs; // For UserRegistrationDTO
using FlashTimes.Repositories;
using FlashTimes.Utilities;
using System.Security.Cryptography;
using System.Text;

namespace FlashTimes.Services;
public interface IUserService
{
    Task<UserRegistrationResponseDTO> RegisterUserAsync(UserRegistrationDTO userRegistrationDTO);
    Task<User> GetUserByIdAsync(int id);
}


public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IHasher _hasher;

    public UserService(IUserRepository userRepository, IHasher hasher)
    {
        _userRepository = userRepository;
        _hasher = hasher;
    }



    public async Task<UserRegistrationResponseDTO> RegisterUserAsync(UserRegistrationDTO userRegistrationDTO)
    {
        // Check if userRegistrationDTO is null
        if (userRegistrationDTO == null)
            throw new ArgumentNullException(nameof(userRegistrationDTO));

        // Check if _userRepository and _hasher are null
        if (_userRepository == null)
            throw new InvalidOperationException("User repository is not initialized.");

        if (_hasher == null)
            throw new InvalidOperationException("Hasher service is not initialized.");

        // Check if userRegistrationDTO.Password is null or empty
        if (string.IsNullOrEmpty(userRegistrationDTO.Password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(userRegistrationDTO.Password));

        // Check if userRegistrationDTO.UserName is null or empty
        if (string.IsNullOrEmpty(userRegistrationDTO.UserName))
            throw new ArgumentException("Username cannot be null or empty.", nameof(userRegistrationDTO.UserName));

        var existingUser = await _userRepository.GetUserByUserNameAsync(userRegistrationDTO.UserName);

        if (existingUser != null)
        {
            throw new Exception("Username is already taken.");
        }

        // Generate salt and hash the password using the Hasher class
        var salt = _hasher.GenerateSalt();
        var hashedPassword = _hasher.HashPassword(userRegistrationDTO.Password, salt);

        var user = new User
        {
            FirstName = userRegistrationDTO.FirstName,
            LastName = userRegistrationDTO.LastName,
            UserName = userRegistrationDTO.UserName,
            PasswordHash = hashedPassword,
            Salt = salt,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddUserAsync(user); // Add user
        var addedUser = await _userRepository.GetUserByUserNameAsync(user.UserName); // Return added user details

        if (addedUser == null)
            throw new Exception("User was not added correctly.");

        return new UserRegistrationResponseDTO
        {
            UserId = addedUser.UserId,
            UserName = addedUser.UserName,
            FirstName = addedUser.FirstName,
            LastName = addedUser.LastName,
            CreatedAt = addedUser.CreatedAt
        };
    }


    public async Task<User> GetUserByIdAsync(int id)
    {
        // Check if the ID is valid (optional, based on your application's requirements)
        if (id <= 0)
        {
            throw new ArgumentException("ID must be greater than zero.", nameof(id));
        }

        // Check if _userRepository is null
        if (_userRepository == null)
        {
            throw new InvalidOperationException("User repository is not initialized.");
        }

        // Retrieve the user from the repository
        var user = await _userRepository.GetUserByIdAsync(id);

        // Handle possible null return values (if necessary)
        if (user == null)
        {
            // You can either return null, or handle the case as needed
            // e.g., throw an exception or return a default value
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        return user;
    }

}