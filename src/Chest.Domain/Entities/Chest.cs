
namespace Chest.Domain.Entities;

public class Chest
{
    public Guid ChestId { get; private set; }
    public string Name { get; private set; }
    public string Tip { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public bool IsSomebodyPlaying { get; private set; }
    public Guid UserId { get; private set; } 
  
    public Guid? RivalId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    
    public Chest() { }

    public Chest(string name, string tip, double lat, double lon, Guid userId)
    {
        ChestId = Guid.NewGuid();
        Name = name;
        Tip = tip;
        Latitude = lat;
        Longitude = lon;
        UserId = userId;
        IsSomebodyPlaying = false; 
        CreatedAt = DateTime.UtcNow;
    }
    
    public void Claim(Guid rivalId)
    {
        if (IsSomebodyPlaying)
            throw new InvalidOperationException("Este baú já está sendo jogado por outra pessoa.");

        IsSomebodyPlaying = true;
        RivalId = rivalId;
    }
    

    public void Release()
    {
        IsSomebodyPlaying = false;
        RivalId = null;
    }
}