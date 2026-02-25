namespace Chest.Application.DTOs;

public class UpdateChestLogout
{
    public Guid RivalId { get; set; }
    public bool IsSomebodyPlaying { get; set; }

    public UpdateChestLogout()
    {
        IsSomebodyPlaying = false;
        RivalId = Guid.Empty;

    }
    
    
}