using API.Common;
using Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpagWallet.Application.DTOs.TransferDtoBranch;
using SpagWallet.Domain.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _fundflow;
        public TransactionController(ITransactionService fundflow)
        {
            _fundflow = fundflow;
        }

        [Authorize]
        [HttpGet("get-all-transactions")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetTransactionDetailsDto>>>> GetAllTransactions()
        {
            var transactionList = await _fundflow.GetAllTransactionsAsync();
            if (transactionList == null)
            {
                return NotFoundResponse<IEnumerable<GetTransactionDetailsDto>>(
                    new List<string> { "Error getting transactions" },
                    "Transactions not fetched successfully");
            }
            return Success
             (transactionList,
             "Transactions retrieved successfully.");
        }

        [Authorize]
        [HttpGet("get-transaction-by/{transactionId}")]
        public async Task<ActionResult<ApiResponse<GetTransactionDetailsDto>>> GetTransactionById(Guid transactionId)
        {
            var transactionRecord = await _fundflow.GetTransactionByIdAsync(transactionId);
            if (transactionRecord == null)
            {
                return NotFoundResponse<GetTransactionDetailsDto>(
                    new List<string> { "Error getting transaction" },
                    "User not fetched successfully");
            }
            return Success
             (transactionRecord,
             "Transactions retrieved successfully.");
        }

        [Authorize]
        [HttpPost("generate-transaction-receipt")]
        public async Task<ActionResult<ApiResponse<TransactionReceiptDto>>> ProcessTransactionReceipt([FromBody] Transaction transactiondata)
        {
            var processedReceipt = await _fundflow.ProcessReceiptAsync(transactiondata);
            if (processedReceipt == null)
            {
                return NotFoundResponse<TransactionReceiptDto>(
                    new List<string> { "Error generating transaction receipt" },
                    "transaction receipt not generated successfully");
            }
            return Success(processedReceipt, "Receipt generated successfully.");
        }
    }
}
