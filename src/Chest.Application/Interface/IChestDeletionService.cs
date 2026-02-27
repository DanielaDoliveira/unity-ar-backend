namespace Chest.Application.Interface;

public interface IChestDeletionService
{
    Task<bool> DeleteChestAsync(Guid chestId, Guid userId);
    
}