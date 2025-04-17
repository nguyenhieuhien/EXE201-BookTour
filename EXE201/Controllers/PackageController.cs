﻿using EXE201.Controllers.DTO.Package;
using EXE201.Models;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/packages")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly IAccountService _accountService;
        private readonly IDestinationService _destinationService;

        public PackageController(IPackageService packageService, IAccountService accountService, IDestinationService destinationService)
        {
            _packageService = packageService;
            _accountService = accountService;
            _destinationService = destinationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackageDTO>>> GetAll()
        {
            var packages = await _packageService.GetAllPackagesAsync();
            var result = new List<PackageDTO>();
            foreach (var package in packages)
            {
                result.Add(new PackageDTO
                {
                    Id = package.Id,
                    AccountId = package.AccountId,
                    DestinationId = package.DestinationId,
                    Name = package.Name,
                    Description = package.Description,
                    Rating = package.Rating,
                    Price = package.Price,
                    PictureUrl = package.PictureUrl,
                    IsActive = package.IsActive,
                });
            }
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PackageDTO>> GetById(long id)
        {
            var package = await _packageService.GetPackageByIdAsync(id);
            if (package == null)
                return NotFound(new { Message = $"Package with ID {id} was not found." });

            return Ok(new PackageDTO
            {
                Id = package.Id,
                AccountId = package.AccountId,
                DestinationId = package.DestinationId,
                Name = package.Name,
                Description = package.Description,
                Rating = package.Rating,
                Price = package.Price,
                PictureUrl = package.PictureUrl,
                IsActive = package.IsActive,
            });
        }


        [HttpPost]
        public async Task<ActionResult> Create(PackageDTOCreate packageDTOCreate)
        {
            var existingAccount = await _accountService.GetByIdAsync(packageDTOCreate.AccountId);
            var existingDestination = await _destinationService.GetDestinationByIdAsync(packageDTOCreate.DestinationId);
            if (existingAccount == null)
            {
                return NotFound(new { Message = $"Account with ID {packageDTOCreate.AccountId} was not found." });
            }
            if (existingDestination == null)
            {
                return NotFound(new { Message = $"Destination with ID {packageDTOCreate.AccountId} was not found." });
            }

            var package = new Package
            {
                //Id = packageDTOCreate.Id,
                AccountId = packageDTOCreate.AccountId,
                DestinationId = packageDTOCreate.DestinationId,
                Name = packageDTOCreate.Name,
                Description = packageDTOCreate.Description,
                Rating = packageDTOCreate.Rating,
                Price = packageDTOCreate.Price,
                PictureUrl = packageDTOCreate.PictureUrl,
                IsActive = true,
            };

            await _packageService.AddPackageAsync(package);

            return CreatedAtAction(nameof(GetById), new { id = package.Id }, new
            {
                Message = "Package created successfully.",
                Data = packageDTOCreate
            });
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, PackageDTOUpdate packageDTOUpdate)
        {

            var existingPackage = await _packageService.GetPackageByIdAsync(id);
            if (existingPackage == null)
            {
                return NotFound(new { Message = $"No Package found with ID {id}." });
            }
            existingPackage.Name = packageDTOUpdate.Name;
            existingPackage.Description = packageDTOUpdate.Description;
            existingPackage.Rating = packageDTOUpdate.Rating;
            existingPackage.Price = packageDTOUpdate.Price;
            existingPackage.PictureUrl = packageDTOUpdate.PictureUrl;

            await _packageService.UpdatePackageAsync(existingPackage);

            return Ok(new
            {
                Message = "Package updated successfully.",
                Data = new
                {
                    Id = existingPackage.Id,
                    AccountId = existingPackage.AccountId,
                    DestinationId = existingPackage.DestinationId,
                    Name = existingPackage.Name,
                    Description = existingPackage.Description,
                    Rating = existingPackage.Rating,
                    Price = existingPackage.Price,
                    PictureUrl = existingPackage.PictureUrl,
                    IsActive = existingPackage.IsActive,
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existingPackage = await _packageService.GetPackageByIdAsync(id);
            if (existingPackage == null)
            {
                return NotFound(new { Message = $"No Package found with ID {id}." });
            }

            await _packageService.DeletePackageAsync(id);
            return Ok(new
            {
                Message = $"Package with ID {id} has been deleted successfully.",
                Data = new
                {
                    Id = existingPackage.Id,
                    AccountId = existingPackage.AccountId,
                    DestinationId = existingPackage.DestinationId,
                    Name = existingPackage.Name,
                    Description = existingPackage.Description,
                    Rating = existingPackage.Rating,
                    Price = existingPackage.Price,
                    PictureUrl = existingPackage.PictureUrl,
                    IsActive = existingPackage.IsActive,
                }
            });
        }
    }
}
