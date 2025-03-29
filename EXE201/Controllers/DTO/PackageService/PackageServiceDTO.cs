using System;

namespace EXE201.Controllers.DTO.PackageService
{
    public class PackageServiceDTO
    {
        public long Id { get; set; }
        public long PackageId { get; set; }
        public long ServiceId { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
    }
}
