namespace Chest.Application.DTOs;

public class UpdateChest
{
    public Guid RivalId { get; set; }
    public bool IsSomebodyPlaying { get; set; }

    public UpdateChest() { }

    public UpdateChest(Guid rivalId)
    {
        RivalId = rivalId;
        IsSomebodyPlaying = true;
    }
    
}