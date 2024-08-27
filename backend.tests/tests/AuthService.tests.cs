using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FlashTimes.Models.DTOs;
using FlashTimes.Repositories;
using FlashTimes.Services;
using FlashTimes.Utilities;
using FlashTimes.Entities;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IHasher> _hasherMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _hasherMock = new Mock<IHasher>();
        _authService = new AuthService(_userRepositoryMock.Object, _hasherMock.Object);
    }

    [Fact]
    public async Task LoginAsync_NullLoginDTO_ThrowsArgumentNullException()
    {
    // Arrange
    LoginDTO? loginDTO = null;

    // Act
    var exception = await Record.ExceptionAsync(() => 
  //  #pragma warning disable CS8604 // Suppress the warning as we are testing for this scenario
    _authService.LoginAsync(loginDTO)
  //  #pragma warning restore CS8604
    );

    // Assert
    Assert.NotNull(exception);
    Assert.IsType<ArgumentNullException>(exception);
    Assert.Equal("loginDTO", ((ArgumentNullException)exception).ParamName);
    }

    [Fact]
    public async Task LoginAsync_EmptyUserNameOrPassword_ThrowsArgumentException()
    {
        // Arrange
        var loginDTO = new LoginDTO { UserName = "", Password = "" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _authService.LoginAsync(loginDTO));
    }

     [Fact]
    public async Task LoginAsync_UserDoesNotExist_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var loginDTO = new LoginDTO { UserName = "nonexistentuser", Password = "password" };
        _userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(It.IsAny<string>()))
                           .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginDTO));
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var loginDTO = new LoginDTO { UserName = "existinguser", Password = "wrongpassword" };
        var user = new User
        {
            UserId = 1,
            UserName = "existinguser",
            PasswordHash = "storedHash",
            Salt = "storedSalt"
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(user);

        _hasherMock.Setup(hasher => hasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                   .Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginDTO));
    }
    

     [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsUserResponseDTO()
    {
        // Arrange
        var loginDTO = new LoginDTO { UserName = "existinguser", Password = "correctpassword" };
        var user = new User
        {
            UserId = 1,
            UserName = "existinguser",
            PasswordHash = "storedHash",
            Salt = "storedSalt",
            FirstName = "John",
            LastName = "Doe",
            CreatedAt = DateTime.UtcNow
        };

        var token = "generatedJwtToken";

        _userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(user);

        _hasherMock.Setup(hasher => hasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                   .Returns(true);

        _hasherMock.Setup(hasher => hasher.GenerateJwtToken(It.IsAny<User>()))
                   .Returns(token);

        // Act
        var result = await _authService.LoginAsync(loginDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.UserId, result.UserId);
        Assert.Equal(user.UserName, result.UserName);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
        Assert.Equal(user.CreatedAt, result.CreatedAt);
        Assert.Equal(token, result.Token);
    }

    [Fact]
    public async Task LoginAsync_FailedToGenerateToken_ThrowsInvalidOperationException()
    {
        // Arrange
        var loginDTO = new LoginDTO { UserName = "existinguser", Password = "correctpassword" };
        var user = new User
        {
            UserId = 1,
            UserName = "existinguser",
            PasswordHash = "storedHash",
            Salt = "storedSalt"
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByUserNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(user);

        _hasherMock.Setup(hasher => hasher.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                   .Returns(true);

        _hasherMock.Setup(hasher => hasher.GenerateJwtToken(It.IsAny<User>()))
                   .Returns(string.Empty);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.LoginAsync(loginDTO));
    }

    [Fact]
    public async Task LogoutAsync_ReturnsCompletedTask()
    {
       // Act
       var result = _authService.LogoutAsync(); // Don't await here, just call the method.

       // Assert
      await result; // Await the task to ensure it completes.
      Assert.Equal(Task.CompletedTask, result); // Now compare the result to Task.CompletedTask.
    }

}

