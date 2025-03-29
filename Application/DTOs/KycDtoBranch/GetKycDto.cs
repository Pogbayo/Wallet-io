
namespace SpagWallet.Application.DTOs.KycDtoBranch
{
    public class GetKycDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? IdentificationType { get; set; }
        public string IdentificationNumber { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
