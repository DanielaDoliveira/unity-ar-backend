namespace Chest.Application.DTOs;

public class FindChestRequest
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Guid UserId { get; set; }

    // Construtor vazio para o Serializador (Unity / WebAPI)
    public FindChestRequest() { }

    
    public FindChestRequest(double latitude, double longitude, Guid userId)
    {
        Latitude = latitude;
        Longitude = longitude;
        UserId = userId;
    }
}