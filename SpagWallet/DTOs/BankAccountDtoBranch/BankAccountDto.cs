using SpagWallet.Domain.Enums.BankAccountEnums;
using System.Transactions;

namespace SpagWallet.Application.DTOs.BankAccountDtoBranch
{
    public class BankAccountDto
    {
        public Guid Id { get;  set; }
        public string? AccountNumber { get;  set; }
        public string? BankName { get;  set; }
        public AccountType AccountType { get; set; }
        public decimal Balance { get;  set; }
        public DateTime CreatedAt { get;  set; } = DateTime.UtcNow;
    }
}
