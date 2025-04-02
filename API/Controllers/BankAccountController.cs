using API.Common;
using Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpagWallet.Application.DTOs.BankAccountDtoBranch;
using SpagWallet.Domain.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : BaseController
    {
        private readonly IBankAccountService _accountService;

        public BankAccountController(IBankAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
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

        [Authorize]
        [HttpGet("get-bankaccount-by-id/{bankaccountid}")]
        public async Task<ActionResult<ApiResponse<BankAccountDto?>>> GetBankAccountById(Guid bankaccountid)
        {
            var bankaccount = await _accountService.GetBankAccountByIdAsync(bankaccountid);
            if (bankaccount == null)
            {
                return Failure<BankAccountDto?>(new List<string> { "Error fetching bank account" }, "Bank account not fetched");
            }
            return Success<BankAccountDto?>(bankaccount, "Bank account fetched successfully.");
        }

        [Authorize]
        [HttpGet("get-bankaccount-by-userId/{userId}")]
        public async Task<ActionResult<ApiResponse<BankAccountDto?>>> GetBankAccountByUserId(Guid userId)
        {
            var bankaccount = await _accountService.GetBankAccountByUserIdAsync(userId);
            if (bankaccount == null)
            {
                return Failure<BankAccountDto?>(new List<string> { "Error fetching bank account" }, "Bank account not fetched");
            }
            return Success<BankAccountDto?>(bankaccount, "Bank account fetched successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update-account-type/{bankaccountId}")]
        public async Task<ActionResult<ApiResponse<BankAccountDto?>>> UpdateAccountType(Guid bankaccountId, [FromBody] UpdateBankAccountDto updateDto)
        {
            var bankaccount = await _accountService.UpdateAccountType(bankaccountId, updateDto);
            if (bankaccount == null)
            {
                return Failure<BankAccountDto?>(new List<string> { "Error updating bank account type" }, "Bank account type not updated");
            }
            return Success<BankAccountDto?>(bankaccount, "Bank account type updated successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-bankaccount-by-id/{bankaccountid}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteBankAccount(Guid bankaccountid)
        {
            bool success = await _accountService.DeleteBankAccountAsync(bankaccountid);
            if (!success)
            {
                return Failure<bool>(new List<string> { "Error deleting bank account" }, "Bank account not deleted");
            }
            return Success<bool>(success, "Bank account deleted successfully.");
        }

        [Authorize]
        [HttpPost("add-funds/{bankaccountId}")]
        public async Task<ActionResult<ApiResponse<bool>>> AddFunds(Guid bankaccountid, [FromBody] decimal amount)
        {
            bool isFunded = await _accountService.AddFundsAsync(bankaccountid, amount);
            if (!isFunded)
            {
                return Failure<bool>(new List<string> { "Error funding bank account" }, "Bank account not funded successfully");
            }
            return Success<bool>(isFunded, "Bank account funded successfully");
        }

        [Authorize]
        [HttpPost("withdraw-from-bankaccount/{bankaccountid}")]
        public async Task<ActionResult<ApiResponse<bool>>> WithdrawFunds(Guid bankaccountid, [FromBody] decimal amount)
        {
            bool isWithdrawn = await _accountService.WithdrawFundsAsync(bankaccountid, amount);
            if (!isWithdrawn)
            {
                return Failure<bool>(new List<string> { "Withdrawal failed" }, "Insufficient balance or invalid request");
            }
            return Success(isWithdrawn, "Withdrawal successful.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("transfer-funds")]
        public async Task<ActionResult<ApiResponse<bool>>> TransferFunds([FromBody] Guid senderbankaccountId, [FromBody] Guid receiverbankaccountId, decimal amount)
        {
            var isTransferred = await _accountService.TransferFundsAsync(senderbankaccountId, receiverbankaccountId, amount);
            if (!isTransferred)
            {
                return Failure<bool>(new List<string> { "Transfer failed" }, "Invalid sender, receiver, or insufficient balance");
            }
            return Success(isTransferred, "Funds transferred successfully");
        }
    }
}
