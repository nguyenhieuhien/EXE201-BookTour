using System.Collections.Generic;
using System.Threading.Tasks;
using EXE201.Models;

public interface INotificationService
{
    Task<IEnumerable<Notification>> GetAllNotifications();
    Task<Notification> GetNotificationById(long id);
    Task AddNotification(Notification notification);
    Task UpdateNotification(Notification notification);
    Task DeleteNotification(long id);
}
