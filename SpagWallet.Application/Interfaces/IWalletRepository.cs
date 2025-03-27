using SpagWallet.Domain.Entities;

namespace SpagWallet.Application.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetByIdAsync(Guid walletId);
        Task<Wallet?> GetByUserIdAsync(Guid userId);
        Task<Card?> GetWalletCardAsync(Guid walletId);
        Task<List<Transaction>> GetTransactionsAsync(Guid walletId);
        Task<bool> UpdateAsync(Wallet wallet);
        Task<bool> DeleteAsync(Guid walletId);
        Task<bool> CreateAsync(Wallet wallet);
    }
}
