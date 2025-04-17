namespace EXE201.Controllers.DTO.Package
{
    public class PackageDTOCreate
    {
        //public long Id { get; set; }

        public long AccountId { get; set; }

        public long DestinationId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Rating { get; set; }

        public double Price { get; set; }

        //public bool IsActive { get; set; }

        public string PictureUrl { get; set; }
    }
}
