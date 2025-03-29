using SpagWallet.Domain.Entities;

namespace Application.Interfaces.ServiceInterfaces
{
    public interface IKycService
    {
        Task<Kyc?> GetKycByIdAsync(Guid kycId);
        Task<Kyc?> GetKycByUserIdAsync(Guid userId);
        Task<bool> SubmitKycAsync(Kyc kycData);
        Task<bool> VerifyKycAsync(Guid userId, bool isVerified);
        Task<IEnumerable<Kyc>> GetVerifiedKycsAsync();
        Task<IEnumerable<Kyc>> GetUnverifiedKycsAsync();
    }
}
