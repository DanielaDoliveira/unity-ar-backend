using Chest.Application.DTOs;
using Chest.Application.Services;
using Chest.Application.Validators;
using Chest.Domain.Interfaces;
using Chest.Domain.Entities;
using Chest.Exception;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Chest.Tests.Application;

public class ChestCreationServiceTest
{
    private readonly IChestRepository _repository;
    private readonly CreateChestValidator _validator;
    private readonly ChestCreationService _service;

    public ChestCreationServiceTest()
    {
        // Repositório criado para teste
        _repository = Substitute.For<IChestRepository>();
        // Validando com o ChestValidator
        _validator = new CreateChestValidator();
        // Injetamos no serviço
        _service = new ChestCreationService(_repository, _validator);
    }

    [Fact]
    [Trait("Category", "BusinessLogic")]
    public async Task CreateChest_WhenDataIsValid_ShouldReturnGuid()
    {
        // Arrange
        var request = new CreateChestRequest(
            name: "Baú de Ouro",
            tip: "Perto da estátua",
            lat: -22.9068,
            lon: -43.1729,
            userId: Guid.NewGuid()
        );

        // Act
        var result = await _service.CreateChestAsync(request);

        // Assert
        result.Should().NotBeEmpty();
        // Garantimos que o repositório foi chamado exatamente 1 vez
        await _repository.Received(1).AddAsync(Arg.Any<Domain.Entities.Chest>());
    }

    [Fact]
    [Trait("Category", "Validation")]
    public async Task CreateChest_WhenNameIsEmpty_ShouldThrowValidationException()
    {
        // Arrange (Nome vazio para forçar o erro)
        var request = new CreateChestRequest(
            name: "", 
            tip: "Dica qualquer",
            lat: 0,
            lon: 0,
            userId: Guid.NewGuid()
        );

        // Act
        Func<Task> act = async () => await _service.CreateChestAsync(request);

        // Assert
        await act.Should().ThrowAsync<ValidationErrorsException>();
        
        // O ponto MAIS importante: O repositório NÃO pode ter sido chamado!
        await _repository.DidNotReceive().AddAsync(Arg.Any<Domain.Entities.Chest>());
    }
    
    [Fact]
    public async Task CreateChest_WhenDataIsValid_ShouldMapCorrectlyAndSave()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateChestRequest(
            name: "Baú Lendário",
            tip: "Debixo da ponte",
            lat: -23.5505,
            lon: -46.6333,
            userId: userId
        );

        // Act
        await _service.CreateChestAsync(request);

        // Assert
        await _repository.Received(1).AddAsync(Arg.Is<Domain.Entities.Chest>(c =>
                c.Name == request.Name &&
                c.Tip == request.Tip &&
                c.Latitude == request.Latitude &&
                c.Longitude == request.Longitude &&
                c.UserId == request.UserId &&
                c.IsSomebodyPlaying == false && // Ninguém jogando
                c.RivalId == null               // Rival em branco
        ));
    }
}