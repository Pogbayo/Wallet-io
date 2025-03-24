
using Microsoft.EntityFrameworkCore;
using SpagWallet.Application.DTOs.TransferDtoBranch;
using SpagWallet.Application.Interfaces;
using SpagWallet.Domain.Entities;
using SpagWallet.Domain.Enums.TransactionEnums;
using SpagWallet.Infrastructure.Persistence.Data;

namespace SpagWallet.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;
        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionReceiptDto?> GenerateTransactionReceiptAsync(CreateTransactionDto transactionDto)
        {
            if (transactionDto == null)
                throw new ArgumentException("Transaction details cannot be null.");

            Wallet? wallet = null;
            BankAccount? bankAccount = null;


            if (transactionDto.WalletId != Guid.Empty)
            {
                wallet = await _context.Wallets
                    .Include(w => w.Transactions)
                    .FirstOrDefaultAsync(w => w.Id == transactionDto.WalletId);

                if (wallet == null)
                    throw new ArgumentException("Wallet not found.");

                transactionDto.Source = TransactionSource.Wallet;
            }
            else if (transactionDto.BankAccountId != Guid.Empty)
            {
                bankAccount = await _context.BankAccounts
                    .Include(b => b.Transactions)
                    .FirstOrDefaultAsync(b => b.Id == transactionDto.BankAccountId);

                if (bankAccount == null)
                    throw new ArgumentException("Bank account not found.");

                transactionDto.Source = TransactionSource.BankAccount;
            }
            else
            {
                throw new ArgumentException("Transaction must be linked to either a Wallet or a BankAccount.");
            }


            var transaction = new Transaction
            {
                Amount = transactionDto.Amount,
                Source = transactionDto.Source,
                Reference = transactionDto.Reference,
                Type = transactionDto.Type,
                CreatedAt = DateTime.UtcNow,
            };


            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return new TransactionReceiptDto
            {
                TransactionId = transaction.Id,
                Amount = transaction.Amount,
                Source = transaction.Source,
                Reference = transaction.Reference,
                Type = transaction.Type,
                CreatedAt = transaction.CreatedAt,
                Status = transaction.Status
            };

        }


        public async Task<Transaction?> GetTransactionByIdAsync(Guid transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction is null)
                throw new ArgumentException($"Transaction with {transactionId} does not exist.");
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task UpdateTransactionStatusAsync(Guid transactionId, TransactionStatus status)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction is null)
                throw new ArgumentException($"Transaction with {transactionId} does not exist.");

            if (transaction.Status != status)
            {
                transaction.Status = status;
                await _context.SaveChangesAsync(); 
            }
        }
    }
}
