namespace Chest.Domain.Entities;

public class User
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public string PasswordHash { get; private set; }
    public int Coins { get; private set; }

    private User() { }

    public User(string name, string passwordHash)
    {
        UserId = Guid.NewGuid();
        Name = name;
        PasswordHash = passwordHash;
        Coins = 0; 
    }
    
    public int MaxChestsAllowed => (Coins / 100) + 1;
    
    public void EarnCoins(int amount) => Coins += amount;
    
    
}