
using EXE201.Controllers.DTO.Notification;
using EXE201.Models;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IAccountService _accountService;

        public NotificationController(INotificationService notificationService, IAccountService accountService)
        {
            _notificationService = notificationService;
            _accountService = accountService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetAll()
        {
            var notifications = await _notificationService.GetAllNotifications();
            var result = new List<NotificationDTO>();
            foreach (var notification in notifications)
            {
                result.Add(new NotificationDTO
                {
                    Id = notification.Id,
                    AccountId = notification.AccountId,
                    Title = notification.Title,
                    Description = notification.Description,
                    IsActive = notification.IsActive,
                });
            }
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDTO>> GetById(long id)
        {
            var notification = await _notificationService.GetNotificationById(id);
            if (notification == null)
                return NotFound(new { Message = $"Notification with ID {id} was not found." });

            return Ok(new NotificationDTO
            {
                Id = notification.Id,
                AccountId = notification.AccountId,
                Title = notification.Title,
                Description = notification.Description,
                IsActive = notification.IsActive,
            });
        }


        [HttpPost]
        public async Task<ActionResult> Create(NotificationDTO notificationDTO)
        {
            var existingAccount = await _accountService.GetAccountByIdAsync(notificationDTO.AccountId);

            if (existingAccount == null)
            {
                return NotFound(new { Message = $"Account with ID {notificationDTO.AccountId} was not found." });
            }

            var notification = new Notification
            {
                Id = notificationDTO.Id,
                AccountId = notificationDTO.AccountId,
                Title = notificationDTO.Title,
                Description = notificationDTO.Description,
                IsActive = notificationDTO.IsActive,
            };

            await _notificationService.AddNotification(notification);

            return CreatedAtAction(nameof(GetById), new { id = notification.Id }, new
            {
                Message = "Notification created successfully.",
                Data = notificationDTO
            });
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, NotificationDTOUpdate notificationDTOUpdate)
        {

            var existingNotification = await _notificationService.GetNotificationById(id);
            if (existingNotification == null)
            {
                return NotFound(new { Message = $"No Notification found with ID {id}." });
            }

            existingNotification.Title = notificationDTOUpdate.Title;
            existingNotification.Description = notificationDTOUpdate.Description;   

            await _notificationService.UpdateNotification(existingNotification);

            return Ok(new
            {
                Message = "Notification updated successfully.",
                Data = new
                {
                    Id = existingNotification.Id,
                    AccountId = existingNotification.AccountId,
                    Title = existingNotification.Title,
                    Description = existingNotification.Description,
                    IsActive = existingNotification.IsActive,
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existingNotification = await _notificationService.GetNotificationById(id);
            if (existingNotification == null)
            {
                return NotFound(new { Message = $"No Notifcation found with ID {id}." });
            }

            await _notificationService.DeleteNotification(id);
            return Ok(new
            {
                Message = $"Delete with ID {id} has been deleted successfully.",
                Data = new
                {
                    Id = existingNotification.Id,
                    AccountId = existingNotification.AccountId,
                    Title = existingNotification.Title,
                    Description = existingNotification.Description,
                    IsActive = existingNotification.IsActive,
                }
            });
        }
    }
}
