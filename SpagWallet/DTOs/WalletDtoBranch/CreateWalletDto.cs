
using SpagWallet.Domain.Entities;

namespace SpagWallet.Application.DTOs.WalletDtoBranch
{
    public class CreateWalletDto
    {
        public Guid UserId { get; set; }
        public required virtual User User { get; init; }
        public Guid BankAccountId { get; set; }
        public required BankAccount BankAccount { get; set; }
        public required string WalletPinHash { get; set; }
    }
}
