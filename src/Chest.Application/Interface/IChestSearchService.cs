using Chest.Application.DTOs;

namespace Chest.Application.Interface;

public interface IChestSearchService
{
    Task<IEnumerable<ChestResponse>> GetUserChestsAsync(Guid userId);
}