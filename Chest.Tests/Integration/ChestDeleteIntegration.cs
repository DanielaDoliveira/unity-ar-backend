using Chest.Application.Services;
using Chest.Infrastructure.Data;
using Chest.Infrastructure.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chest.Tests.Infrastructure;

public class ChestDeleteIntegration
{
    private ChestDbContext CreateDbContext()
    {
        // Cria uma conexão in-memory que fica viva durante o teste
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ChestDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ChestDbContext(options);
        context.Database.EnsureCreated(); // Cria as tabelas na hora
        return context;
    }

    [Fact]
    public async Task DeleteChestAsync_ShouldThrowUnauthorized_WhenUserIsNotOwner()
    {
        // Arrange
        using var context = CreateDbContext(); // Certifique-se de usar 'using' para limpar a memória
        var repository = new ChestRepository(context);
        var service = new ChestDeletionService(repository);

        var ownerId = Guid.NewGuid();
        var intruderId = Guid.NewGuid();
        var chest = new Domain.Entities.Chest("Tesouro Protegido", "Dica", 10, 20, ownerId);
    
        await repository.AddAsync(chest);

        // Act & Assert
        // O xUnit "vigia" a execução. Se a exceção UnauthorizedAccessException for lançada, o teste PASSA.
        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => 
        {
            await service.DeleteChestAsync(chest.ChestId, intruderId);
        });

        // Verificação extra: O baú ainda deve existir no banco (não foi deletado)
        var chestStillExists = await context.Chests.AnyAsync(c => c.ChestId == chest.ChestId);
        Assert.True(chestStillExists);
    }
}