
using FluentValidation;
using Chest.Application.DTOs;

namespace Chest.Application.Validators;


public class CreateChestValidator : AbstractValidator<CreateChestRequest>
{
    public CreateChestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage("O nome do baú é obrigatório.");
        RuleFor(x => x.Tip).NotEmpty().WithMessage("A dica não pode ser vazia.");
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude inválida.");
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).WithMessage("Longitude inválida.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("O ID do usuário criador é obrigatório.");
    }
}   