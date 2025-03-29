using EXE201.Controllers.DTO.Service;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceDTO>>> GetAll()
        {
            var services = await _serviceService.GetAllAsync();
            var result = new List<ServiceDTO>();
            foreach (var service in services)
            {
                result.Add(new ServiceDTO
                {
                    Id = service.Id,
                    Name = service.Name,
                    Description = service.Description,
                    Price = service.Price,
                    IsActive = service.IsActive
                });
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceDTO>> GetById(long id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            if (service == null)
                return NotFound(new { Message = $"Service with ID {id} was not found." });

            return Ok(new ServiceDTO
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                IsActive = service.IsActive
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create(ServiceDTO serviceDTO)
        {
            var existingService = await _serviceService.GetByNameAsync(serviceDTO.Name);
            if (existingService != null)
            {
                return Conflict(new { Message = $"Service with name '{serviceDTO.Name}' already exists." });
            }

            var service = new Models.Service
            {
                Id = serviceDTO.Id,
                Name = serviceDTO.Name,
                Description = serviceDTO.Description,
                Price = serviceDTO.Price,
                IsActive = serviceDTO.IsActive
            };

            await _serviceService.AddAsync(service);

            return CreatedAtAction(nameof(GetById), new { id = service.Id }, new
            {
                Message = "Service created successfully.",
                Data = serviceDTO
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, ServiceDTOUpdate serviceDTOUpdate)
        {

            var existingService = await _serviceService.GetByIdAsync(id);
            if (existingService == null)
            {
                return NotFound(new { Message = $"No Service found with ID {id}." });
            }

            var existingName = await _serviceService.GetByNameAsync(serviceDTOUpdate.Name);
            if (existingName != null)
            {
                return Conflict(new { Message = $"Service with name '{serviceDTOUpdate.Name}' already exists." });
            }

            existingService.Name = serviceDTOUpdate.Name;
            existingService.Description = serviceDTOUpdate.Description;
            existingService.Price = serviceDTOUpdate.Price;

            await _serviceService.UpdateAsync(existingService);

            return Ok(new
            {
                Message = "Service updated successfully.",
                Data = new
                {
                    Id = existingService.Id,
                    Name = existingService.Name,
                    Description = existingService.Description,
                    Price = existingService.Price,
                    IsActive = existingService.IsActive
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existingService = await _serviceService.GetByIdAsync(id);
            if (existingService == null)

                return NotFound(new { Message = $"No Service found with ID {id}." });
            await _serviceService.DeleteAsync(id);
            return Ok(new
            {
                Message = $"Service with ID {id} has been deleted successfully.",
                Data = new
                {
                    Id = existingService.Id,
                    Name = existingService.Name,
                    Description = existingService.Description,
                    Price = existingService.Price,
                    IsActive = existingService.IsActive
                }
            });
        }
    }
}
