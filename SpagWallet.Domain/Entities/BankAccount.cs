using SpagWallet.Domain.Enums.BankAccountEnums;
using SpagWallet.Domain.Enums.CardEnums;

namespace SpagWallet.Domain.Entities
{
   public class BankAccount
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid UserId { get; private set; }
        public required virtual User User { get; init; }
        public required virtual Card? Card { get; set; }

        public string? AccountNumber { get; private set; }
        public string BankName { get; private set; } = "Spag Bank";
        public AccountType AccountType { get; set; } = AccountType.Savings;

        public Wallet? Wallet { get; set; }
        public decimal Balance { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public virtual List<Transaction> Transactions { get;private set; } = new() ;

        private BankAccount() {}

        public BankAccount( User user, string accountNumber, string bankName, string accountType, decimal initialDeposit)
        {
            if (user == null) throw new ArgumentException("UserId is required.");
            if (string.IsNullOrWhiteSpace(accountNumber)) throw new ArgumentException("Account number is required.");
            if (string.IsNullOrWhiteSpace(bankName)) throw new ArgumentException("Bank name is required.");
            if (initialDeposit < 0) throw new ArgumentException("Initial balance cannot be negative.");
        
                UserId =  user.Id;
                User = user;
                AccountNumber = GenerateUniqueAccountNumber();
                BankName = bankName;
                AccountType = AccountType.Savings;
                Balance = initialDeposit;
        }

        public void Credit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Deposit amount must be greater than zero");
            Balance += amount;
        }

        private string GenerateUniqueAccountNumber()
        {
            Random random = new Random();
            long min = 1000000000L;  
            long max = 9999999999L;  

            long accountNumber = min + (long)(random.NextDouble() * (max - min));
            return accountNumber.ToString();
        }

        public void GenerateCard()
        {
            if (Card is not null)
                throw new InvalidOperationException("Card already exists for this bank account.");

            if (Wallet == null)
                throw new InvalidOperationException("Wallet must be assigned before generating a card.");

            Card = new Card(Wallet.Id, Id, CardTypeEnum.Virtual, CardProviderEnum.Visa);
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("withdrawal amount must be gretaer than Zero");
            if (amount > Balance) throw new ArgumentException("Insufficient funds");
            Balance -= amount;
        }

        public string GetFormattedBalance()
        {
            return $"Current Balance: ${Balance}";
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction is null) throw new ArgumentException("Transaction cannot be null");
            Transactions.Add(transaction);
        }
    }
}
