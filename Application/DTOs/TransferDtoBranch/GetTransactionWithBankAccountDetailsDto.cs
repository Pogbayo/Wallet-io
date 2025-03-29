using SpagWallet.Application.DTOs.BankAccountDtoBranch;

namespace SpagWallet.Application.DTOs.TransferDtoBranch
{
   public class GetTransactionWithBankAccountDetailsDto
    {
        public BankAccountDto? BankAccount { get; set; }

    }
}
