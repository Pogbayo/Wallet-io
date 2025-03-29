using SpagWallet.Domain.Enums.CardEnums;

namespace SpagWallet.Application.DTOs.CardDtoBranch
{
    public class CardDetailsDto
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public Guid BankAccountId { get; set; }
        public string? CardNumber { get; set; }
        public string? Cvv { get; set; }
        public CardTypeEnum CardType { get; set; }
        public CardProviderEnum CardProvider { get; set; }
        public CardStatusEnum CardStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
