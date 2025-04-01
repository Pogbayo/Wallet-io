using SpagWallet.Domain.Entities;

namespace Application.Interfaces.RepoInterfaces
{
   public interface ICardRepository
    {
        Task<Card?> GetByIdAsync(Guid cardId);
        Task<IEnumerable<Card?>> GetAllAsync();
        Task<Card?> GetByWalletIdAsync(Guid walletId);
        Task<Card?> GetByBankAccountIdAsync(Guid bankAccountId);
        Task<bool> DeactivateCard(Guid cardId);
        Task<bool> ActivateCard(Guid cardId);
        Task<bool> BlockCard(Guid cardId);
        Task<bool> IsExpired(Guid cardId);
        Task<DateTime> CreatedAt(Guid cardId);
        Task<string?> GetCardNumber(Guid cardId);
        Task<string> GetCvv(Guid cardId);
    }
}
