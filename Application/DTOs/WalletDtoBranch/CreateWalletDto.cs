
using SpagWallet.Domain.Entities;

namespace SpagWallet.Application.DTOs.WalletDtoBranch
{
    public class CreateWalletDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BankAccountId { get; set; }
        public required string WalletPin { get; set; }
    }
}
