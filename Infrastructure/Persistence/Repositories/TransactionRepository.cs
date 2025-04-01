using Application.Interfaces.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using SpagWallet.Domain.Entities;
using SpagWallet.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpagWallet.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Transaction> _transactionTable;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
            _transactionTable = context.Transactions;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _transactionTable.ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(Guid transactionId)
        {
            return await _transactionTable.FindAsync(transactionId);
        }

        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            await _transactionTable.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> UpdateAsync(Transaction transaction)
        {
            _transactionTable.Update(transaction);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
