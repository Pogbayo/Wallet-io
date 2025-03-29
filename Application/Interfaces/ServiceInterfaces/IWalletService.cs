using SpagWallet.Domain.Entities;


namespace Application.Interfaces.ServiceInterfaces
{
    public interface IWalletService
    {
        Task<Wallet?> GetWalletByIdAsync(Guid walletId);
        Task<Wallet?> GetWalletByUserIdAsync(Guid userId);
        Task<bool> CreateWalletAsync(Wallet wallet);
        Task<bool> DeleteWalletAsync(Guid walletId);
    }
}
