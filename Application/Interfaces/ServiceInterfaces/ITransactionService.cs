using SpagWallet.Domain.Entities;

namespace Application.Interfaces.ServiceInterfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction?> GetTransactionByIdAsync(Guid transactionId);
        Task<bool> ProcessTransactionAsync(Transaction transaction);
    }
}
