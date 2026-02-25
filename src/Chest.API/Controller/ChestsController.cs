using Chest.Application.Constants;
using Chest.Application.DTOs;
using Chest.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Chest.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChestsController : ControllerBase
    {
        
        private readonly IChestCreationService _creationService;
        
        // Injetamos o serviço no construtor
        public ChestsController(IChestCreationService creationService)
        {
        
            _creationService = creationService;
        }
        [HttpPost("create-chest")]
        [SwaggerOperation(Summary = ChestSwaggerDocs.CreateSummary, Description = ChestSwaggerDocs.CreateDescription)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateChest([FromBody] CreateChestRequest request)
        {
            var chestId = await _creationService.CreateChestAsync(request);
            return StatusCode(StatusCodes.Status201Created, new { Id = chestId, Message = "Chest Created" });
        
        }

        
        [HttpPost("find-chest")]
        [SwaggerOperation(Summary = ChestSwaggerDocs.FindSummary, Description = ChestSwaggerDocs.FindDescription)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> FindChest([FromBody] FindChest dto)
        {
            // // A lógica de busca agora consome os dados que vieram no DTO
            // var chest = await _chestService.FindNearestAvailableAsync(dto.Latitude, dto.Longitude, 100);
            //
            // if (chest == null)
            // {
            //     // Regra: Se em 100m não houver nada disponível, retorna vazio (404)
            //     return StatusCode(StatusCodes.Status404NotFound, new { message = "Área limpa: nenhum tesouro por aqui." });
            // }
            return StatusCode(StatusCodes.Status200OK, "Chest Founded!");
            
        }

        [HttpPut("update-status/{chestId}")]
        [SwaggerOperation(Summary = ChestSwaggerDocs.UpdateSummary, Description = ChestSwaggerDocs.UpdateDescription)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        
        public async Task<IActionResult> UpdateStatus(Guid chestId, [FromBody] UpdateChest dto)
        {
            // // O ATO: Pedimos ao serviço para processar a ocupação
            // var success = await _chestService.UpdateChestStatusAsync(id, dto);
            //
            // if (!success) 
            //     return StatusCode(StatusCodes.Status404NotFound);

            // 204 No Content: Sucesso, o baú agora é oficialmente deste jogador
            return StatusCode(StatusCodes.Status204NoContent);
        }
        
        [HttpGet("user-chests/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = ChestSwaggerDocs.GetSummary, Description = ChestSwaggerDocs.GetDescription)]
        public async Task<IActionResult> GetUserChests(Guid userId)
        {
          //  var chest = await _repository.GetByIdAsync(userId); 
            //if (chest == null) return NotFound();
      
            return StatusCode(StatusCodes.Status200OK, "List of user chests");
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = ChestSwaggerDocs.DeleteSummary, Description = ChestSwaggerDocs.DeleteDescription)]
        public async Task<IActionResult> Delete(Guid id)
        {
            // // O Service verificará NOVAMENTE se IsSomebodyPlaying é false (Segurança de Backend)
            // var result = await _chestService.DeleteChestAsync(id);
            //
            // if (result == "NOT_FOUND") return StatusCode(StatusCodes.Status404NotFound);
            //
            // if (result == "IS_PLAYING") 
            //     return StatusCode(StatusCodes.Status400BadRequest, new { message = "Baú ocupado no momento." });

            // Retorna a sua DeleteChestResponse com a mensagem de sucesso
            return StatusCode(StatusCodes.Status200OK, new DeleteChestResponse("Baú deletado com sucesso!"));
        }
    }
}
