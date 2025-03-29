using SpagWallet.Application.DTOs.BankAccountDtoBranch;

namespace SpagWallet.Application.DTOs.UserDtoBranch
{
   public class UserBankDetails
    {
        public Guid Id { get; set; }
        public BankAccountDto? BankAccount { get; set; }
    }
}
