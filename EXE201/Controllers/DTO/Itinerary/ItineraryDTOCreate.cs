namespace EXE201.Controllers.DTO.Itinerary
{
    public class ItineraryDTOCreate
    {
        public long PackageId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
    }
}
