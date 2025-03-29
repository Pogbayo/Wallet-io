
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

        public async Task<BankAccountDto> CreateAsync(BankAccount CreateBankAccountDto)
        {
            if (CreateBankAccountDto == null)
                throw new ArgumentException("Bank details incomplete.");

            var existingBankAccount = await _bankAccountTable
                .FirstOrDefaultAsync(b => b.AccountNumber == CreateBankAccountDto.AccountNumber);

            if (existingBankAccount != null)
                throw new ArgumentException("Bank account already exists.");

            return new BankAccountDto
            {
                Id = CreateBankAccountDto.Id,
                AccountNumber = CreateBankAccountDto.AccountNumber,
                BankName = CreateBankAccountDto.BankName,
                AccountType = CreateBankAccountDto.AccountType,
                Balance = 0m,
                CreatedAt = DateTime.UtcNow,
            };

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

            if (accountRecords.Count == 0)
                throw new ArgumentException("Account list empty");

            return accountRecords.Select( account => account.Id).ToList();
        }

        public async Task<BankAccountDto?> GetByIdAsync(Guid bankAccountId)
        {
            var accountRecord = await _bankAccountTable
                .FindAsync(bankAccountId);

            if (accountRecord is null)
                return null;

            return new BankAccountDto
            {
                Id = accountRecord.Id,
                AccountNumber = accountRecord.AccountNumber,
                BankName = accountRecord.BankName,
                AccountType = accountRecord.AccountType,
                Balance = accountRecord.Balance,
                CreatedAt = accountRecord.CreatedAt,
            };
        }

        public async Task<BankAccountDto?> GetByUserIdAsync(Guid userId)
        {
            var accountRecord = await _bankAccountTable
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (accountRecord is null)
                throw new ArgumentException("Bank account does not exist");

            return new BankAccountDto
            {
                Id = accountRecord.Id,
                AccountNumber = accountRecord.AccountNumber,
                BankName = accountRecord.BankName,
                AccountType = accountRecord.AccountType,
                Balance = accountRecord.Balance,
                CreatedAt = accountRecord.CreatedAt,
            };
        }

        public async Task<BankAccountDto?> GetByWalletIdAsync(Guid walletId)
        {
            var accountRecord = await _bankAccountTable
                .Include(a => a.Wallet)
                .FirstOrDefaultAsync(a => a.Wallet != null && a.Wallet.Id == walletId);

            if (accountRecord is null)
                throw new ArgumentException("Bank account does not exist");

            return new BankAccountDto
            {
                Id = accountRecord.Id,
                AccountNumber = accountRecord.AccountNumber,
                BankName = accountRecord.BankName,
                AccountType = accountRecord.AccountType,
                Balance = accountRecord.Balance,
                CreatedAt = accountRecord.CreatedAt,
            };
        }

        public async Task<BankAccountDto> UpdateAccountType(Guid bankaccountId, UpdateBankAccountDto updateDto)
        {
            var accountRecord = await _bankAccountTable
                .FindAsync(bankaccountId);

            if (accountRecord is null)
                throw new ArgumentException("Bank account does not exist");

            accountRecord.AccountType = updateDto.AccountType ?? accountRecord.AccountType;

            await _context.SaveChangesAsync();

            return new BankAccountDto
            {
                Id = accountRecord.Id,
                AccountNumber = accountRecord.AccountNumber,
                BankName = accountRecord.BankName,
                AccountType = accountRecord.AccountType,
                Balance = accountRecord.Balance,
                CreatedAt = accountRecord.CreatedAt,
            };

        }
        //public async Task<BankAccountDto> UpdateAccountType(Guid bankaccountId, UpdateBankAccountDto updateDto)
        //{
        //    var accountRecord = await _bankAccountTable
        //        .FindAsync(bankaccountId);

        //    if (accountRecord is null)
        //        throw new ArgumentException("Bank account does not exist");


        //    if (updateDto.AccountType.HasValue)
        //    {
        //        accountRecord.AccountType = updateDto.AccountType.Value;
        //    }
        //    else
        //    {
        //        throw new ArgumentException("AccountType cannot be null");
        //    }

        //    return new BankAccountDto
        //    {
        //        Id = accountRecord.Id,
        //        AccountNumber = accountRecord.AccountNumber,
        //        BankName = accountRecord.BankName,
        //        AccountType = accountRecord.AccountType,
        //        Balance = accountRecord.Balance,
        //        CreatedAt = accountRecord.CreatedAt,
        //    };

        //}

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
    }
}
