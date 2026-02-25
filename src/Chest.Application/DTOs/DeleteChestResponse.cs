namespace Chest.Application.DTOs;

public class DeleteChestResponse
{
    public string Message { get; set; } = "Baú deletado!"; 

    public DeleteChestResponse() { }
    public DeleteChestResponse(string message)
    {
        Message = message;
    }
}