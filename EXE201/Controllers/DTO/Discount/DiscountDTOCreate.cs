namespace EXE201.Controllers.DTO.Discount
{
    public class DiscountDTOCreate
    {
        public string Code { get; set; } = null!;
        public int Percentage { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
