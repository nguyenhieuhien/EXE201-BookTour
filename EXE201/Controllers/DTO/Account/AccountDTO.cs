namespace EXE201.Controllers.DTO.Account
{
        public class AccountDTO
        {
            public long Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; } 
            public string Phone { get; set; }
            public string Password { get; set; } 
            public bool IsActive { get; set; }
            public long RoleId { get; set; }
        }
    }
