using SpagWallet.Domain.Entities;

namespace SpagWallet.Application.Interfaces
{
   public interface ICardRepository
    {
        Task<Card> AddCardAsync(Card card);
        Task<Card> GetByIdAsync(Guid cardId);
        Task<IEnumerable<Card>> GetAllAsync();
        Task<IEnumerable<Card>> GetByWalletIdAsync(Guid walletId);
        Task<IEnumerable<Card>> GetByBankAccountIdAsync(Guid bankAccountId);
    }
}
