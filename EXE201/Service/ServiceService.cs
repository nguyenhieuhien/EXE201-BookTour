using EXE201.Models;
using EXE201.Repository.Interface;
using EXE201.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EXE201.Service
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Models.Service> GetByIdAsync(long id)
        {
            return await _serviceRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Models.Service>> GetAllAsync()
        {
            return await _serviceRepository.GetAllAsync();
        }

        public async Task<Models.Service?> GetByNameAsync(string name)
        {
            return await _serviceRepository.GetByNameAsync(name);
        }

        public async Task AddAsync(Models.Service service)
        {
            await _serviceRepository.AddAsync(service);
        }

        public async Task UpdateAsync(Models.Service service)
        {
            await _serviceRepository.UpdateAsync(service);
        }

        public async Task DeleteAsync(long id)
        {
            await _serviceRepository.DeleteAsync(id);
        }
    }
}
