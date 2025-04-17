namespace EXE201.Controllers.DTO.Itinerary
{
    public class ItineraryDTOCreate
    {
        public long PackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
    }
}
