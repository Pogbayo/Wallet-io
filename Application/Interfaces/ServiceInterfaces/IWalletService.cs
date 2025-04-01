using SpagWallet.Application.DTOs.TransferDtoBranch;
using SpagWallet.Application.DTOs.WalletDtoBranch;

namespace Application.Interfaces.ServiceInterfaces
{
    public interface IWalletService
    {
        Task<IEnumerable<WalletDto?>?> GetWallets();
        Task<WalletDto?> GetWalletByIdAsync(Guid walletId);
        Task<WalletDto?> GetWalletByUserIdAsync(Guid userId);
        Task<WalletDto?> CreateWalletAsync(CreateWalletDto walletdata);
        Task<bool> DeleteWalletAsync(Guid walletId);
        Task<bool> AddFundsAsync(Guid walletId, decimal amount);
        Task<bool> WithdrawFundsAsync(Guid walletId, decimal amount);
        Task<IEnumerable<GetTransactionDetailsDto>> GetTransactionsAsync(Guid walletId);
        Task<bool> TransferFundsAsync(Guid senderWalletId, Guid receiverWalletId, decimal amount);
    }
}
