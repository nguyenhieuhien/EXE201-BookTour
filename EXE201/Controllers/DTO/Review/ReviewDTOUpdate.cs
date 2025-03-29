using System.Text.Json.Serialization;

namespace EXE201.Controllers.DTO.Review
{
    public class ReviewDTOUpdate
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
