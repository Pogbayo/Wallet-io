using Application.Interfaces.RepoInterfaces;
using Domain.Entities;
using SpagWallet.Infrastructure.Persistence.Data;

namespace Infrastructure.Persistence.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddLogAsync(AuditLog log)
        {
            await _context.AuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
