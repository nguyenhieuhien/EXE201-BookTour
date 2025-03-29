namespace EXE201.Controllers.DTO.Package
{
    public class PackageDTOUpdate
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Rating { get; set; }
        public double Price { get; set; }
    }
}
