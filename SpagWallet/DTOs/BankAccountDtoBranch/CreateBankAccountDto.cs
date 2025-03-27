using SpagWallet.Domain.Enums.BankAccountEnums;

namespace SpagWallet.Application.DTOs.BankAccountDtoBranch
{
   public class CreateBankAccountDto
    {
        public Guid Id { get; set; }
        public AccountType AccountType { get; set; }
        public decimal InitialDeposit { get; set; }
    }
}
