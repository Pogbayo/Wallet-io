using SpagWallet.Domain.Entities;

namespace Application.Interfaces.ServiceInterfaces
{
    public interface ICardService
    {
        Task<Card?> GetCardByIdAsync(Guid cardId);
        Task<IEnumerable<Card?>> GetAllCardsAsync();
    }
}
