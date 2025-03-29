using System;

namespace EXE201.Controllers.DTO.Notification
{
    public class NotificationDTO
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
