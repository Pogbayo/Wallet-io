using API.Common;
using Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using SpagWallet.Application.DTOs.KycDtoBranch;
using SpagWallet.Domain.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KycController : BaseController
    {
        private readonly IKycService _identityAuth;
        public KycController(IKycService identityAuth)
        {
            _identityAuth = identityAuth;
        }

        [HttpGet("get-kyc-by/{kycId}")]
        public async Task<ActionResult<ApiResponse<GetKycDto>>> GetKycById(Guid kycId)
        {
            var kycRecord = await _identityAuth.GetKycByIdAsync(kycId);
            if (kycRecord == null)
            {
                return NotFoundResponse<GetKycDto>(new List<string> { "Error getting kyc record" }, "Kyc record not found");
            }
            return Success(kycRecord, "Kyc record fetched successfully");
        }

        [HttpGet("get-kyc-by-userid/{userId}")]
        public async Task<ActionResult<ApiResponse<GetKycDto>>> GetKycByUserId(Guid userId)
        {
            var kycRecord = await _identityAuth.GetKycByUserIdAsync(userId);
            if (kycRecord == null)
            {
                return NotFoundResponse<GetKycDto>(new List<string> { "Error getting kyc record" }, "Kyc record not found");
            }
            return Success(kycRecord, "Kyc record fetched successfully");
        }

        [HttpGet("get-unverified-kyc-records")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Guid>>>> GetUnverifiedKycRecords()
        {
            var unverifiedKycs = await _identityAuth.GetUnverifiedKycsAsync();
            if (!unverifiedKycs.Any())
            {
                return NotFoundResponse<IEnumerable<Guid>>(new List<string> { "Error getting kyc record" }, "Kyc record not found");
            }
            return Success(unverifiedKycs, "Unveirifed kycs fetched successfully.");
        }

        [HttpGet("get-all-verified-kyc-records")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Guid>>>> GetVerifiedKycRecords()
        {
            var unverifiedKycs = await _identityAuth.GetVerifiedKycsAsync();
            if (!unverifiedKycs.Any())
            {
                return NotFoundResponse<IEnumerable<Guid>>(new List<string> { "Error getting kyc record" }, "Kyc record not found");
            }
            return Success(unverifiedKycs, "Verified kycs fetched successfully.");
        }


        [HttpPost("submit-kyc-record")]
        public async Task<ActionResult<ApiResponse<bool>>> SubmitKyc([FromBody] Kyc kycdata)
        {
            bool success = await _identityAuth.SubmitKycAsync(kycdata);
            if (!success)
            {
                return Failure<bool>(new List<string> { "Error submitting kyc record" }, "Kyc record not submitted");

            }
            return Success(success, "Kyc submission successful.");
        }

        [HttpPost("verify-kyc/{userId}")]
        public async Task<ActionResult<ApiResponse<bool>>> VerifyKyc(Guid userId,[FromBody] bool isVefiried)
        {
            bool success = await _identityAuth.VerifyKycAsync(userId,isVefiried);
            if (!success)
            {
                return Failure<bool>(new List<string> { "Error verifying kyc record" }, "Kyc record not verified");
            }
            return Success(success, "Kyc verification successful.");
        }

        [HttpDelete("delete-kyc/{kycId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteKyc(Guid kycId)
        {
            var success = await _identityAuth.DeleteKycAsync(kycId);
            if (!success)
            {
                return Failure<bool>(new List<string> { "Error deleting kyc record" }, "Kyc record not deleted");
            }
            return Success(success, "Kyc record deleted successfully.");
        }

        [HttpGet("check-kyc-verification-status/{kycId}")]
        public async Task<ActionResult<ApiResponse<bool>>> IsKycVerified(Guid kycId)
        {
            bool success = await _identityAuth.IsKycVerifiedAsync(kycId);
            if (!success)
            {
                return Failure<bool>(new List<string> { "Error verifying kyc status" }, "Kyc status not verified");
            }
            return Success(success, "Kyc verification status checked successfully.");
        }
        
        [HttpGet("check-kyc-submission-status/{userId}")]
        public async Task<ActionResult<ApiResponse<bool>>> UserHasSubmittedKyc(Guid userId)
        {
            bool success = await _identityAuth.UserHasSubmittedKycAsync(userId);
            if (!success)
            {
                return Failure<bool>(new List<string> { "Error verifying kyc submission status" }, "Kyc submission status not verified");
            }
            return Success(success, "User has submitted kyc.");
        }

        [HttpGet("check-veirifcation-time/{kycId}")]
        public async Task<ActionResult<ApiResponse<DateTime?>>> CheckVerificationTime(Guid kycId)
        {
            var datetime = await _identityAuth.VerifiedAtAsync(kycId);
            return Success(datetime, $"Kyc was verified at {datetime}");
        }
    }
}
