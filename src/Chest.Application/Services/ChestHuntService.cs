using Chest.Application.Interface;
using Chest.Domain.Interfaces;

namespace Chest.Application.Services;

public class ChestHuntService : IChestHuntService
{
    private readonly IChestRepository _repository;

    public ChestHuntService(IChestRepository repository)
    {
        _repository = repository;
    }

    // ETAPA 1: O Jogador está caminhando e o radar encontra algo
    public async Task<Domain.Entities.Chest?> FindNearestAvailableAsync(double latitude, double longitude)
    {
        var availableChests = await _repository.GetAvailableChestsAsync();

        if (!availableChests.Any()) return null;

        return availableChests
            .Select(chest => new 
            {
                Chest = chest,
                Distance = CalculateDistance(latitude, longitude, chest.Latitude, chest.Longitude)
            })
            .OrderBy(c => c.Distance)
            .ThenBy(c => c.Chest.ChestId)
            .Select(c => c.Chest)
            .FirstOrDefault();
    }

    // ETAPA 2: O Jogador viu o Pop-up e clicou em "OK" na Unity
    public async Task<bool> LockChestAsync(Guid chestId, Guid userId)
    {
        // 1. Buscamos o baú específico que o jogador quer abrir
        var chest = await _repository.GetByIdAsync(chestId);

        // 2. Validação: se o baú sumiu ou alguém trancou no milissegundo anterior
        if (chest == null || chest.IsSomebodyPlaying)
            return false;

        // 3. Aciona o método de domínio que você criou (Claim)
        chest.Claim(userId);

        // 4. Salva a alteração
        await _repository.UpdateAsync(chest);
        
        return true;
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double earthRadius = 6371e3; 
        var phi1 = lat1 * Math.PI / 180;
        var phi2 = lat2 * Math.PI / 180;
        var deltaPhi = (lat2 - lat1) * Math.PI / 180;
        var deltaLambda = (lon2 - lon1) * Math.PI / 180;

        var a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                Math.Cos(phi1) * Math.Cos(phi2) *
                Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadius * c; 
    }
}