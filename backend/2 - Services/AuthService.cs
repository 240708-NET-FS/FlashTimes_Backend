using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using FlashTimes.Models.DTOs;
using FlashTimes.Repositories;
using FlashTimes.Utilities;

namespace FlashTimes.Services;

public interface IAuthService
{
    Task<UserResponseDTO> LoginAsync(LoginDTO loginDTO);
    Task LogoutAsync();

}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IHasher _hasher;

    public AuthService(IUserRepository userRepository, IHasher hasher)
    {
        _userRepository = userRepository;
        _hasher = hasher;
    }


    public async Task<UserResponseDTO> LoginAsync(LoginDTO loginDTO)
    {
        if (loginDTO == null)
            throw new ArgumentNullException(nameof(loginDTO));

        if (string.IsNullOrEmpty(loginDTO.UserName) || string.IsNullOrEmpty(loginDTO.Password))
            throw new ArgumentException("Username and password must be provided.");

        // Retrieve the user from the database
        var user = await _userRepository.GetUserByUserNameAsync(loginDTO.UserName);

        if (user == null)
        {
            // User does not exist
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        // Retrieve the stored password hash and salt
        var storedHash = user.PasswordHash;
        var storedSalt = user.Salt;

        if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(storedSalt))
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        // Verify the provided password
        if (!_hasher.VerifyPassword(loginDTO.Password, storedHash, storedSalt))
        {
            // Password does not match
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        // Generate and return JWT token if password is correct
        var jwtToken = _hasher.GenerateJwtToken(user);

        if (string.IsNullOrEmpty(jwtToken))
            throw new InvalidOperationException("Failed to generate JWT token.");

        return new UserResponseDTO
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt,
            Token = jwtToken
        };
    }


    public Task LogoutAsync()
    {

        //Invalidate the current user's JWT on the client side
        return Task.CompletedTask;
    }



}