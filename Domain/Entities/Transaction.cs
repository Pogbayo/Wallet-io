using SpagWallet.Domain.Enums.TransactionEnums;

namespace SpagWallet.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? WalletId { get; private set; }
        public Guid? BankAccountId { get; private set; }

        public decimal Amount { get; set; }
        public required string Reference { get; set; }

        public TransactionType Type { get;  set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
        public TransactionSource Source { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Transaction() {}

        public Transaction(decimal amount, TransactionType transactionType, string reference, Guid? bankAccountId = null, Guid? walletId = null)
        {
            if (walletId == null && bankAccountId == null)
            {
                throw new ArgumentException("Transaction must be linked to either a Wallet or a BankAccount.");
            }

            if (amount <= 0) throw new ArgumentException("Transaction amount must be greater than zero.");
            if (string.IsNullOrWhiteSpace(reference)) throw new ArgumentException("Transaction reference is required.");

            WalletId = walletId;
            Amount = amount;
            Type = transactionType;
            Reference = reference;
            Status = TransactionStatus.Pending;
            BankAccountId = bankAccountId;
            CreatedAt = DateTime.UtcNow;
            Source = walletId != null ? TransactionSource.Wallet : TransactionSource.BankAccount;
        }

        public void MarkAsCompleted()
        {
            if (Status == TransactionStatus.Pending)
            {
                Status = TransactionStatus.Completed;
            }
        }


        public void CancelTransaction()
        {
            if (Status == TransactionStatus.Pending)
            {
                Status = TransactionStatus.Cancelled;
            }
        }

        public bool IsSuccessful()
        {
            return Status == TransactionStatus.Completed;
        }

        public bool IsPending()
        {
            return Status == TransactionStatus.Pending;
        }
    }
}

