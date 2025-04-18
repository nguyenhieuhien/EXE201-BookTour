namespace EXE201.Controllers.DTO.Booking
{
    public class BookingDTOCreate
    {
        public long AccountId { get; set; }
        //public long? DiscountId { get; set; }
        public string Description { get; set; } = null!;
        public DateTime BookingDate { get; set; }
        //public double TotalPrice { get; set; }
        //public string Status { get; set; } = null!;
    }
}
