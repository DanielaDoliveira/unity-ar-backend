namespace Chest.Tests.Unit;

public class ClaimTest
{
    [Fact]
    public void Claim_ShouldThrowException_WhenChestIsAlreadyOccupied()
    {
        // Arrange
        var chest = new Domain.Entities.Chest("Nome", "Dica", 0, 0, Guid.NewGuid());
        chest.Claim(Guid.NewGuid()); // Primeira ocupação

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => chest.Claim(Guid.NewGuid()));
    }
}