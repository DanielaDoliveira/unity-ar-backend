using Chest.Application.Services;
using Chest.Application.Validators;
using Chest.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace Chest.Tests;

public class ChestDeletionServiceTest
{
    private readonly IChestRepository _repository;
    private readonly ChestDeletionService _service;

    public ChestDeletionServiceTest()
    {
        // Repositório criado para teste
        _repository = Substitute.For<IChestRepository>();
     
        _service = new ChestDeletionService(_repository);
      
    }
    /// <summary>
    /// Validação: se o usuário que quer apagar o baú não for o real dono do baú:
    /// </summary>
    [Fact]
    public async Task DeleteChestAsync_WhenUserIsNotOwner_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var chestId = Guid.NewGuid();
        var realOwnerId = Guid.NewGuid();
        var impostorId = Guid.NewGuid(); // ID diferente!

        var fakeChest = new Domain.Entities.Chest("Baú", "Dica", 0, 0, realOwnerId);

      
        _repository.GetByIdAsync(chestId).Returns(fakeChest);

        // Act & Assert
      
        await _service.Invoking(s => s.DeleteChestAsync(chestId, impostorId))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("You are not the owner of this chest.");

       
        await _repository.DidNotReceive().DeleteAsync(Arg.Any<Domain.Entities.Chest>());
    }
    [Fact]
    public async Task DeleteChestAsync_WhenUserIsOwner_ShouldReturnTrue()
    {
        // Arrange
        var chestId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var fakeChest = new Domain.Entities.Chest("Baú", "Dica", 0, 0, ownerId);

        _repository.GetByIdAsync(chestId).Returns(fakeChest);
        _repository.DeleteAsync(fakeChest).Returns(true); // Configura o retorno de sucesso

        // Act
        var result = await _service.DeleteChestAsync(chestId, ownerId);

        // Assert
        result.Should().BeTrue();
        await _repository.Received(1).DeleteAsync(fakeChest);
    }
}