using SpagWallet.Application.DTOs.CardDtoBranch;

namespace Application.Interfaces.ServiceInterfaces
{
    public interface ICardService
    {
        Task<CardDto?> GetCardByIdAsync(Guid cardId);
        Task<IEnumerable<CardDto?>> GetAllCardsAsync();
        Task<bool> DeactivateCard(Guid cardId);
        Task<bool> ActivateCard(Guid cardId);
        Task<bool> BlockCard(Guid cardId);
        Task<bool> IsExpired(Guid cardId);
        Task<DateTime> CreatedAt(Guid cardId);
        Task<string?> GetCardNumber(Guid cardId);
        Task<string> GetCvv(Guid cardId);
    }
}
