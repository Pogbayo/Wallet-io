using SpagWallet.Domain.Entities;

namespace SpagWallet.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(Guid transactionId);
        Task<bool> AddAsync(Transaction transaction);
        Task<bool> UpdateAsync(Transaction transaction);
    }
}
