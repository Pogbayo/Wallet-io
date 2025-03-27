using Microsoft.EntityFrameworkCore;
using SpagWallet.Application.Interfaces;
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

        public async Task<IEnumerable<Card?>> GetAllAsync()
        {
            var cardRecords = await _cardTable.ToListAsync();
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
    }
}
