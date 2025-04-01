using API.Common;
using Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using SpagWallet.Application.DTOs.BankAccountDtoBranch;
using SpagWallet.Application.Services;
using SpagWallet.Domain.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : BaseController
    {
        private readonly IBankAccountService _accountService;
        public BankAccountController
           (
           IBankAccountService AccountService
           )
        {
            _accountService = AccountService;
        }

        [HttpPost("create-bankaccount")]
        public async Task<ActionResult<ApiResponse<BankAccountDto?>>> CreateBankAccount(BankAccount bankaccountdata)
        {
            var bankAccount = await _accountService.CreateAsync(bankaccountdata);
            if (bankAccount == null)
            {
                return Failure<BankAccountDto?>(new List<string> { "Error creating bank account" }, "Bank account not created");

            }
            return Success<BankAccountDto?>(bankAccount, "Bank account created successfully.");
        }

        [HttpGet("get-bankaccount-by-id/{bankaccountid}")]
        public async Task<ActionResult<ApiResponse<BankAccountDto?>>> GetBankAccountById(Guid bankaccountid)
        {
            var bankaccount = await _accountService.GetBankAccountByIdAsync(bankaccountid);
            if (bankaccount == null)
            {
                return Failure<BankAccountDto?>(new List<string> { "Error fetching bank account" }, "Bank account not fetched");
            }
            return Success<BankAccountDto?>(bankaccount, "Bank account fecthed successfully.");
        }

        [HttpGet("get-bankaccount-by-userId/{userId}")]
        public async Task<ActionResult<ApiResponse<BankAccountDto?>>> GetBankAccountByUserId(Guid userId)
        {
            var bankaccount = await _accountService.GetBankAccountByUserIdAsync(userId);
            if (bankaccount == null)
            {
                return Failure<BankAccountDto?>(new List<string> { "Error fetching bank account" }, "Bank account not fetched");
            }
            return Success<BankAccountDto?>(bankaccount, "Bank account fecthed successfully.");
        }

        [HttpPost("update-account-type/{bankaccountId}")]
        public async Task<ActionResult<ApiResponse<BankAccountDto?>>> UpdateAccountType(Guid bankaccountId, [FromBody]UpdateBankAccountDto updateDto)
        {
            var bankaccount = await _accountService.UpdateAccountType(bankaccountId, updateDto);
            if (bankaccount == null)
            {
                return Failure<BankAccountDto?>(new List<string> { "Error updating bank account type" }, "Bank account type not updated");
            }
            return Success<BankAccountDto?>(bankaccount, "Bank account type updated successfully.");
        }
    }
}
