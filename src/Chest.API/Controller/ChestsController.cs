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
        private readonly IChestSearchService _searchService;
        private readonly IChestDeletionService _deletionService;
        private readonly IChestHuntService _huntService;
        // Injetamos o serviço no construtor
        public ChestsController(IChestCreationService creationService, IChestSearchService searchService, IChestDeletionService deletionService, IChestHuntService huntService)
        {
            _creationService = creationService;
            _searchService = searchService;
            _deletionService = deletionService;
            _huntService = huntService;
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
        public async Task<IActionResult> FindChest([FromBody] FindChestRequest request)
        {
            var chest = await _huntService.FindNearestAvailableAsync(request.Latitude, request.Longitude);

            if (chest == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Nenhum baú disponível por perto");
            }

       
            return StatusCode(StatusCodes.Status200OK, "Baú encontrado!");
          
            
        }

        [HttpPut("lock/{chestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = ChestSwaggerDocs.UpdateSummary, Description = ChestSwaggerDocs.UpdateDescription)]
        public async Task<IActionResult> LockChest(Guid chestId, [FromQuery] Guid userId)
        {
            var success = await _huntService.LockChestAsync(chestId, userId);

            if (!success)
            {
                return Conflict(new { message = "Este baú já foi ocupado por outro caçador!" });
            }

            return StatusCode(StatusCodes.Status200OK, "Mapa traçado!");
        }
        
        [HttpGet("user-chests/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(Summary = ChestSwaggerDocs.GetSummary, Description = ChestSwaggerDocs.GetDescription)]
        public async Task<IActionResult> GetUserChests(Guid userId)
        {
            var chests = await _searchService.GetUserChestsAsync(userId);
            if(!chests.Any()) return StatusCode(StatusCodes.Status204NoContent);
      
            return StatusCode(StatusCodes.Status200OK,chests);
        }
        
        [HttpDelete("{chestId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = ChestSwaggerDocs.DeleteSummary, Description = ChestSwaggerDocs.DeleteDescription)]
        public async Task<IActionResult> Delete(Guid chestId, [FromQuery] Guid userId) 
        {
            // Agora passamos os DOIS IDs para o serviço
            var deleted = await _deletionService.DeleteChestAsync(chestId, userId);

            if (!deleted)
                return StatusCode(StatusCodes.Status404NotFound);

           
            return StatusCode(StatusCodes.Status204NoContent); 
        }
    }
}
