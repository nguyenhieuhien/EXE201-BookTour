using System;

namespace EXE201.Controllers.DTO.Itinerary
{
    public class ItineraryDTO
    {
        public long Id { get; set; }

        public long PackageId { get; set; }

        public DateTime StartDate { get; set; }


        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

    }
}
