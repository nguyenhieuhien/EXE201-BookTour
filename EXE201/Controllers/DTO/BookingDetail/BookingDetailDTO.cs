namespace EXE201.Controllers.DTO.BookingDetail
{
    public class BookingDetailDTO
    {
        public long Id { get; set; }
        public long BookingId { get; set; }
        public long PackageId { get; set; }
        public bool IsActive { get; set; }
    }
}
