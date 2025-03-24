using Microsoft.EntityFrameworkCore;
using SpagWallet.Application.DTOs.WalletDtoBranch;
using SpagWallet.Application.Interfaces;
using SpagWallet.Domain.Entities;
using SpagWallet.Infrastructure.Persistence.Data;
using System.Linq.Expressions;

namespace SpagWallet.Infrastructure.Persistence.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _context;
        public WalletRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ChangeWalletPinAsync(Guid walletId, string newPinHash)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet is null)
                return false;
            wallet.WalletPinHash = newPinHash;
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<CreateWalletResponse> CreateAsync(CreateWalletDto walletObject)
        {
            if (walletObject is null)
            {
                throw new ArgumentException("Wallet details can not be null");
            }

            try
            {
                var wallet = new Wallet
                {
                    Id = Guid.NewGuid(),
                    UserId = walletObject.UserId,
                    BankAccountId = walletObject.BankAccountId,
                    WalletPinHash = walletObject.WalletPinHash
                };

                 await _context.Wallets.AddAsync(wallet);
                int result = await _context.SaveChangesAsync();

                return new CreateWalletResponse
                {
                    IsSuccess = result > 0,
                    WalletId = wallet.Id,
                    ErrorMessage = result > 0 ? null :"Failed to save wallet to the database."
                };
            }
            catch (Exception ex)
            {
                return new CreateWalletResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
           
        public async Task<bool> DeleteWalletAsync(Guid walletId)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet is null)
            {
                return false;
            }
            _context.Wallets.Remove(wallet);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DepositAsync(Guid walletId, decimal amount)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet is null)
                return false;

            wallet.Credit(amount);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Card?> GetWalletCardAsync(Guid walletId)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet is null)
                throw new ArgumentException("Card does not exist");
            return wallet.Card;
            
        }

        public async Task<Wallet?> GetByIdAsync(Guid walletId)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet is null)
                throw new ArgumentException("Wallet does not exist");
            return wallet;
        }

        public async Task<Wallet?> GetByUserIdAsync(Guid userId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            return wallet;
        }

        public async Task<bool> SetWalletLockStatusAsync(Guid walletId, bool isLocked)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet is null)
                return false;

            if (isLocked)
                wallet.LockWallet();

            else
                wallet.UnlockWallet();

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Guid walletId, CreateWalletDto walletObject)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet is null)
                return false;

            _context.Entry(wallet).CurrentValues.SetValues(walletObject);
            return true;
        }

        public async Task<bool> WithdrawAsync(Guid walletId, decimal amount)
        {
            var wallet = await _context.Wallets.FindAsync(walletId);
            if (wallet is null)
                return false;
            wallet.Withdraw(amount);
            return true;
        }

        public async Task<List<Transaction>> GetTransactionsAsync(Guid walletId)
        {
            return await _context.Transactions
                .Where(w => w.WalletId == walletId)
                .ToListAsync();
        }
    }
}
