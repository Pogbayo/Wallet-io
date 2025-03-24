
using SpagWallet.Domain.Entities;

namespace SpagWallet.Application.Interfaces
{
   public interface IBankAccount
    {
        Task<List<BankAccount>> GetAllAsync();
        Task<BankAccount> CreateAsync(BankAccount bankAccount);
        Task<BankAccount?> GetByIdAsync(Guid bankAccountId);
        Task<List<BankAccount>> GetByUserIdAsync(Guid userId);
        Task<BankAccount?> GetByWalletIdAsync(Guid walletId);
        Task<BankAccount> UpdateAsync(BankAccount bankAccount);
        Task<bool> DeleteAsync(Guid bankAccountId);
        Task<bool> UpdateBalanceAsync(Guid walletId, decimal amount); 
        Task<List<Transaction>> GetTransactionsAsync(Guid bankAccountId);
    }
}
