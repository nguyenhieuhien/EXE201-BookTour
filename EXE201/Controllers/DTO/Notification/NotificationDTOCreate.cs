namespace EXE201.Controllers.DTO.Notification
{
    public class NotificationDTOCreate
    {
        public long AccountId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        //public bool IsActive { get; set; }
    }
}
