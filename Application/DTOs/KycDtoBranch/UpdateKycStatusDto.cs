
namespace SpagWallet.Application.DTOs.KycDtoBranch
{
    public class UpdateKycStatusDto
    {
        public Guid UserId { get; set; }
        public bool IsVerified { get; set; }
    }
}
