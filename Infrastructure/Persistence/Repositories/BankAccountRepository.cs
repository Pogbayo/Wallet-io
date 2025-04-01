
using Application.Interfaces.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using SpagWallet.Application.DTOs.BankAccountDtoBranch;
using SpagWallet.Domain.Entities;
using SpagWallet.Infrastructure.Persistence.Data;

namespace SpagWallet.Infrastructure.Persistence.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<BankAccount> _bankAccountTable;

        public BankAccountRepository(AppDbContext context)
        {
            _context = context;
            _bankAccountTable = context.BankAccounts;
        }

        public async Task<BankAccount> CreateAsync(BankAccount bankAccountdata)
        {
            var existingBankAccount = await _bankAccountTable
                .FirstOrDefaultAsync(b => b.AccountNumber == bankAccountdata.AccountNumber);

            if (existingBankAccount != null)
                throw new ArgumentException("Bank account already exists.");

            await _bankAccountTable.AddAsync(bankAccountdata);

            return bankAccountdata;

        }

        public async Task<bool> DeleteAsync(Guid bankAccountId)
        {
            var account = await _bankAccountTable.FindAsync(bankAccountId);
            if (account is null)
                throw new ArgumentException("Account is null");

            _bankAccountTable.Remove(account);

            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Guid>> GetAllAsync()
        {
            var accountRecords = await _bankAccountTable
                .ToListAsync();
            return accountRecords.Select( account => account.Id).ToList();
        }

        public async Task<BankAccount> GetByIdAsync(Guid bankAccountId)
        {
            var accountRecord = await _bankAccountTable
                .FindAsync(bankAccountId);
            if (accountRecord ==  null)
            {
                throw new ArgumentException("Account does not exist");
            }

            return accountRecord;
        }

        public async Task<BankAccount> GetByUserIdAsync(Guid userId)
        {
            var accountRecord = await _bankAccountTable
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (accountRecord is null)
                throw new ArgumentException("Bank account does not exist");

            return accountRecord;
        }

        public async Task<BankAccount> GetByWalletIdAsync(Guid walletId)
        {
            var accountRecord = await _bankAccountTable
                .Include(a => a.Wallet)
                .FirstOrDefaultAsync(a => a.Wallet != null && a.Wallet.Id == walletId);

            if (accountRecord is null)
                throw new ArgumentException("Bank account does not exist");

            return accountRecord;
        }

        public async Task<BankAccount> UpdateAccountTypeAsync(Guid bankaccountId, UpdateBankAccountDto updateDto)
        {
            var accountRecord = await _bankAccountTable
                           .FindAsync(bankaccountId);

            if (accountRecord is null)
                throw new ArgumentException("Bank account does not exist");

            accountRecord.AccountType = updateDto.AccountType ?? accountRecord.AccountType;

            await _context.SaveChangesAsync();

            return accountRecord;
        }


        public async Task<bool> UpdateBalanceAsync(Guid bankAccountId, decimal amount)
        {
            var accountRecord = await _bankAccountTable
                .FindAsync(bankAccountId);

            if (accountRecord is null)
                throw new ArgumentException("Bank account does not exist");

            if (amount > 0)
                accountRecord.Credit(amount);
            else if (amount < 0)
                accountRecord.Withdraw(amount);

           int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> SaveAsync()
        {
          int result =  await _context.SaveChangesAsync();
          return result > 0;
        }

        public async Task<bool> AddFundsAsync(Guid bankaccountId, decimal amount)
        {
            var bankAccount = await GetByIdAsync(bankaccountId);
            if (bankAccount == null) return false;

            bankAccount.Credit(amount);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> WithdrawFundsAsync(Guid bankaccountId, decimal amount)
        {
            var bankAccount = await GetByIdAsync(bankaccountId);
            if (bankAccount == null || bankAccount.Balance < amount) return false;

            bankAccount.Withdraw(amount);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> TransferFundsAsync(Guid senderbankaccountid, Guid receiverbankaccountIid, decimal amount)
        {
            var senderbankaccount = await GetByIdAsync(senderbankaccountid);
            var receiverbankaccount = await GetByIdAsync(receiverbankaccountIid);

            if (senderbankaccount == null || receiverbankaccount == null || senderbankaccount.Balance < amount)
                return false;

            senderbankaccount.Withdraw(amount);
            receiverbankaccount.Credit(amount);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
