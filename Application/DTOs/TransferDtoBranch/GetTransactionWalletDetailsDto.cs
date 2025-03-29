
using SpagWallet.Application.DTOs.WalletDtoBranch;

namespace SpagWallet.Application.DTOs.TransferDtoBranch
{
    public class GetTransactionWalletDetailsDto : GetTransactionDetailsDto
    {
        public WalletDto? Wallet { get; set; }
    }
}
