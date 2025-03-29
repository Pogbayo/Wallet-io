using SpagWallet.Domain.Enums.BankAccountEnums;

namespace SpagWallet.Application.DTOs.BankAccountDtoBranch
{
   public class UpdateBankAccountDto
    {
        public AccountType? AccountType { get; set; }
    }
}
