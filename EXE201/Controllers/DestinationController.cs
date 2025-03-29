using EXE201.Controllers.DTO.Destination;
using EXE201.Models;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/destinations")]
    public class DestinationController : ControllerBase
    {
        private readonly IDestinationService _destinationService;

        public DestinationController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DestinationDTO>>> GetAll()
        {
            var destinations = await _destinationService.GetAllDestinationsAsync();
            var result = new List<DestinationDTO>();
            foreach (var destination in destinations)
            {
                result.Add(new DestinationDTO
                {
                    Id = destination.Id,
                    Name = destination.Name,
                    Description = destination.Description,
                    Location = destination.Location,
                    IsActive = destination.IsActive,
                });
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DestinationDTO>> GetById(long id)
        {
            var destination = await _destinationService.GetDestinationByIdAsync(id);
            if (destination == null)
                return NotFound(new { Message = $"Destination with ID {id} was not found." });

            return Ok(new DestinationDTO
            {
                Id = destination.Id,
                Name = destination.Name,
                Description = destination.Description,
                Location = destination.Location,
                IsActive = destination.IsActive,
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create(DestinationDTO destinationDTO)
        {
            var destination = new Destination
            {
                Id = destinationDTO.Id,
                Name = destinationDTO.Name,
                Description = destinationDTO.Description,
                Location = destinationDTO.Location,
                IsActive = destinationDTO.IsActive,
            };

            await _destinationService.AddDestinationAsync(destination);

            return CreatedAtAction(nameof(GetById), new { id = destination.Id }, new
            {
                Message = "Destination created successfully.",
                Data = destinationDTO
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, DestinationDTOUpdate destinationDTOUpdate)
        {
            var existingDestination = await _destinationService.GetDestinationByIdAsync(id);
            if (existingDestination == null)
            {
                return NotFound(new { Message = $"No Destination found with ID {id}." });
            }

            existingDestination.Name = destinationDTOUpdate.Name;
            existingDestination.Description = destinationDTOUpdate.Description;
            existingDestination.Location = destinationDTOUpdate.Location;

            await _destinationService.UpdateDestinationAsync(existingDestination);

            return Ok(new
            {
                Message = "Destination updated successfully.",
                Data = new
                {
                    Id = existingDestination.Id,
                    Name = existingDestination.Name,
                    Description = existingDestination.Description,
                    Location = existingDestination.Location,
                    IsActive = existingDestination.IsActive,
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existingDestination = await _destinationService.GetDestinationByIdAsync(id);
            if (existingDestination == null)
            {
                return NotFound(new { Message = $"No Destination found with ID {id}." });
            }

            await _destinationService.DeleteDestinationAsync(id);
            return Ok(new
            {
                Message = $"Destination with ID {id} has been deleted successfully.",
                Data = new
                {
                    Id = existingDestination.Id,
                    Name = existingDestination.Name,
                    Description = existingDestination.Description,
                    Location = existingDestination.Location,
                    IsActive = existingDestination.IsActive,
                }
            });
        }
    }
}
