using System;

namespace EXE201.Controllers.DTO.Destination
{
    public class DestinationDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Location { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
