using Chest.Application.DTOs;
using Chest.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Chest.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
  
            var userId = await _authService.RegisterAsync(request);
            
            return Ok(new 
            { 
                UserId = userId, 
                Message = "Pirata recrutado com sucesso! Bem-vindo ao bando." 
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            // Passamos o objeto 'request' completo aqui também
            var userId = await _authService.LoginAsync(request);
        
            if (userId == null)
                return Unauthorized(new { message = "Nome ou senha de marujo inválidos!" });
            
            return Ok(new { UserId = userId });
        }
        
        [HttpDelete("delete-account")] 
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteUserRequest request)
        {
        
            await _authService.DeleteAccountAsync(request);
    
            return Ok(new { Message = "O pirata caminhou na prancha e todos os seus registros foram apagados." });
        }
    }
}