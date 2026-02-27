using Chest.Application.DTOs;
using Chest.Application.Services;
using Chest.Domain.Entities;
using Chest.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Chest.UnitTests.Application;

public class ChestSearchServiceTests
{
    private readonly IChestRepository _repositoryMock;
    private readonly ChestSearchService _service;

    public ChestSearchServiceTests()
    {
   
        _repositoryMock = Substitute.For<IChestRepository>();
        _service = new ChestSearchService(_repositoryMock); 
    }

    [Fact]
    public async Task GetUserChestsAsync_WhenChestsExist_ShouldReturnMappedChestResponseList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var fakeChests = new List<Domain.Entities.Chest>
        {
            new Domain.Entities.Chest("Baú Lendário", "Perto da árvore", 10.5, 20.5, userId),
            new Domain.Entities.Chest("Baú Comum", "Na caverna", 30.0, 40.0, userId)
        };

        // Configurando o retorno no NSubstitute (Sintaxe bem limpa!)
        _repositoryMock.GetByUserIdAsync(userId).Returns(fakeChests);

        // Act
        var result = await _service.GetUserChestsAsync(userId);

        // Assert (Usando FluentAssertions)
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var firstChest = result.First();
        firstChest.Name.Should().Be("Baú Lendário");
        firstChest.UserId.Should().Be(userId);
        firstChest.Latitude.Should().Be(10.5);

        // Verificando se o repositório foi realmente chamado uma vez
        await _repositoryMock.Received(1).GetByUserIdAsync(userId);
    }

    [Fact]
    public async Task GetUserChestsAsync_WhenNoChestsExist_ShouldReturnEmptyList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _repositoryMock.GetByUserIdAsync(userId).Returns(new List<Domain.Entities.Chest>());

        // Act
        var result = await _service.GetUserChestsAsync(userId);

        // Assert
        result.Should().BeEmpty();
        result.Should().NotBeNull();
    }
}