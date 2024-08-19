using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using FlashTimes.Entities;


namespace FlashTimes.Utilities;

//Hasher class focuses on security-related operations like hashing passwords and creating tokens. 
//It encapsulates all JWT-related logic, keeping it separate from the AuthService.
public class Hasher : IHasher
{
    private readonly JwtSettings _jwtSettings;

    // Constructor takes IOptions<JwtSettings> to initialize JwtSettings
    public Hasher(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    public string GenerateSalt()
    {

        byte[] saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);

    }
    public string HashPassword(string password, string salt)
    {
        if (string.IsNullOrEmpty(salt))
        {
            throw new FormatException("Salt cannot be null or empty.");
        }
        byte[] saltBytes = Convert.FromBase64String(salt);
        using var hmac = new HMACSHA512(saltBytes);
        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        // Handle null or empty salt
        if (string.IsNullOrEmpty(storedSalt))
        {
            throw new ArgumentNullException(nameof(storedSalt), "Salt cannot be null or empty.");
        }

        // Handle null hash
        if (storedHash == null)
        {
            return false;
        }

        byte[] saltBytes = Convert.FromBase64String(storedSalt);
        using var hmac = new HMACSHA512(saltBytes);
        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        string computedHashString = Convert.ToBase64String(computedHash);

        return computedHashString == storedHash;
    }




    public string GenerateJwtToken(User user)
    {
        Console.WriteLine("GenerateJwtToken() is running");

        // Check if user or _jwtSettings is null
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        if (_jwtSettings == null)
        {
            throw new InvalidOperationException("JWT settings are not configured.");
        }

        // Check if user.UserName is null or empty
        if (string.IsNullOrEmpty(user.UserName))
        {
            throw new ArgumentException("User name cannot be null or empty.", nameof(user.UserName));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        Console.WriteLine("tokenHandler: " + tokenHandler);

        // Ensure _jwtSettings.Secret is not null or empty
        if (string.IsNullOrEmpty(_jwtSettings.Secret))
        {
            throw new InvalidOperationException("JWT secret is not configured.");
        }

        Console.WriteLine("JWT Secret: " + _jwtSettings.Secret);

        // GetBytes(): This has to return at least 32 bytes.
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        Console.WriteLine("key (Base64): " + Convert.ToBase64String(key));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, user.UserName),
        }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        Console.WriteLine("token from GenerateJwtToken(): " + token);
        return tokenHandler.WriteToken(token);
    }


}