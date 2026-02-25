
namespace Chest.Application.DTOs;

public class CreateChestRequest
{
    public string Name { get; set; }
    public string Tip { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Guid UserId { get; set; } 

    public CreateChestRequest() { }

    public CreateChestRequest(string name, string tip, double lat, double lon, Guid userId)
    {
        Name = name;
        Tip = tip;
        Latitude = lat;
        Longitude = lon;
        UserId = userId; 
    }

    
}