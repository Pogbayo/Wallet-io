
using SpagWallet.Application.DTOs.WalletDtoBranch;

namespace SpagWallet.Application.DTOs.UserDtoBranch
{
   public class UserWalletDetails
    {
        public Guid Id { get; set; }
        public WalletDto? Wallet { get; set; }
    }
}
