using SpagWallet.Domain.Entities;

namespace Application.Interfaces.RepoInterfaces
{
   public interface ICardRepository
    {
        Task<Card?> GetByIdAsync(Guid cardId);
        Task<IEnumerable<Card?>> GetAllAsync();
        Task<Card?> GetByWalletIdAsync(Guid walletId);
        Task<Card?> GetByBankAccountIdAsync(Guid bankAccountId);
    }
}
