using Xunit;
using Moq;
using System.Threading.Tasks;
using FlashTimes.Entities;
using FlashTimes.Models.DTOs;
using FlashTimes.Repositories;
using FlashTimes.Services;
using FlashTimes.Utilities;
using System;

//namespace FlashTimes.Tests.Services
//{
    public class UserServiceTests
{
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IHasher> _hasherMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _hasherMock = new Mock<IHasher>();
            _userService = new UserService(_userRepositoryMock.Object, _hasherMock.Object);
        }

    [Fact]
    public async Task RegisterUserAsync_ShouldThrowArgumentNullException_WhenUserRegistrationDTOIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.RegisterUserAsync(null));
    }
    

    [Fact]
    public async Task RegisterUserAsync_ShouldThrowArgumentException_WhenPasswordIsNullOrEmpty()
    {
            // Arrange
        var userRegistrationDTO = new UserRegistrationDTO
        {
            UserName = "testuser",
            Password = ""
        };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.RegisterUserAsync(userRegistrationDTO));
    }
    
    [Fact]
    public async Task RegisterUserAsync_ShouldThrowArgumentException_WhenUserNameIsNullOrEmpty()
    {
        // Arrange
        var userRegistrationDTO = new UserRegistrationDTO
        {
            UserName = "",
            Password = "Password123!"
        };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.RegisterUserAsync(userRegistrationDTO));
    }


   [Fact]
   public async Task RegisterUserAsync_ShouldReturnUserRegistrationResponseDTO_WhenUserIsRegisteredSuccessfully()
   {
         // Arrange
         var userRegistrationDTO = new UserRegistrationDTO
         {
             FirstName = "gisa",
             LastName = "litt",
             UserName = "lawer1",
             Password = "lawer123"
        };

        var addedUser = new User
        {
            UserId = 13,
            UserName = "lawer1",
            FirstName = "gisa",
            LastName = "litt",
            CreatedAt = DateTime.UtcNow
        };

           _userRepositoryMock
                .SetupSequence(repo => repo.GetUserByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null) // First call returns null (user does not exist)
                .ReturnsAsync(addedUser); // Second call returns the added user

          _hasherMock
                .Setup(hasher => hasher.GenerateSalt())
                .Returns("salt");

         _hasherMock
                .Setup(hasher => hasher.HashPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("hashedPassword");

        _userRepositoryMock
                .Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

      // Act
      var result = await _userService.RegisterUserAsync(userRegistrationDTO);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(addedUser.UserId, result.UserId);
      Assert.Equal(addedUser.UserName, result.UserName);
      Assert.Equal(addedUser.FirstName, result.FirstName);
      Assert.Equal(addedUser.LastName, result.LastName);
      Assert.Equal(addedUser.CreatedAt, result.CreatedAt);
   }   
   
    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowArgumentException_WhenIdIsLessThanOrEqualToZero()
    {
            // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetUserByIdAsync(0));
    }


    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowKeyNotFoundException_WhenUserIsNotFound()
    {
            // Arrange
         _userRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((User)null);

            // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.GetUserByIdAsync(1));
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserIsFound()
    {
        // Arrange
        var user = new User
        {
            UserId = 1,
            UserName = "testuser",
            FirstName = "Test",
            LastName = "User"
        };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            // Act
        var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserId, result.UserId);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
    }

}    