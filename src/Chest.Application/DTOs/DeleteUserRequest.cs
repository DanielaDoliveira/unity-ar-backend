namespace Chest.Application.DTOs;

public class DeleteUserRequest
{
    public Guid UserId { get; set; }
    public string ConfirmPassword { get; set; } // Segurança extra!

    public DeleteUserRequest() { }
}