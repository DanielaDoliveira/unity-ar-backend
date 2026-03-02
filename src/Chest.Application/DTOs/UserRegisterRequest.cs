namespace Chest.Application.DTOs;

public class UserRegisterRequest
{
    public string Name { get; set; }
    public string Password { get; set; }

    public UserRegisterRequest() { }

    public UserRegisterRequest(string name, string password)
    {
        Name = name;
        Password = password;
    }
}