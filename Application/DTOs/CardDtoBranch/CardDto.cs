using SpagWallet.Domain.Enums.CardEnums;

namespace SpagWallet.Application.DTOs.CardDtoBranch
{
    public class CardDto
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public Guid BankAccountId { get; set; }
        public string? MaskedCardNumber { get; set; }
        public CardTypeEnum CardType { get; set; }
        public CardProviderEnum CardProvider { get; set; }
        public CardStatusEnum CardStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
