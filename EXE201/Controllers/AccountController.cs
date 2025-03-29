using EXE201.Controllers.DTO.EXE201.DTOs;

using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccounts()
        {
            return Ok(await _accountService.GetAllAccountsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> GetAccount(long id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
                return NotFound();
            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountDTO accountDto)
        {
            if (accountDto == null) return BadRequest("Invalid account data.");

            await _accountService.AddAccountAsync(accountDto);
            return CreatedAtAction(nameof(GetAccount), accountDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(long id, [FromBody] AccountDTO accountDto)
        {
            if (accountDto == null) return BadRequest("Invalid account data.");

            var existingAccount = await _accountService.GetAccountByIdAsync(id);
            if (existingAccount == null) return NotFound();

            await _accountService.UpdateAccountAsync(id, accountDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(long id)
        {
            var existingAccount = await _accountService.GetAccountByIdAsync(id);
            if (existingAccount == null) return NotFound();

            await _accountService.DeleteAccountAsync(id);
            return NoContent();
        }
    }
}
