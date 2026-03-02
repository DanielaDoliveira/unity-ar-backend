using Chest.Application.DTOs;
using Chest.Application.Services;
using Chest.Application.Validators;
using Chest.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using FluentAssertions;
using Xunit;
using Chest.Exception;

namespace Chest.Tests.Application;

public class AuthServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<UserRegisterRequest> _registerValidator;
    private readonly IValidator<UserLoginRequest> _loginValidator;
    private readonly IValidator<DeleteUserRequest> _deleteValidator;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        // ⚔️ Criando os substitutos (Mocks)
        _userRepository = Substitute.For<IUserRepository>();
        _registerValidator = Substitute.For<IValidator<UserRegisterRequest>>();
        _loginValidator = Substitute.For<IValidator<UserLoginRequest>>();
        _deleteValidator = Substitute.For<IValidator<DeleteUserRequest>>();

        // Injetando no serviço
        _authService = new AuthService(
            _userRepository,
            _registerValidator,
            _loginValidator,
            _deleteValidator
        );
    }
    
    [Fact]
    public async Task RegisterAsync_ShouldReturnUserId_WhenDataIsValid()
    {
        // Arrange
        var request = new UserRegisterRequest { Name = "JackSparrow", Password = "rum123password" };
    
      
        _registerValidator.ValidateAsync(request).Returns(new ValidationResult());

        // Act
        var result = await _authService.RegisterAsync(request);

       
        result.Should().NotBeEmpty(); 
        
        await _userRepository.Received(1).AddAsync(Arg.Any<Chest.Domain.Entities.User>());
    }
    
    [Fact]
    public async Task RegisterAsync_ShouldThrowException_WhenValidationFails()
    {
        // Arrange
        var request = new UserRegisterRequest { Name = "Jack", Password = "123" };
        var failures = new List<ValidationFailure> { new("Password", "Senha curta demais!") };
    
        // NSubstitute configurado para o método assíncrono do FluentValidation
        _registerValidator.ValidateAsync(request, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(failures));

        // Act
        var act = async () => await _authService.RegisterAsync(request);

        // Assert
        // 1. Capturamos a exceção
        var exceptionAssertion = await act.Should().ThrowAsync<ValidationErrorsException>();

        // 2. Usamos 'ErrorMessages' (O nome exato da sua classe!)
        exceptionAssertion.Which.ErrorMessages.Should().Contain("Senha curta demais!");
    
        // 3. Garantimos que o repositório NUNCA foi chamado
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<Chest.Domain.Entities.User>());
    }
    
    [Fact]
    public async Task Validator_ShouldHaveError_WhenNameAlreadyExists()
    {
        // Arrange
        var repo = Substitute.For<IUserRepository>();
        repo.GetByNameAsync("CapitaoGancho").Returns(new Chest.Domain.Entities.User("CapitaoGancho", "hash"));
    
        var validator = new UserRegisterRequestValidator(repo);
        var request = new UserRegisterRequest { Name = "CapitaoGancho", Password = "password123" };

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == "Este nome de pirata já foi recrutado por outro bando!");
    }
    
}