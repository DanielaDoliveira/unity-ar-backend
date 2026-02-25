using Microsoft.EntityFrameworkCore;

namespace Chest.Infrastructure.Data;

public class ChestDbContext: DbContext
{
    public ChestDbContext(DbContextOptions<ChestDbContext> options) : base(options) { }
    public DbSet<Domain.Entities.Chest> Chests { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações específicas (Fluent API)
        modelBuilder.Entity<Domain.Entities.Chest>(entity => 
        {
            entity.HasKey(e => e.ChestId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Latitude).IsRequired();
            entity.Property(e => e.Longitude).IsRequired();
        });
        
        base.OnModelCreating(modelBuilder); 
    }
    
}