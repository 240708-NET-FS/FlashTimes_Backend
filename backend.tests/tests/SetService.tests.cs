using Moq;
using System.Threading.Tasks;
using Xunit;
using FlashTimes.Entities;
using FlashTimes.Repositories;
using FlashTimes.Services;
using System;

public class SetServiceTests
{
    private readonly Mock<ISetRepository> _mockSetRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly ISetService _setService;

    public SetServiceTests()
    {
        _mockSetRepository = new Mock<ISetRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _setService = new SetService(_mockSetRepository.Object, _mockUserRepository.Object);
    }

    //CreateSetAsync

    [Fact]
    public async Task CreateSetAsync_ShouldReturnSet_WhenUserExists()
    {
        // Arrange
        var user = new User { UserId = 1, UserName = "testuser" };
        var set = new Set { SetName = "Test Set", UserId = 1 };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(set.UserId))
            .ReturnsAsync(user);

        _mockSetRepository.Setup(repo => repo.CreateSetAsync(It.IsAny<Set>()))
            .ReturnsAsync(set);

        // Act
        var result = await _setService.CreateSetAsync(set);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Set", result.SetName);
        Assert.Equal(1, result.UserId);

        _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(set.UserId), Times.Once);
        _mockSetRepository.Verify(repo => repo.CreateSetAsync(It.IsAny<Set>()), Times.Once);
    }

    [Fact]
    public async Task CreateSetAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var set = new Set { SetName = "Test Set", UserId = 1 };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(set.UserId))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _setService.CreateSetAsync(set);

        // Assert
        Assert.Null(result);

        _mockUserRepository.Verify(repo => repo.GetUserByIdAsync(set.UserId), Times.Once);
        _mockSetRepository.Verify(repo => repo.CreateSetAsync(It.IsAny<Set>()), Times.Never);
    }


    //GetAllSetsAsync
    [Fact]
    public async Task GetAllSetsAsync_ShouldReturnAllSets_WhenSetsExist()
    {
        // Arrange
        var sets = new List<Set>
    {
        new Set { SetId = 1, SetName = "Set 1", UserId = 1 },
        new Set { SetId = 2, SetName = "Set 2", UserId = 2 }
    };

        _mockSetRepository.Setup(repo => repo.GetAllSetsAsync())
            .ReturnsAsync(sets);

        // Act
        var result = await _setService.GetAllSetsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _mockSetRepository.Verify(repo => repo.GetAllSetsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllSetsAsync_ShouldReturnEmptyList_WhenNoSetsExist()
    {
        // Arrange
        _mockSetRepository.Setup(repo => repo.GetAllSetsAsync())
            .ReturnsAsync(new List<Set>());

        // Act
        var result = await _setService.GetAllSetsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _mockSetRepository.Verify(repo => repo.GetAllSetsAsync(), Times.Once);
    }



    //GetSetByIdAsync
    [Fact]
    public async Task GetSetByIdAsync_ShouldReturnSet_WhenSetExists()
    {
        // Arrange
        var set = new Set { SetId = 1, SetName = "Set 1", UserId = 1 };

        _mockSetRepository.Setup(repo => repo.GetSetByIdAsync(set.SetId))
            .ReturnsAsync(set);

        // Act
        var result = await _setService.GetSetByIdAsync(set.SetId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(set.SetId, result.SetId);

        _mockSetRepository.Verify(repo => repo.GetSetByIdAsync(set.SetId), Times.Once);
    }

    [Fact]
    public async Task GetSetByIdAsync_ShouldReturnNull_WhenSetDoesNotExist()
    {
        // Arrange
        _mockSetRepository.Setup(repo => repo.GetSetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Set?)null);

        // Act
        var result = await _setService.GetSetByIdAsync(1);

        // Assert
        Assert.Null(result);

        _mockSetRepository.Verify(repo => repo.GetSetByIdAsync(1), Times.Once);
    }





    //UpdateSetAsync
    [Fact]
    public async Task UpdateSetAsync_ShouldReturnUpdatedSet_WhenSetExists()
    {
        // Arrange
        var set = new Set { SetId = 1, SetName = "Updated Set", UserId = 1 };

        _mockSetRepository.Setup(repo => repo.UpdateSetAsync(It.IsAny<Set>()))
            .ReturnsAsync(set);

        // Act
        var result = await _setService.UpdateSetAsync(set);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Set", result.SetName);

        _mockSetRepository.Verify(repo => repo.UpdateSetAsync(It.IsAny<Set>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSetAsync_ShouldReturnNull_WhenSetDoesNotExist()
    {
        // Arrange
        _mockSetRepository.Setup(repo => repo.UpdateSetAsync(It.IsAny<Set>()))
            .ReturnsAsync((Set?)null);

        // Act
        var result = await _setService.UpdateSetAsync(new Set { SetId = 1 });

        // Assert
        Assert.Null(result);

        _mockSetRepository.Verify(repo => repo.UpdateSetAsync(It.IsAny<Set>()), Times.Once);
    }


    //DeleteSetAsync
    [Fact]
    public async Task DeleteSetAsync_ShouldReturnTrue_WhenSetIsDeleted()
    {
        // Arrange
        _mockSetRepository.Setup(repo => repo.DeleteSetAsync(It.IsAny<int>()))
            .ReturnsAsync(true);

        // Act
        var result = await _setService.DeleteSetAsync(1);

        // Assert
        Assert.True(result);

        _mockSetRepository.Verify(repo => repo.DeleteSetAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteSetAsync_ShouldReturnFalse_WhenSetIsNotDeleted()
    {
        // Arrange
        _mockSetRepository.Setup(repo => repo.DeleteSetAsync(It.IsAny<int>()))
            .ReturnsAsync(false);

        // Act
        var result = await _setService.DeleteSetAsync(1);

        // Assert
        Assert.False(result);

        _mockSetRepository.Verify(repo => repo.DeleteSetAsync(1), Times.Once);
    }





}
