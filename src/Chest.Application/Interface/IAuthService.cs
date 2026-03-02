using Chest.Application.DTOs;

namespace Chest.Application.Interface;

public interface IAuthService
{
    /// <summary>
    /// Valida o request, cria um novo pirata e guarda o hash da senha no banco.
    /// </summary>
    Task<Guid> RegisterAsync(UserRegisterRequest request);

    /// <summary>
    /// Valida o request, verifica as credenciais e retorna o UserId se estiver correto.
    /// </summary>
    Task<Guid?> LoginAsync(UserLoginRequest request);
    Task DeleteAccountAsync(DeleteUserRequest request);
}