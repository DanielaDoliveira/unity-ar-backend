using Chest.Application.DTOs;
using Chest.Domain.Interfaces;
using FluentValidation;

namespace Chest.Application.Validators;

public class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequest>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserRequestValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("O ID do pirata é necessário para a execução.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Você precisa confirmar sua senha para deletar a conta.");

        // Regra Especial: O pirata deve existir e a senha deve bater
        RuleFor(x => x).MustAsync(VerifyPirateIdentity)
            .WithMessage("Credenciais inválidas! Você não tem autorização para apagar este pirata.");
    }

    private async Task<bool> VerifyPirateIdentity(DeleteUserRequest request, CancellationToken token)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null) return false;

     
        return BCrypt.Net.BCrypt.Verify(request.ConfirmPassword, user.PasswordHash);
    }
}