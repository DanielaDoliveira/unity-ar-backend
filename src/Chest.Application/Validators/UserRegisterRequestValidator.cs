using Chest.Application.DTOs;
using Chest.Domain.Interfaces;
using FluentValidation;

namespace Chest.Application.Validators;

public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
{
    private readonly IUserRepository _userRepository;

    public UserRegisterRequestValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do pirata não pode estar vazio.")
            .Length(3, 20).WithMessage("O nome deve ter entre 3 e 20 caracteres.")
            .MustAsync(BeUniqueName).WithMessage("Este nome de pirata já foi recrutado por outro bando!"); // <--- A MÁGICA AQUI

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
    }

    // Função que verifica no banco se o nome é único
    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByNameAsync(name);
        return existingUser == null; 
    }
}