namespace EXE201.Controllers.DTO.Booking
{
    public class BookingDTOUpdate
    {
        public string Description { get; set; } = null!;
        public DateTime BookingDate { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; } = null!;
    }
}
