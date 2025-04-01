using Application.Interfaces.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using SpagWallet.Domain.Entities;
using SpagWallet.Infrastructure.Persistence.Data;

namespace SpagWallet.Infrastructure.Persistence.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Card> _cardTable;

        public CardRepository(AppDbContext context)
        { 
          _context = context;
          _cardTable = context.Cards;
        }

        public async Task<bool> ActivateCard(Guid cardId)
        {
            var cardRecord = await GetByIdAsync(cardId);
            if (cardRecord is null)
                throw new ArgumentException("No card recorded.");
            bool success = cardRecord.ActivateCard();
            await _context.SaveChangesAsync();

            return success;
        }

        public async Task<bool> BlockCard(Guid cardId)
        {
            var cardRecord = await GetByIdAsync(cardId);
            if (cardRecord is null)
                throw new ArgumentException("No card recorded.");
            bool success = cardRecord.BlockCard();
            await _context.SaveChangesAsync();

            return success;
        }

        public async Task<DateTime> CreatedAt(Guid cardId)
        {
            var cardRecord = await GetByIdAsync(cardId);
            if (cardRecord is null)
                throw new ArgumentException("No card recorded.");

            DateTime datetime = cardRecord.CreatedAt;
            return datetime;
        }

        public async Task<string> GetCvv(Guid cardId)
        {
            var cardRecord = await GetByIdAsync(cardId);
            if (cardRecord is null)
                throw new ArgumentException("No card recorded.");

            return cardRecord.Cvv;
        }

        public async Task<bool> DeactivateCard(Guid cardId)
        {
            var cardRecord = await GetByIdAsync(cardId);
            if (cardRecord is null)
                return false;
            bool success = cardRecord.DeactivateCard();
            await _context.SaveChangesAsync();
            return success;
        }

        public async Task<IEnumerable<Card?>> GetAllAsync()
        {
            var cardRecords = await _cardTable.ToListAsync() ?? new List<Card>();
            if (cardRecords.Count == 0)
                throw new ArgumentException("No card recorded.");

            return cardRecords;
        }
        
        public async Task<Card?> GetByBankAccountIdAsync(Guid bankAccountId)
        {
            var cardRecord = await _cardTable.FirstOrDefaultAsync(c => c.Id == bankAccountId);

            if (cardRecord is null)
                return null;

            return cardRecord;
        }

        public async Task<Card?> GetByIdAsync(Guid cardId)
        {
            var cardRecord = await _cardTable.FirstOrDefaultAsync(c => c.Id == cardId);

            if (cardRecord is null)
                return null;

            return cardRecord;
        }

        public async Task<Card?> GetByWalletIdAsync(Guid walletId)
        {
            var cardRecord = await _cardTable.FirstOrDefaultAsync(c => c.Id == walletId);

            if (cardRecord is null)
                return null;

            return cardRecord;
        }

        public async Task<string?> GetCardNumber(Guid cardId)
        {
            var cardRecord = await GetByIdAsync(cardId);
            if (cardRecord is null)
                return null;

            var cardNumber = cardRecord.CardNumber;
            return cardNumber;
        }

        public async Task<bool> IsExpired(Guid cardId)
        {
            var cardRecord = await GetByIdAsync(cardId);
            if (cardRecord is null)
                return false;
            bool isExpired = cardRecord.IsExpired();
            return isExpired;
        }
    }
}
