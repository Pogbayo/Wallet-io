using SpagWallet.Application.DTOs.WalletDtoBranch;
using SpagWallet.Domain.Entities;

namespace SpagWallet.Application.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetByIdAsync(Guid walletId);
        Task<Wallet?> GetByUserIdAsync(Guid userId);
        Task<Card?> GetWalletCardAsync(Guid walletId);
        Task<List<Transaction>> GetTransactionsAsync(Guid walletId);

        Task<bool> UpdateAsync(Guid walletId, CreateWalletDto walletData);
        Task<bool> DeleteWalletAsync(Guid walletId);

        Task<CreateWalletResponse> CreateAsync(CreateWalletDto walletData);
        Task<bool> ChangeWalletPinAsync(Guid walletId, string newPinHash);
        Task<bool> WithdrawAsync(Guid walletId, decimal amount);
        Task<bool> DepositAsync(Guid walletId, decimal amount); 
        Task<bool> SetWalletLockStatusAsync(Guid walletId, bool isLocked);
    }
}
