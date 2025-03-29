using System.Collections.Generic;
using System.Threading.Tasks;
using EXE201.Controllers.DTO.Itinerary;
using EXE201.DTO;
using EXE201.Models;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/itineraries")]
    public class ItineraryController : ControllerBase
    {
        private readonly IItineraryService _itineraryService;
        private readonly IPackageService _packageService;

        public ItineraryController(IItineraryService itineraryService, IPackageService packageService)
        {
            _itineraryService = itineraryService;
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItineraryDTO>>> GetAll()
        {
            var itineraries = await _itineraryService.GetAllItinerariesAsync();
            var result = new List<ItineraryDTO>();
            foreach (var itinerary in itineraries)
            {
                result.Add(new ItineraryDTO
                {
                    Id = itinerary.Id,
                    PackageId = itinerary.PackageId,
                    Date = itinerary.Date,
                    Description = itinerary.Description,
                    IsActive = itinerary.IsActive,
                });
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItineraryDTO>> GetById(long id)
        {
            var itinerary = await _itineraryService.GetItineraryByIdAsync(id);
            if (itinerary == null)
                return NotFound(new { Message = $"Itinerary with ID {id} was not found." });

            return Ok(new ItineraryDTO
            {
                Id = itinerary.Id,
                PackageId = itinerary.PackageId,
                Date = itinerary.Date,
                Description = itinerary.Description,
                IsActive = itinerary.IsActive,
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create(ItineraryDTO itineraryDTO)
        {
            var existingPackage = await _packageService.GetPackageByIdAsync(itineraryDTO.PackageId);
            if (existingPackage == null)
            {
                return NotFound(new { Message = $"Package with ID {itineraryDTO.PackageId} was not found." });
            }

            var itinerary = new Itinerary
            {
                Id = itineraryDTO.Id,
                PackageId = itineraryDTO.PackageId,
                Date = itineraryDTO.Date,
                Description = itineraryDTO.Description,
                IsActive = itineraryDTO.IsActive,
            };

            await _itineraryService.AddItineraryAsync(itinerary);

            return CreatedAtAction(nameof(GetById), new { id = itinerary.Id }, new
            {
                Message = "Itinerary created successfully.",
                Data = itineraryDTO
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, ItineraryDTOUpdate itineraryDTOUpdate)
        {
            var existingItinerary = await _itineraryService.GetItineraryByIdAsync(id);
            if (existingItinerary == null)
            {
                return NotFound(new { Message = $"No Itinerary found with ID {id}." });
            }

            existingItinerary.Date = itineraryDTOUpdate.Date;
            existingItinerary.Description = itineraryDTOUpdate.Description;

            await _itineraryService.UpdateItineraryAsync(existingItinerary);

            return Ok(new
            {
                Message = "Itinerary updated successfully.",
                Data = new
                {
                    Id = existingItinerary.Id,
                    PackageId = existingItinerary.PackageId,
                    Date = existingItinerary.Date,
                    Description = existingItinerary.Description,
                    IsActive = existingItinerary.IsActive,
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existingItinerary = await _itineraryService.GetItineraryByIdAsync(id);
            if (existingItinerary == null)
            {
                return NotFound(new { Message = $"No Itinerary found with ID {id}." });
            }

            await _itineraryService.DeleteItineraryAsync(id);
            return Ok(new
            {
                Message = $"Itinerary with ID {id} has been deleted successfully.",
                Data = new
                {
                    Id = existingItinerary.Id,
                    PackageId = existingItinerary.PackageId,
                    Date = existingItinerary.Date,
                    Description = existingItinerary.Description,
                    IsActive = existingItinerary.IsActive,
                }
            });
        }
    }
}