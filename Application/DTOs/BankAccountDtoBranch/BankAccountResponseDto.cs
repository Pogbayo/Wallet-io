using SpagWallet.Domain.Enums.BankAccountEnums;

namespace SpagWallet.Application.DTOs.BankAccountDtoBranch
{
    public class BankAccountResponseDto
    {
        public Guid Id { get; set; }
        public string BankName { get; set; } = "Spag Bank";
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
    }
}
