using SpagWallet.Domain.Entities;

namespace Application.Interfaces.RepoInterfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(Guid transactionId);
        Task<bool> AddAsync(Transaction transaction);
        Task<bool> UpdateAsync(Transaction transaction);
    }
}
