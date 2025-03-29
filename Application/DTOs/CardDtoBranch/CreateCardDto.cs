
using SpagWallet.Domain.Enums.CardEnums;

namespace SpagWallet.Application.DTOs.CardDtoBranch
{
    public class CreateCardDto
    {
        public Guid WalletId { get; set; }
        public Guid BankAccountId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public CardTypeEnum CardType { get; set; }
        public CardProviderEnum CardProvider { get; set; }
    }
}
