namespace Chest.Application.DTOs;

public class ChestResponse
{
    public Guid ChestId { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsSomebodyPlaying { get; set; }
    public Guid UserId { get; set; }

    public ChestResponse(Guid chestId, string name, double latitude, double longitude, bool isSomebodyPlaying, Guid userId)
    {
        ChestId = chestId;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        IsSomebodyPlaying = isSomebodyPlaying;
        UserId = userId;
    }

    public ChestResponse() { }

   

  
}