namespace EXE201.Controllers.DTO
{
    namespace EXE201.DTOs
    {
        public class AccountDTO
        {
            public string UserName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Phone { get; set; } = null!;
            public string Password { get; set; } = null!;
            public bool IsActive { get; set; }
            public long RoleId { get; set; }
        }
    }

}
