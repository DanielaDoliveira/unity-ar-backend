using Chest.Application.Services;
using Chest.Infrastructure.Data;
using Chest.Infrastructure.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chest.Tests.Infrastructure;

public class ChestHuntIntegrationTests
{
    private ChestDbContext CreateDbContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ChestDbContext>().UseSqlite(connection).Options;
        
        var context = new ChestDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task LockChestAsync_ShouldUpdateDatabase_WhenChestIsAvailable()
    {
        var context = CreateDbContext();
        var repository = new ChestRepository(context);
        var service = new ChestHuntService(repository);
        
        var chest = new Domain.Entities.Chest("Tesouro Antigo", "Perto da estátua", 10.0, 20.0, Guid.NewGuid());
        await repository.AddAsync(chest); // Salva o baú inicial
        var user2Id = Guid.NewGuid();
        
        var result = await service.LockChestAsync(chest.ChestId, user2Id);
        
        Assert.True(result);
        
        var updatedChest = await context.Chests.FindAsync(chest.ChestId);
        Assert.True(updatedChest!.IsSomebodyPlaying);
        Assert.Equal(user2Id, updatedChest.RivalId);
    }
    
    
    [Fact]
    public async Task LockChestAsync_ShouldPersistChanges_WhenChestIsAvailable()
    {
      
        using var context = CreateDbContext();
        var repository = new ChestRepository(context);
        var service = new ChestHuntService(repository);

        var ownerId = Guid.NewGuid();
        var playerId = Guid.NewGuid();
        var chest = new Domain.Entities.Chest("Baú para Lock", "Dica", 10, 20, ownerId);
        await repository.AddAsync(chest);

      
        var result = await service.LockChestAsync(chest.ChestId, playerId);


        Assert.True(result); 

        context.ChangeTracker.Clear(); 
    
        var chestInDb = await context.Chests.FindAsync(chest.ChestId);
        Assert.NotNull(chestInDb);
        Assert.True(chestInDb.IsSomebodyPlaying);
        Assert.Equal(playerId, chestInDb.RivalId);  
    }
}