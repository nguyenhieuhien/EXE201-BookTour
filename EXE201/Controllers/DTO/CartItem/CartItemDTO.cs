namespace EXE201.Controllers.DTO.CartItem
{
    public class CartItemDTO
    {
        public long Id { get; set; }
        public long PackageId { get; set; }
        public long CartId { get; set; }
        public bool IsActive { get; set; }
    }
}
