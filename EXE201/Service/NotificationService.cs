using EXE201.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;

    public NotificationService(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Notification>> GetAllNotifications()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Notification> GetNotificationById(long id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddNotification(Notification notification)
    {
        await _repository.AddAsync(notification);
    }

    public async Task UpdateNotification(Notification notification)
    {
        await _repository.UpdateAsync(notification);
    }

    public async Task DeleteNotification(long id)
    {
        await _repository.DeleteAsync(id);
    }
}
