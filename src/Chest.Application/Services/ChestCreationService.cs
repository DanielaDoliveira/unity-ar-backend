using Chest.Application.DTOs;
using Chest.Application.Interface;
using Chest.Domain.Interfaces;
using Chest.Exception;
using FluentValidation;

namespace Chest.Application.Services;

public class ChestCreationService: IChestCreationService
{
    private readonly IChestRepository _repository;
    private readonly IValidator<CreateChestRequest> _validator;
    
    public ChestCreationService(IChestRepository repository, IValidator<CreateChestRequest> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    
    
    public async Task<Guid> CreateChestAsync(CreateChestRequest request)
    {
        
        var validationResult = await _validator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorsException(errors);
        }

      
        var newChest = new Domain.Entities.Chest(
            request.Name,
            request.Tip,
            request.Latitude,
            request.Longitude,
            request.UserId
        );

      
        await _repository.AddAsync(newChest);

        return newChest.ChestId;
    }
}