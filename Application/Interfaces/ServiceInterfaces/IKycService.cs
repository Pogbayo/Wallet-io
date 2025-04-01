using SpagWallet.Application.DTOs.KycDtoBranch;
using SpagWallet.Domain.Entities;

namespace Application.Interfaces.ServiceInterfaces
{
    public interface IKycService
    {
        Task<GetKycDto?> GetKycByIdAsync(Guid kycId);
        Task<GetKycDto?> GetKycByUserIdAsync(Guid userId);
        Task<bool> SubmitKycAsync(Kyc kycData);
        Task<IEnumerable<Guid>> GetVerifiedKycsAsync();
        Task<IEnumerable<Guid>> GetUnverifiedKycsAsync();
        Task<bool> VerifyKycAsync(Guid userId, bool isVerified);
        Task<bool> DeleteKycAsync(Guid kycId);
        Task<bool> IsKycVerifiedAsync(Guid userId);
        Task<bool> UserHasSubmittedKycAsync(Guid userId);
        Task<DateTime?> VerifiedAtAsync(Guid kycId);
    }
}
