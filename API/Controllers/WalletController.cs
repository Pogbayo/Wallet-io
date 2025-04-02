using API.Common;
using Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using SpagWallet.Application.DTOs.WalletDtoBranch;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("get-all-wallets")]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<ApiResponse<IEnumerable<WalletDto?>>>> GetAllWallets()
        {
            var wallets = await _walletService.GetWallets();
            if (wallets == null)
            {
                return NotFoundResponse<IEnumerable<WalletDto?>>(
                   new List<string> { "Error getting users" },
                   "Users not fetched successfully");
            }

            return Success(wallets, "Users retrieved successfully.");
        }

        [HttpDelete("delete-wallet-by/{walletId}")]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<ApiResponse<bool>>> DeleteWallet(Guid walletId)
        {
            bool isDeleted = await _walletService.DeleteWalletAsync(walletId);
            if (!isDeleted)
            {
                return Failure<bool>(new List<string> { "Wallet not deleted" }, "Error deleting wallet with the provided ID");
            }
            return Success<bool>(isDeleted, "Wallet deleted successfully.");
        }

        [HttpGet("get-wallet-by-userid/{userId}")]
        [Authorize(Roles = "Admin,User")] 
        public async Task<ActionResult<ApiResponse<WalletDto>>> GetWalletByUserId(Guid userId)
        {
            var retrievedWallet = await _walletService.GetWalletByUserIdAsync(userId);
            if (retrievedWallet == null)
            {
                return Failure<WalletDto>(new List<string> { "Wallet not found" }, "No wallet exists with the provided ID");
            }
            return Success<WalletDto>(retrievedWallet, "Wallet created successfully");
        }

        [HttpPost("create-wallet")]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<ApiResponse<WalletDto>>> CreateWallet([FromBody] CreateWalletDto walletData)
        {
            var createdWallet = await _walletService.CreateWalletAsync(walletData);
            if (createdWallet == null)
            {
                return Failure<WalletDto>(new List<string> { "Wallet not found" }, "No wallet exists with the provided ID");
            }
            return Success<WalletDto>(createdWallet, "Wallet created successfully");
        }

        [HttpPost("fund-wallet/{walletId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<bool>>> AddFunds(Guid walletId, [FromBody] decimal amount)
        {
            bool isFunded = await _walletService.AddFundsAsync(walletId, amount);
            if (!isFunded)
            {
                return Failure<bool>(new List<string> { "Wallet not funded successfully" }, "Error funding wallet successfully");
            }
            return Success<bool>(isFunded, "Wallet funded successfully");
        }

        [HttpPost("withdraw-from-wallet/{walletId}")]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<ApiResponse<bool>>> WithdrawFunds(Guid walletId, [FromBody] decimal amount)
        {
            bool isWithdrawn = await _walletService.WithdrawFundsAsync(walletId, amount);
            if (!isWithdrawn)
            {
                return Failure<bool>(new List<string> { "Withdrawal failed" }, "Insufficient balance or invalid request");
            }
            return Success(isWithdrawn, "Withdrawal successful.");
        }

        [HttpPost("transfer-funds")]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<ApiResponse<bool>>> TransferFunds(Guid senderwalletId, Guid receiverwalletId, decimal amount)
        {
            var isTransferred = await _walletService.TransferFundsAsync(senderwalletId, receiverwalletId, amount);
            if (!isTransferred)
            {
                return Failure<bool>(new List<string> { "Transfer failed" }, "Invalid sender, receiver, or insufficient balance");
            }
            return Success(isTransferred, "Funds transferred successfully");
        }
    }
}
