using EXE201.Controllers.DTO.Account;
using EXE201.Models;
using EXE201.Service;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
  
        private readonly IAccountService _accountService;
        private readonly ICartService _cartService;

        public AccountsController(IAccountService accountService ,ICartService cartService)
        {
            _accountService = accountService;
            _cartService = cartService;
        }


        [HttpGet]
public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAll()
{
    try
    {
        var accounts = await _accountService.GetAllAsync();
        var result = accounts.Select(account => new AccountDTO
        {
            Id = account.Id,
            UserName = account.UserName,
            Password = account.Password,
            Email = account.Email,
            Phone = account.Phone,
            IsActive = account.IsActive,
        }).ToList();
    }catch (Exception ex)
    {
        Console.WriteLine(ex.Message.ToString());
    }

    return Ok();
}



        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> GetById(long id)
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account == null)
                return NotFound(new { Message = $"Account with ID {id} was not found." });

            return Ok(new AccountDTO
            {
                Id = account.Id,
                RoleId = account.RoleId,
                UserName = account.UserName,
                Password = account.Password,
                Email = account.Email,
                Phone = account.Phone,
                IsActive = account.IsActive,
            });
        }


        [HttpPost]
        public async Task<ActionResult> Create(AccountDTOCreate accountDTOCreate)
        {
            //var existingAccount = await _accountService.GetByIdAsync(accountDTOCreate.Id);
            var existingUsername = await _accountService.GetByNameAsync(accountDTOCreate.UserName);
            //if (existingAccount != null)
            //{
            //    return Conflict(new { Message = $"Account with ID {accountDTOCreate.Id} was found." });
            //}

            if (existingUsername != null)
            {
                return Conflict(new { Message = $"Account with Username {accountDTOCreate.UserName} was found." });
            }
            

            var account = new Account
            {
                //Id = accountDTOCreate.Id,
                RoleId = 3,
                UserName = accountDTOCreate.UserName,
                Password = accountDTOCreate.Password,
                Email = accountDTOCreate.Email,
                Phone = accountDTOCreate.Phone,
                IsActive = true,
            };

            await _accountService.AddAsync(account);

            ///////////////////////////////////////////
          
            var cart = new Cart
            {
                AccountId = account.Id,
                IsActive = true
            };
            await _cartService.AddCartAsync(cart);

            return CreatedAtAction(nameof(GetById), new { id = account.Id }, new
            {
                Message = "Account created successfully.",
                Data = accountDTOCreate
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, AccountDTOUpdate accountDTOUpdate)
        {

            var existingAccount = await _accountService.GetByIdAsync(id);
            if (existingAccount == null)
            {
                return NotFound(new { Message = $"Account with ID {id} was not found." });
            }

            existingAccount.UserName = accountDTOUpdate.UserName;
            existingAccount.Password = accountDTOUpdate.Password;
            existingAccount.Email = accountDTOUpdate.Email;
            existingAccount.Phone = accountDTOUpdate.Phone; 
               await _accountService.UpdateAsync(existingAccount);

            return Ok(new
            {
                Message = "Account updated successfully.",
                Data = new
                {
                   Id = existingAccount.Id,
                   RoleId = existingAccount.RoleId,
                   UserName = existingAccount.UserName,
                   Password = existingAccount.Password,
                   Email = existingAccount.Email,
                   Phone = existingAccount.Phone,
                   IsActive = existingAccount.IsActive,

                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existingAccount = await _accountService.GetByIdAsync(id);
            if (existingAccount == null)

                return NotFound(new { Message = $"No Account found with ID {id}." });
            await _accountService.DeleteAsync(id);
            return Ok(new
            {
                Message = $"Account with ID {id} has been deleted successfully.",
                Data = new
                {
                    Id = existingAccount.Id,
                    RoleId = existingAccount.RoleId,
                    UserName = existingAccount.UserName,
                    Password = existingAccount.Password,
                    Email = existingAccount.Email,
                    Phone = existingAccount.Phone,
                    IsActive = existingAccount.IsActive,
                }
            });
        }
    }
}
