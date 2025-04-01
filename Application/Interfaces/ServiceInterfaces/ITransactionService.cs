using SpagWallet.Application.DTOs.TransferDtoBranch;
using SpagWallet.Domain.Entities;

namespace Application.Interfaces.ServiceInterfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<GetTransactionDetailsDto>> GetAllTransactionsAsync();
        Task<GetTransactionDetailsDto?> GetTransactionByIdAsync(Guid transactionId);
        Task<TransactionReceiptDto?> ProcessReceiptAsync(Transaction transaction);
        Task<TransactionReceiptDto> AddAsync(Transaction transaction);
    }
}
