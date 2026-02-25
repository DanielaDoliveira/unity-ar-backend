using Chest.Application.DTOs;

namespace Chest.Application.Interface;

public interface IChestCreationService
{
    /// <summary>
    /// Transforma um pedido em um ChestId
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Guid> CreateChestAsync(CreateChestRequest request);
}