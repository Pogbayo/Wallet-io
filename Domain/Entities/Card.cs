
using Domain.Entities;
using SpagWallet.Domain.Enums.CardEnums;
using System;

namespace SpagWallet.Domain.Entities
{
    public class Card
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid? WalletId { get; private set; }
        public virtual Wallet? Wallet { get; set; }

        public Guid BankAccountId { get; private set; }
        public virtual BankAccount BankAccount { get; private set; }

        public string CardNumber { get; private set; } = string.Empty;
        public string Cvv { get; private set; } 

        public CardTypeEnum CardType { get; private set; } = CardTypeEnum.Virtual;
        public CardProviderEnum CardProvider { get; private set; } = CardProviderEnum.Visa;
        public CardStatusEnum CardStatus { get; private set; } = CardStatusEnum.Active;

        private static readonly Random _random = new Random();

        public bool IsActive { get; private set; } = true;
        public DateTime ExpiryDate { get; private set; } = DateTime.UtcNow.AddYears(4);
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private Card() {
            Cvv = "000";
            BankAccount = null!;
        }

        public Card(Guid walletId, Guid bankAccountId, CardTypeEnum cardType, CardProviderEnum cardProvider)
        {
            if (BankAccount == null) throw new ArgumentException("BankAccount is required.");

            WalletId = walletId;
            BankAccountId = bankAccountId;
            CardNumber = GenerateCardNumber();
            Cvv = GenerateCvv();
            CardType = cardType;
            CardProvider = cardProvider;
        }

        private string GenerateCardNumber()
        {
            return $"{_random.Next(1000, 9999)} {_random.Next(1000, 9999)} {_random.Next(1000, 9999)} {_random.Next(1000, 9999)}";      
        }

        public bool ActivateCard()
        {
            if (IsActive)
                throw new InvalidOperationException("Card is already active.");
            IsActive = true;
            CardStatus = CardStatusEnum.Active;
            return true;
        }

        public bool BlockCard()
        {
            CardStatus = CardStatusEnum.Blocked;
            IsActive = false;

            return true;
        }

        public bool IsExpired() => ExpiryDate <= DateTime.UtcNow;

        //public void MarkAsExpired()
        //{
        //    if (IsExpired())
        //    {
        //        CardStatus = CardStatusEnum.Expired;
        //        IsActive = false;
        //    }
        //}

        public bool DeactivateCard()
        {
            if (!IsActive)
                throw new InvalidOperationException("Card does not exist.");

            IsActive = false;
            CardStatus = CardStatusEnum.Inactive;

            return true;
        }

        private string GenerateCvv()
        {
            return _random.Next(100, 999).ToString();
        }

        public string GetMaskedCardNumber()
        {
            if (CardNumber.Length < 4)
                return "****"; 
            return $"**** **** **** {CardNumber[^4..]}";
        }
    }
}
