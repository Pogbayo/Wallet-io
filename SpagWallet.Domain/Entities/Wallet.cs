
namespace SpagWallet.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public virtual User? User { get; init; }

        public Guid BankAccountId { get; set; }
        public  BankAccount? BankAccount { get; set; }

        public decimal Balance { get; private set; } = 0m;
        public string Currency { get; set; } = "NGN";
        public Guid WalletNumber { get; set; } = Guid.NewGuid();
        public required string WalletPinHash { get; set; }
        public bool IsLocked { get; private set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Card? Card { get; set; }
        public virtual List<Transaction> Transactions { get; private set; } = new();

        public Wallet() {}

        public Wallet(Guid userId,string walletPin, decimal balance, string currency)
        {
            
            if (userId == Guid.Empty) throw new ArgumentException("User ID is required.");
            if (string.IsNullOrWhiteSpace(walletPin) || walletPin.Length != 4) throw new ArgumentException("Wallet PIN must be exactly 4 digits.");

            UserId = userId;
            WalletPinHash = HashPin(walletPin);
            Balance = balance;
            Currency = string.IsNullOrWhiteSpace(currency) ? "NGN" : currency;
            CreatedAt = DateTime.UtcNow;
        }

        private string HashPin(string pin)
        {
            return BCrypt.Net.BCrypt.HashPassword(pin);
        }

        public void Credit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.");
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (IsLocked) throw new InvalidOperationException("Wallet is locked. Cannot withdraw funds.");
            if (amount <= 0) throw new ArgumentException("Withdrawal amount must be greater than zero.");
            if (amount > Balance) throw new InvalidOperationException("Insufficient funds.");

            Balance -= amount;
        }

        public void LockWallet() => IsLocked = true;
        public void UnlockWallet() => IsLocked = false;

        public void UpdatePin(string newPin)
        {
            if (string.IsNullOrWhiteSpace(newPin) || newPin.Length != 4)
                throw new ArgumentException("PIN must be exactly 4 digits.");

            WalletPinHash = HashPin(newPin);
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction is null) throw new ArgumentException("Transaction cannot be null");
            Transactions.Add(transaction);
        }
    }
}

