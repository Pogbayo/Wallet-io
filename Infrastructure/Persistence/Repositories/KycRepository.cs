using Application.Interfaces.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using SpagWallet.Domain.Entities;
using SpagWallet.Infrastructure.Persistence.Data;


namespace SpagWallet.Infrastructure.Persistence.Repositories
{
    public class KycRepository : IKycRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Kyc> _kycTable;

        public KycRepository(AppDbContext context)
        {
            _context = context;
            _kycTable = context.Kycs;
        }

        public async Task<Kyc?> GetByIdAsync(Guid kycId)
        {
            return await _kycTable.FindAsync(kycId);
        }

        public async Task<Kyc?> GetByUserIdAsync(Guid userId)
        {
            return await _kycTable.FirstOrDefaultAsync(k => k.UserId == userId);
        }

        public async Task<bool> UserHasSubmittedKycAsync(Guid userId)
        {
            return await _kycTable.AnyAsync(k => k.UserId == userId);
        }

        public async Task<bool> IsKycVerifiedAsync(Guid userId)
        {
            return await _kycTable.AnyAsync(k => k.UserId == userId && k.IsVerified);
        }

        public async Task<IEnumerable<Kyc>> GetVerifiedKycsAsync()
        {
            return await _kycTable.Where(k => k.IsVerified).ToListAsync();
        }

        public async Task AddKycAsync(Kyc kycData)
        {
            await _kycTable.AddAsync(kycData);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteKycAsync(Guid kycId)
        {
            var kyc = await _kycTable.FindAsync(kycId);
            if (kyc != null)
            {
                _kycTable.Remove(kyc);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> VerifyKycAsync(Guid userId, bool isVerified)
        {
            var kyc = await _kycTable.FirstOrDefaultAsync(k => k.UserId == userId);
            if (kyc == null) return false;

            kyc.IsVerified = isVerified;
            kyc.VerifiedAt = isVerified ? DateTime.UtcNow : kyc.VerifiedAt;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<DateTime?> VerifiedAtAsync(Guid kycId)
        {
            var kyc = await _kycTable.FindAsync(kycId);
            return kyc?.VerifiedAt;
        }

        public async Task<IEnumerable<Kyc>> GetUnverifiedKycsAsync()
        {
            return await _kycTable.Where(k => !k.IsVerified).ToListAsync();
        }

        public async Task<Kyc?> SearchByFullNameAsync(string fullName)
        {
            return await _kycTable.FirstOrDefaultAsync(k => k.FullName == fullName);
        }
    }
}
