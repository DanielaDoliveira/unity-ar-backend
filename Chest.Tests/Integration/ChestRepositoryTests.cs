using Chest.Infrastructure.Data;
using Chest.Infrastructure.Repository;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chest.Tests.Infrastructure;

public class ChestRepositoryTests: IDisposable
{
    private readonly ChestDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly ChestRepository _repository;


    public ChestRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ChestDbContext>()
            .UseSqlite(_connection)
            .Options;
        _context = new ChestDbContext(options);
        
        _context.Database.EnsureCreated();
        _repository = new ChestRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistChestInDatabase()
    {
        //Arranje
        var userId = Guid.NewGuid();
        var newChest = new Domain.Entities.Chest("Baú de Teste", "Dica", 1.0, 2.0, userId);
        //Action
        await _repository.AddAsync(newChest);
        //Assert
        var chestInDb = await _context.Chests.FirstOrDefaultAsync(c => c.UserId == userId);
        chestInDb.Should().NotBeNull();
        chestInDb!.Name.Should().Be("Baú de Teste");
        chestInDb.UserId.Should().Be(userId);
        
        
    }
    
    /// <summary>
    /// Verifica se o banco de dados retorna os dados corretamente:averigua se os baús retornados batem com o criador dele (UserId) 
    /// </summary>
    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnOnlyChestsBelongingToSpecificUser()
    {
        // Arrange (Preparar)
        var user1 = Guid.NewGuid();
        var user2 = Guid.NewGuid();

        var chests = new List<Domain.Entities.Chest>
        {
            new Domain.Entities.Chest("Baú User 1 - A", "Dica", 10, 10, user1),
            new Domain.Entities.Chest("Baú User 1 - B", "Dica", 11, 11, user1),
            new Domain.Entities.Chest("Baú User 2 - C", "Dica", 12, 12, user2)
        };

        await _context.Chests.AddRangeAsync(chests);
        await _context.SaveChangesAsync();

        // Act (Ação)
        var result = await _repository.GetByUserIdAsync(user1);

        // Assert (Verificação)
        result.Should().NotBeNull();
        result.Should().HaveCount(2); // Deve trazer apenas os 2 baús do user1
        result.Should().AllSatisfy(c => c.UserId.Should().Be(user1));
        result.Should().NotContain(c => c.UserId == user2); // Garante que o baú do user2 não "vazou"
    }
    
    /// <summary>
    /// Verifica se o banco de dados retorna os dados corretamente: sem baús para o usuário que não criou nenhum
    /// </summary>
    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnEmptyList_WhenUserHasNoChests()
    {
        // Arrange
        var userWithNoChests = Guid.NewGuid();
        var userWithChests = Guid.NewGuid();
    
        var chest = new Domain.Entities.Chest("Baú Existente", "Dica", 0, 0, userWithChests);
        await _context.Chests.AddAsync(chest);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByUserIdAsync(userWithNoChests);

        // Assert
        result.Should().BeEmpty();
    }
    
    
    [Fact]
    public async Task DeleteAsync_ShouldActuallyRemoveFromDatabase()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chest = new Domain.Entities.Chest("Baú para Deletar", "Dica", 0, 0, userId);
        await _context.Chests.AddAsync(chest);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(chest);
    
        // Assert
        var deletedChest = await _context.Chests.FindAsync(chest.ChestId);
        deletedChest.Should().BeNull(); 
    }
    
    
    
    public void Dispose()
    {
        _connection.Close();
        _context.Dispose();
    }
}