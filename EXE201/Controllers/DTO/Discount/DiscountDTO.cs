using System;

namespace EXE201.Controllers.DTO.Discount
{
    public class DiscountDTO
    {
        public long Id { get; set; }
        public string Code { get; set; } = null!;
        public int Percentage { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }
}
