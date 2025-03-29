using SpagWallet.Application.DTOs.WalletDtoBranch;
using SpagWallet.Domain.Enums.TransactionEnums;


namespace SpagWallet.Application.DTOs.TransferDtoBranch
{
    public class GetTransactionDetailsDto
    {
        public Guid Id { get; set; }
        public Guid? WalletId { get; set; }
        public Guid? BankAccountId { get; set; }

        public decimal Amount { get; set; }
        public required string Reference { get; set; }

        public TransactionType Type { get;  set; }
        public TransactionStatus Status { get;  set; }
        public TransactionSource Source { get; set; }

        public DateTime CreatedAt { get; private set; } 
    }
}
