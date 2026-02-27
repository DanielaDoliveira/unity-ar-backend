using Chest.Application.DTOs;
using Chest.Application.Interface;
using Chest.Domain.Interfaces;

namespace Chest.Application.Services;

public class ChestSearchService: IChestSearchService
{
    private readonly IChestRepository _repository;

    public ChestSearchService(IChestRepository repository)
    {
        _repository = repository;
    }

  
    public async Task<IEnumerable<ChestResponse>> GetUserChestsAsync(Guid userId)
    {
        var chests = await _repository.GetByUserIdAsync(userId);

        
        return chests.Select(c => new ChestResponse
        {
            ChestId = c.ChestId,
            Name = c.Name,
            Latitude = c.Latitude,
            Longitude = c.Longitude,
            IsSomebodyPlaying = c.IsSomebodyPlaying,
            UserId = c.UserId
        }).ToList();
    }
}