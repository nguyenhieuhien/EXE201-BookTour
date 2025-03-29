using EXE201.Controllers.DTO.PackageService;
using EXE201.Service;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/packageservices")]
    public class PackageServiceController : ControllerBase
    {
        private readonly IPackageServiceService _packageServiceService;
        private readonly IPackageService _packageService;
        private readonly IServiceService _service;

        public PackageServiceController(IPackageServiceService packageServiceService, IPackageService packageService, IServiceService service)
        {
            _packageServiceService = packageServiceService;
            _packageService = packageService;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackageServiceDTO>>> GetAll()
        {
            var packageServices = await _packageServiceService.GetAllPackageServicesAsync();
            var result = new List<PackageServiceDTO>();
            foreach (var packageService in packageServices)
            {
                result.Add(new PackageServiceDTO
                {
                    Id = packageService.Id,
                    PackageId = packageService.PackageId,
                    ServiceId = packageService.ServiceId,
                    Price = packageService.Price,
                    IsActive = packageService.IsActive
                });
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PackageServiceDTO>> GetById(long id)
        {
            var packageService = await _packageServiceService.GetPackageServiceByIdAsync(id);
            if (packageService == null)
                return NotFound(new { Message = $"PackageService with ID {id} was not found." });

            return Ok(new PackageServiceDTO
            {
                Id = packageService.Id,
                PackageId = packageService.PackageId,
                ServiceId = packageService.ServiceId,
                Price = packageService.Price,
                IsActive = packageService.IsActive
            });
        }
        [HttpPost]
        public async Task<ActionResult> Create(PackageServiceDTO packageServiceDTO)
        {
            var existingService = await _service.GetByIdAsync(packageServiceDTO.ServiceId);
            var existingPackage = await _packageService.GetPackageByIdAsync(packageServiceDTO.PackageId);
            if (existingService == null)
            {
                return NotFound(new { Message = $"Service with ID {packageServiceDTO.ServiceId} was not found." });
            }
            if (existingPackage == null)
            {
                return NotFound(new { Message = $"Package with ID {packageServiceDTO.PackageId} was not found." });
            }
            var packageService = new Models.PackageService
            {
                Id = packageServiceDTO.Id,
                PackageId = packageServiceDTO.PackageId,
                ServiceId = packageServiceDTO.ServiceId,
                Price = packageServiceDTO.Price,
                IsActive = packageServiceDTO.IsActive
            };

            await _packageServiceService.AddPackageServiceAsync(packageService);

            return CreatedAtAction(nameof(GetById), new { id = packageService.Id }, new
            {
                Message = "PackageService created successfully.",
                Data = packageServiceDTO
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, PackageServiceDTOUpdate packageServiceDTOUpdate)
        {

            var existingPackageService = await _packageServiceService.GetPackageServiceByIdAsync(id);
            if (existingPackageService == null)
            {
                return NotFound(new { Message = $"No PackageService found with ID {id}." });
            }


            existingPackageService.Price = packageServiceDTOUpdate.Price;

            await _packageServiceService.UpdatePackageServiceAsync(existingPackageService);

            return Ok(new
            {
                Message = "PackageService updated successfully.",
                Data = new
                {
                    Id = existingPackageService.Id,
                    PackageId = existingPackageService.PackageId,
                    ServiceId = existingPackageService.ServiceId,
                    Price = existingPackageService.Price,
                    IsActive = existingPackageService.IsActive
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existingPackageService = await _packageServiceService.GetPackageServiceByIdAsync(id);
            if (existingPackageService == null)
            {
                return NotFound(new { Message = $"No PackageService found with ID {id}." });
            }

            await _packageServiceService.DeletePackageServiceAsync(id);
            return Ok(new
            {
                Message = $"PackageService with ID {id} has been deleted successfully.",
                Data = new
                {
                    Id = existingPackageService.Id,
                    PackageId = existingPackageService.PackageId,
                    ServiceId = existingPackageService.ServiceId,
                    Price = existingPackageService.Price,
                    IsActive = existingPackageService.IsActive
                }
            });
        }

    }
    }
