namespace Chest.Application.DTOs;

public class FindChest
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Guid RivalId { get; set; }
    public FindChest() { }
    
    public FindChest(double lat, double lon, Guid rivalId)
    {
        Latitude = lat;
        Longitude = lon;
        RivalId = rivalId;
    }
}