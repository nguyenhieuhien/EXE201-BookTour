using System.Collections.Generic;
using System.Threading.Tasks;
using EXE201.Models;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetAllAsync();
    Task<Notification> GetByIdAsync(long id);
    Task AddAsync(Notification notification);
    Task UpdateAsync(Notification notification);
    Task DeleteAsync(long id);
}
