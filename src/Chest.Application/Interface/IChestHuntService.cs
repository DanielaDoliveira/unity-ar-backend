namespace Chest.Application.Interface;

public interface IChestHuntService
{
    /// <summary>
    /// Apenas localiza o baú disponível mais próximo, sem alterá-lo.
    /// </summary>
    Task<Domain.Entities.Chest?> FindNearestAvailableAsync(double latitude, double longitude);

    /// <summary>
    /// Efetivamente vincula o baú ao jogador após a confirmação.
    /// </summary>
    Task<bool> LockChestAsync(Guid chestId, Guid userId);
}