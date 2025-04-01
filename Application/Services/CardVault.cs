

using Application.Interfaces.RepoInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Entities;
using SpagWallet.Application.DTOs.CardDtoBranch;
using SpagWallet.Domain.Entities;

namespace Application.Services
{
    public class CardVault : ICardService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICardRepository _cardRepository;
    
        public Guid currentuserId { get; }
        public CardVault(
            IAuditLogRepository auditLogRepository,
            ICurrentUserService currentUserService,
            ICardRepository cardRepository
           )
        {
            _auditLogRepository = auditLogRepository;
            _currentUserService = currentUserService;
            _cardRepository = cardRepository;
            currentuserId = currentUserService.GetUserId();
           
        }

        public async Task<CardDto?> GetCardByIdAsync(Guid cardId)
        {
            var card = await _cardRepository.GetByIdAsync(cardId);

            if (card == null)
                return null;

            var auditLog = new AuditLog(
              action: "Fetching a card",
              performedBy: currentuserId,
              details: "Fetching card by Id."
            );

            await _auditLogRepository.AddLogAsync(auditLog);

            return new CardDto
            {
                Id = card.Id,
                WalletId = card.WalletId,
                BankAccountId = card.BankAccountId,
                MaskedCardNumber = card.GetMaskedCardNumber(),
                CardType = card.CardType,
                CardProvider = card.CardProvider,
                CardStatus = card.CardStatus,
                IsActive = card.IsActive,
                ExpiryDate = card.ExpiryDate,
                CreatedAt = card.CreatedAt
            };
        }

        public async Task<IEnumerable<CardDto?>> GetAllCardsAsync()
        {
            var list = await _cardRepository.GetAllAsync() ?? new List<Card>();

            if (!list.Any())
                throw new ArgumentException("Card list is empty.");

            var listDtos = list
                .Where(card => card != null)
                .Select(card => new CardDto
                {
                    Id = card!.Id,
                    WalletId = card.WalletId,
                    BankAccountId = card.BankAccountId,
                    MaskedCardNumber = card.GetMaskedCardNumber(),
                    CardType = card.CardType,
                    CardProvider = card.CardProvider,
                    CardStatus = card.CardStatus,
                    IsActive = card.IsActive,
                    ExpiryDate = card.ExpiryDate,
                    CreatedAt = card.CreatedAt
                });

            var auditLog = new AuditLog(
              action: "Fetching of cards",
              performedBy: currentuserId,
              details: "Fetching list of cards."
            );

            await _auditLogRepository.AddLogAsync(auditLog);

            return listDtos;
        }

        public async Task<bool> DeactivateCard(Guid cardId)
        {
            var cardRecord = await _cardRepository.GetByIdAsync(cardId);
            if (cardRecord == null)
            {
                throw new ArgumentException("Card does not exist.");
            }

            bool success = await _cardRepository.DeactivateCard(cardId);

            if (!success)
            {
                return false;
            }

            var auditLog = new AuditLog(
              action: "Deactivating card",
              performedBy: currentuserId,
              details: "Card deactivated."
            );

            await _auditLogRepository.AddLogAsync(auditLog);

            return success;
        }

        public async Task<bool> ActivateCard(Guid cardId)
        {
            var cardRecord = await _cardRepository.GetByIdAsync(cardId);
            if (cardRecord == null)
            {
                throw new ArgumentException("Card does not exist.");
            }
            bool success = await _cardRepository.ActivateCard(cardId);
            if (!success)
            {
                return false;
            }

            var auditLog = new AuditLog(
              action: "Card activation",
              performedBy: currentuserId,
              details: "Card activated."
            );

            await _auditLogRepository.AddLogAsync(auditLog);

            return success;
        }

        public async Task<bool> BlockCard(Guid cardId)
        {
            var cardRecord = await _cardRepository.GetByIdAsync(cardId);
            if (cardRecord == null)
            {
                throw new ArgumentException("Card does not exist.");
            }

            bool success = await _cardRepository.BlockCard(cardId);
            if (!success)
            {
                return false;
            }

            var auditLog = new AuditLog(
               action: "Blocking card",
               performedBy: currentuserId,
              details: "Card blocked."
               );

            await _auditLogRepository.AddLogAsync(auditLog);

            return success;
        }

        public async Task<bool> IsExpired(Guid cardId)
        {
            var cardRecord = await _cardRepository.GetByIdAsync(cardId);
            if (cardRecord == null)
            {
                throw new ArgumentException("Card does not exist.");
            }

            var auditLog = new AuditLog(
               action: "Checked Card Expiry Status",
               performedBy: currentuserId, 
               details: $"Checked expiry status of card with ID: {cardId}."
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return cardRecord.IsExpired();
        }

        public async Task<DateTime> CreatedAt(Guid cardId)
        {
            var cardRecord = await _cardRepository.GetByIdAsync(cardId); 
            if (cardRecord == null)
            {
                throw new ArgumentException("Card does not exist.");
            }
            var auditLog = new AuditLog(
              action: "Checked Card Creation Date",
              performedBy: currentuserId,
              details: $"Checked the created date of card with ID: {cardId}."
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return cardRecord.CreatedAt; 
        }
        public async Task<string?> GetCardNumber(Guid cardId)
        {
            var cardNumber = await _cardRepository.GetCardNumber(cardId);

            if (cardNumber == null)
            {
                throw new ArgumentException("Card does not exist.");
            }

            var auditLog = new AuditLog(
                action: "Checked Card Number",
                performedBy: currentuserId,
                details: $"Checked card number for card ID: {cardId}."
            );

            await _auditLogRepository.AddLogAsync(auditLog); 

            return cardNumber;
        }

        public async Task<string> GetCvv(Guid cardId)
        {
            var cvv = await _cardRepository.GetCvv(cardId);

            if (cvv == null)
            {
                throw new ArgumentException("Card does not exist.");
            }

            var auditLog = new AuditLog(
                action: "Checked Card CVV",
                performedBy: currentuserId,
                details: $"Checked CVV for card ID: {cardId}."
            );

            await _auditLogRepository.AddLogAsync(auditLog); 

            return cvv;
        }
    }
}
