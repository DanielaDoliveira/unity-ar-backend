using Chest.Application.DTOs;
using FluentValidation;

namespace Chest.Application.Validations;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Informe o nome do pirata.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Informe a senha.");
    }
}