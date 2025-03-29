using System;

namespace EXE201.Controllers.DTO.Itinerary
{
    public class ItineraryDTO
    {
        public long Id { get; set; }
        public long PackageId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
