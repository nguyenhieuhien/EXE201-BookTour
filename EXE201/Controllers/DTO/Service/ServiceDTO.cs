namespace EXE201.Controllers.DTO.Service
{
    public class ServiceDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public bool IsActive { get; set; }
    }
}
