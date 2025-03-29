using System;

namespace EXE201.Controllers.DTO.Package
{
    public class PackageDTO
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public long DestinationId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Rating { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
    }
}
