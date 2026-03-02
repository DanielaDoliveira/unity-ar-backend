using Chest.Application.DTOs;
using Chest.Application.Interface;
using Chest.Application.Validators;
using Chest.Domain.Entities;
using Chest.Domain.Interfaces;
using Chest.Exception; 
using FluentValidation;

namespace Chest.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<UserRegisterRequest> _registerValidator;
    private readonly IValidator<UserLoginRequest> _loginValidator;
    private readonly IValidator<DeleteUserRequest> _deleteUserValidator;

    public AuthService(
        IUserRepository userRepository, 
        IValidator<UserRegisterRequest> registerValidator,
        IValidator<UserLoginRequest> loginValidator, 
        IValidator<DeleteUserRequest> deleteUserValidator)
    {
        _userRepository = userRepository;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _deleteUserValidator = deleteUserValidator;
    }

    // --- REGISTO ---
    public async Task<Guid> RegisterAsync(UserRegisterRequest request)
    {
        await ValidateUserRegister(request);

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var newUser = new User(request.Name, passwordHash);
        
        await _userRepository.AddAsync(newUser);
        return newUser.UserId;
    }

    private async Task ValidateUserRegister(UserRegisterRequest request)
    {
        var validationResult = await _registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorsException(errors);
        }

        var existingUser = await _userRepository.GetByNameAsync(request.Name);
        if (existingUser != null)
            throw new InvalidOperationException("Este nome de pirata já foi recrutado!");
    }

    // --- LOGIN ---
    public async Task<Guid?> LoginAsync(UserLoginRequest request)
    {
    
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorsException(errors);
        }

   
        var user = await _userRepository.GetByNameAsync(request.Name);
        
       
        if (user == null) return null;

     bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid) return null;

 
        return user.UserId;
    }

    public async Task DeleteAccountAsync(DeleteUserRequest request)
    {
        var validationResult = await _deleteUserValidator.ValidateAsync(request);
    
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorsException(errors);
        }

        await _userRepository.DeleteAsync(request.UserId);
    }
}