using Application.Interfaces.RepoInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Entities;
using SpagWallet.Application.DTOs.KycDtoBranch;
using SpagWallet.Domain.Entities;

namespace Application.Services
{
    public class IdentityAuthService : IKycService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IKycRepository _kycRepository;
        private readonly ICurrentUserService _currentUserService;
        public Guid currentuserId { get; }
        public IdentityAuthService(
            IAuditLogRepository auditLogRepository,
            IKycRepository kycRepository,
            ICurrentUserService currentUserService
            )
        {
            _auditLogRepository = auditLogRepository;
            _kycRepository = kycRepository;
            _currentUserService = currentUserService;
            currentuserId = _currentUserService.GetUserId();
        }

        public async Task<GetKycDto?> GetKycByIdAsync(Guid kycId)
        {
            var KycRecord = await _kycRepository.GetByIdAsync(kycId);

            if (KycRecord is null)
                throw new ArgumentException("kyc record does not exist");

            //var kycOwner = await _kycRepository.GetByUserIdAsync(KycRecord.UserId);

            //if (kycOwner is null)
            //    throw new ArgumentException("kyc record does not exist");

            var auditLog = new AuditLog
             (
             action: "Finding Kyc record",
             performedBy: currentuserId,
             details: $"Wallet created for {KycRecord.FullName}."
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return new GetKycDto
            {
                Id = KycRecord.Id,
                UserId = KycRecord.UserId,
                FullName = KycRecord.FullName,
                IdentificationNumber = KycRecord.IdentificationNumber,
                IsVerified = KycRecord.IsVerified,
                CreatedAt = KycRecord.CreatedAt
            };
        }

        public async Task<GetKycDto?> GetKycByUserIdAsync(Guid userId)
        {
            var kycRecord = await _kycRepository.GetByUserIdAsync(userId);

            if (kycRecord is null)
                throw new ArgumentException("kyc record does not exist");

            var auditLog = new AuditLog
              (
             action: "FInding Kyc record",
             performedBy: currentuserId,
             details: $"Wallet created for {kycRecord.FullName}."
              );

            await _auditLogRepository.AddLogAsync(auditLog);

            return new GetKycDto
            {
                Id = kycRecord.Id,
                UserId = kycRecord.UserId,
                FullName = kycRecord.FullName,
                IdentificationNumber = kycRecord.IdentificationNumber,
                IsVerified = kycRecord.IsVerified,
                CreatedAt = kycRecord.CreatedAt
            };
        }

        public async Task<IEnumerable<Guid>> GetUnverifiedKycsAsync()
        {
            var list = await _kycRepository.GetVerifiedKycsAsync();

            if (!list.Any())
            {
                throw new ArgumentException("No unverified kyc");
            }
            IEnumerable<Guid> KycIds = list.Select(kyc => kyc.Id);

            var auditLog = new AuditLog
             (
            action: "Searching for unverified kyc records",
            performedBy: currentuserId,
            details: "Retrieved list of unverifed kyc records"
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return KycIds;
        }

        public async Task<IEnumerable<Guid>> GetVerifiedKycsAsync()
        {
            var list = await _kycRepository.GetVerifiedKycsAsync();

            if (!list.Any())
            {
                throw new ArgumentException("No verified kyc");
            }

            IEnumerable<Guid> KycIds = list.Select(kyc => kyc.Id);

            var auditLog = new AuditLog
             (
            action: "Searching for verified kyc records",
            performedBy: currentuserId,
            details: "Retrieved list of verified kyc records"
             );
            await _auditLogRepository.AddLogAsync(auditLog);

            return KycIds;
        }

        public async Task<bool> SubmitKycAsync(Kyc kycData)
        {
            if (kycData.UserId == Guid.Empty)
                throw new ArgumentException("User Id is required");

            if (string.IsNullOrEmpty(kycData.FullName))
                throw new ArgumentException("Fullname is required");
            
            if (kycData.IdentificationNumber == Guid.Empty)
                throw new ArgumentException("Identification number is required");
            

            var existingKycRecord = await _kycRepository.GetByIdAsync(kycData.Id);
            if (existingKycRecord is not null)
            {
                throw new ArgumentException("Kyc record already exists");
            }

            await _kycRepository.AddKycAsync(kycData);

            var auditLog = new AuditLog
           (
             action: "Kyc submission",
             performedBy: currentuserId,
             details: "Submitted kyc record"
           );

            await _auditLogRepository.AddLogAsync(auditLog);

            return true;
        }

        public async Task<bool> VerifyKycAsync(Guid userId, bool isVerified)
        {
            var kyc = await _kycRepository.GetByUserIdAsync(userId);

            if (kyc == null) return false;

            kyc.IsVerified = isVerified;
            kyc.VerifiedAt = isVerified ? DateTime.UtcNow : kyc.VerifiedAt;

            var auditLog = new AuditLog
             (
                action: "Kyc verification",
                performedBy: currentuserId,
                details: "Verifying kyc records"
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return true;
        }

        public async Task<bool> DeleteKycAsync(Guid kycId)
        {
            var kycRecord = await _kycRepository.GetByIdAsync(kycId);
            if (kycRecord == null)
               return false;

            bool isDeleted = await _kycRepository.DeleteKycAsync(kycId);
            if (!isDeleted)
            {
                throw new ArgumentException("Error deleting kyc record.");
            }

            var auditLog = new AuditLog
            (
               action: "Kyc removal",
               performedBy: currentuserId,
               details: "Removing kyc record"
            );

            await _auditLogRepository.AddLogAsync(auditLog);
            return isDeleted;
        }

        public async Task<bool> IsKycVerifiedAsync(Guid userId)
        {
            var kycRecord = await _kycRepository.GetByUserIdAsync(userId);
            if (kycRecord == null)
            {
                return false;
            }
            var isVerified = await _kycRepository.IsKycVerifiedAsync(userId);

            var auditLog = new AuditLog
            (
               action: "Kyc verification confirmation",
               performedBy: currentuserId,
               details: "Confirming verified kyc record"
            );

            await _auditLogRepository.AddLogAsync(auditLog);

            return isVerified;
        }

        public async Task<bool> UserHasSubmittedKycAsync(Guid userId)
        {
            bool success = await _kycRepository.UserHasSubmittedKycAsync(userId);
            if (!success)
            {
                return false;
            }

            var auditLog = new AuditLog
            (
               action: "Kyc confirmation",
               performedBy: currentuserId,
               details: "Confirming submitted kyc record"
            );
            return success;
        }

        public async Task<DateTime?> VerifiedAtAsync(Guid kycId)
        {
            var kycRecord = await _kycRepository.GetByIdAsync(kycId);
            if (kycRecord == null)
            {
                throw new ArgumentException("Kyc record with provided ID does not exist");
            }

            DateTime dateTime = kycRecord.CreatedAt;
            var auditLog = new AuditLog
            (
               action: "Kyc creation time",
               performedBy: currentuserId,
               details: "Checking kyc creation date"
            );
            return dateTime;
        }
    }
}
