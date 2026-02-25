namespace Chest.Application.DTOs;

public class UserChestResponse
{
    public Guid ChestId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public bool IsSomebodyPlaying { get; set; }

    public UserChestResponse() { }
    public UserChestResponse(Guid chestId, Guid userId, string name, bool isSomebodyPlaying)
    {
        ChestId = chestId;
        UserId = userId;
        Name = name;
        IsSomebodyPlaying = isSomebodyPlaying;
    }
}