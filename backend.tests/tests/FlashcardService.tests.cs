using FlashTimes.Entities;
using FlashTimes.Repositories;
using FlashTimes.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class FlashCardServiceTests
{
    private readonly Mock<IFlashCardRepository> _flashcardRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ISetRepository> _setRepositoryMock;
    private readonly FlashCardService _flashCardService;

    public FlashCardServiceTests()
    {
        _flashcardRepositoryMock = new Mock<IFlashCardRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _setRepositoryMock = new Mock<ISetRepository>();
        _flashCardService = new FlashCardService(_flashcardRepositoryMock.Object, _userRepositoryMock.Object, _setRepositoryMock.Object);
    }

    [Fact]
    public async Task GetFlashcardByIdAsync_ShouldReturnFlashcard_WhenFlashcardExists()
    {
        // Arrange
        var flashcardId = 1;
        var flashcard = new Flashcard { FlashcardId = flashcardId };
        _flashcardRepositoryMock.Setup(repo => repo.GetFlashcardByIdAsync(flashcardId)).ReturnsAsync(flashcard);

        // Act
        var result = await _flashCardService.GetFlashcardByIdAsync(flashcardId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(flashcardId, result.FlashcardId);
    }

     [Fact]
    public async Task GetFlashcardByIdAsync_ShouldReturnNull_WhenFlashcardDoesNotExist()
    {
        // Arrange
        var flashcardId = 1;
        _flashcardRepositoryMock.Setup(repo => repo.GetFlashcardByIdAsync(flashcardId)).ReturnsAsync((Flashcard?)null);

        // Act
        var result = await _flashCardService.GetFlashcardByIdAsync(flashcardId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllFlashcardsAsync_ShouldReturnAllFlashcards()
    {
        // Arrange
        var flashcards = new List<Flashcard> { new Flashcard(), new Flashcard() };
        _flashcardRepositoryMock.Setup(repo => repo.GetAllFlashcardsAsync()).ReturnsAsync(flashcards);

        // Act
        var result = await _flashCardService.GetAllFlashcardsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(flashcards.Count, ((List<Flashcard>)result).Count);
    }

    [Fact]
    public async Task AddFlashcardAsync_ShouldThrowArgumentNullException_WhenFlashcardIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _flashCardService.AddFlashcardAsync(null!));
    }
    
     [Fact]
    public async Task AddFlashcardAsync_ShouldThrowInvalidOperationException_WhenUserDoesNotExist()
    {
        // Arrange
        var flashcard = new Flashcard { UserId = 1 };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(flashcard.UserId)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _flashCardService.AddFlashcardAsync(flashcard));
    }

    [Fact]
    public async Task AddFlashcardAsync_ShouldThrowInvalidOperationException_WhenSetDoesNotExist()
    {
        // Arrange
        var flashcard = new Flashcard { UserId = 1, SetId = 1 };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(flashcard.UserId)).ReturnsAsync(new User());
        _setRepositoryMock.Setup(repo => repo.GetSetByIdAsync(flashcard.SetId)).ReturnsAsync((Set?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _flashCardService.AddFlashcardAsync(flashcard));
    }

    [Fact]
    public async Task AddFlashcardAsync_ShouldReturnFlashcard_WhenSuccessfullyAdded()
    {
        // Arrange
        var flashcard = new Flashcard { UserId = 1, SetId = 1 };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(flashcard.UserId)).ReturnsAsync(new User());
        _setRepositoryMock.Setup(repo => repo.GetSetByIdAsync(flashcard.SetId)).ReturnsAsync(new Set());
        _flashcardRepositoryMock.Setup(repo => repo.AddFlashcardAsync(flashcard)).ReturnsAsync(flashcard);

        // Act
        var result = await _flashCardService.AddFlashcardAsync(flashcard);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(flashcard, result);
    }

    [Fact]
    public async Task UpdateFlashcardAsync_ShouldThrowArgumentNullException_WhenFlashcardIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _flashCardService.UpdateFlashcardAsync(null!));
    }
    
    [Fact]
    public async Task UpdateFlashcardAsync_ShouldThrowArgumentException_WhenFlashcardDoesNotExist()
    {
        // Arrange
        var flashcard = new Flashcard { FlashcardId = 1 };
        _flashcardRepositoryMock.Setup(repo => repo.GetFlashcardByIdAsync(flashcard.FlashcardId)).ReturnsAsync((Flashcard?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _flashCardService.UpdateFlashcardAsync(flashcard));
    }
    
    [Fact]
    public async Task UpdateFlashcardAsync_ShouldThrowArgumentException_WhenUserDoesNotOwnFlashcard()
    {
        // Arrange
        var flashcard = new Flashcard { FlashcardId = 1, UserId = 2 };
        var existingFlashcard = new Flashcard { FlashcardId = 1, UserId = 1 };
        _flashcardRepositoryMock.Setup(repo => repo.GetFlashcardByIdAsync(flashcard.FlashcardId)).ReturnsAsync(existingFlashcard);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _flashCardService.UpdateFlashcardAsync(flashcard));
    }

    [Fact]
    public async Task UpdateFlashcardAsync_ShouldReturnUpdatedFlashcard_WhenSuccessfullyUpdated()
    {
        // Arrange
        var flashcard = new Flashcard { FlashcardId = 1, UserId = 1, Question = "Updated Question", Answer = "Updated Answer" };
        var existingFlashcard = new Flashcard { FlashcardId = 1, UserId = 1, Question = "Original Question", Answer = "Original Answer" };
        _flashcardRepositoryMock.Setup(repo => repo.GetFlashcardByIdAsync(flashcard.FlashcardId)).ReturnsAsync(existingFlashcard);
        _flashcardRepositoryMock.Setup(repo => repo.UpdateFlashcardAsync(existingFlashcard)).ReturnsAsync(flashcard);

        // Act
        var result = await _flashCardService.UpdateFlashcardAsync(flashcard);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(flashcard.Question, result.Question);
        Assert.Equal(flashcard.Answer, result.Answer);
    }

    [Fact]
    public async Task DeleteFlashcardAsync_ShouldReturnTrue_WhenFlashcardIsDeleted()
    {
        // Arrange
        var flashcardId = 1;
        _flashcardRepositoryMock.Setup(repo => repo.DeleteFlashcardAsync(flashcardId)).ReturnsAsync(true);

        // Act
        var result = await _flashCardService.DeleteFlashcardAsync(flashcardId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteFlashcardAsync_ShouldReturnFalse_WhenFlashcardIsNotDeleted()
    {
        // Arrange
        var flashcardId = 1;
        _flashcardRepositoryMock.Setup(repo => repo.DeleteFlashcardAsync(flashcardId)).ReturnsAsync(false);

        // Act
        var result = await _flashCardService.DeleteFlashcardAsync(flashcardId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteFlashcardAsync_ShouldThrowArgumentException_WhenIdIsInvalid()
    {
        // Arrange
        var invalidId = 0;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _flashCardService.DeleteFlashcardAsync(invalidId));
    }

}
