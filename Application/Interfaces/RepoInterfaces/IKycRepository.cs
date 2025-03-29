using SpagWallet.Domain.Entities;

namespace Application.Interfaces.RepoInterfaces
{
    public interface IKycRepository
    {
        Task<Kyc?> GetByIdAsync(Guid kycId);
        Task<Kyc?> GetByUserIdAsync(Guid userId);
        Task<bool> UserHasSubmittedKycAsync(Guid userId);
        Task<bool> IsKycVerifiedAsync(Guid userId);
        Task<IEnumerable<Kyc>> GetVerifiedKycsAsync();
        Task AddKycAsync(Kyc kycData);
        Task DeleteKycAsync(Guid kycId);
        Task<bool> VerifyKycAsync(Guid userId, bool isVerified);
        Task<DateTime?> VerifiedAtAsync(Guid kycId);
        Task<IEnumerable<Kyc>> GetUnverifiedKycsAsync();
        Task<Kyc?> SearchByFullNameAsync(string fullName);
    }
}