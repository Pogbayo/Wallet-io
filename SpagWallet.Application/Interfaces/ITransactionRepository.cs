using SpagWallet.Application.DTOs.TransferDtoBranch;
using SpagWallet.Domain.Entities;
using SpagWallet.Domain.Enums.TransactionEnums;

namespace SpagWallet.Application.Interfaces
{
   public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetTransactionsAsync();
        Task<Transaction?> GetTransactionByIdAsync(Guid transactionId);
        Task<TransactionReceiptDto?> GenerateTransactionReceiptAsync(CreateTransactionDto transactionDto);
        Task UpdateTransactionStatusAsync(Guid transactionId, TransactionStatus status);
    }
}
