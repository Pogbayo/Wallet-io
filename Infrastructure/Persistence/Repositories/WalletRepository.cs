using Application.Interfaces.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using SpagWallet.Application.DTOs.WalletDtoBranch;
using SpagWallet.Domain.Entities;
using SpagWallet.Infrastructure.Persistence.Data;


namespace SpagWallet.Infrastructure.Persistence.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Wallet> _walletTable;

        public WalletRepository(AppDbContext context)
        {
            _context = context;
            _walletTable = context.Wallets;
        }

        public async Task<Wallet?> GetByIdAsync(Guid walletId)
        {
            return await _walletTable.FindAsync(walletId);
        }

        public async Task<Wallet?> GetByUserIdAsync(Guid userId)
        {
            return await _walletTable.FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<Card?> GetWalletCardAsync(Guid walletId)
        {
            var wallet = await _walletTable.Include(w => w.Card).FirstOrDefaultAsync(w => w.Id == walletId);
            return wallet?.Card;
        }

        public async Task<List<Transaction>> GetTransactionsAsync(Guid walletId)
        {
            return await _context.Transactions
                .Where(t => t.WalletId == walletId)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Wallet wallet)
        {
            _walletTable.Update(wallet);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid walletId)
        {
            var wallet = await _walletTable.FindAsync(walletId);
            if (wallet == null) return false;

            _walletTable.Remove(wallet);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateAsync(Wallet wallet)
        {
            await _walletTable.AddAsync(wallet);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddFundsAsync(Guid walletId, decimal amount)
        {
            var wallet = await GetByIdAsync(walletId);
            if (wallet == null) return false;

            wallet.Credit(amount);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> WithdrawFundsAsync(Guid walletId, decimal amount)
        {
            var wallet = await GetByIdAsync(walletId);
            if (wallet == null || wallet.Balance < amount) return false;

            wallet.Withdraw(amount);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> TransferFundsAsync(Guid senderWalletId, Guid receiverWalletId, decimal amount)
        {
            var senderWallet = await GetByIdAsync(senderWalletId);
            var receiverWallet = await GetByIdAsync(receiverWalletId);

            if (senderWallet == null || receiverWallet == null || senderWallet.Balance < amount)
                return false;

            senderWallet.Withdraw(amount);
            receiverWallet.Credit(amount);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Wallet>> GetAllWalletsAsync()
        {
            var walletList = await _walletTable.ToListAsync();
            return walletList;
        }
    }
}
