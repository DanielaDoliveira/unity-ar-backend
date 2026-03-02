namespace Chest.Application.DTOs;

public class UserLoginRequest
{
    public string Name { get; set; }
    public string Password { get; set; }

    public UserLoginRequest() { }

    public UserLoginRequest(string name, string password)
    {
        Name = name;
        Password = password;
    }
}