using Domain.Entities;
using SpagWallet.Domain.Entities;

namespace Application.Interfaces.RepoInterfaces
{
    public interface IWalletRepository
    {
        Task<List<Wallet>> GetAllWalletsAsync();
        Task<Wallet?> GetByIdAsync(Guid walletId);
        Task<Wallet?> GetByUserIdAsync(Guid userId);
        Task<Card?> GetWalletCardAsync(Guid walletId);
        Task<List<Transaction>> GetTransactionsAsync(Guid walletId);
        Task<bool> UpdateAsync(Wallet wallet);
        Task<bool> DeleteAsync(Guid walletId);
        Task<bool> CreateAsync(Wallet wallet);
        Task SaveAsync();
        Task<bool> AddFundsAsync(Guid walletId, decimal amount);
        Task<bool> WithdrawFundsAsync(Guid walletId, decimal amount);
        Task<bool> TransferFundsAsync(Guid senderWalletId, Guid receiverWalletId, decimal amount);
    }
}
