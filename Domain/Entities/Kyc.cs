

namespace SpagWallet.Domain.Entities
{
    public class Kyc
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public required virtual User User { get; init; }
        public string FullName { get; set; } = string.Empty;
        public Guid IdentificationNumber { get; set; } = Guid.NewGuid();
        public bool IsVerified { get; set; } = false;
        public DateTime VerifiedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        private Kyc() { }

        public Kyc(Guid userId, string fullName, DateTime dateOfBirth, string? idType, string idNumber)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full name is required.");
            if (dateOfBirth > DateTime.UtcNow) throw new ArgumentException("Date of birth cannot be in the future.");
            if (string.IsNullOrWhiteSpace(idNumber)) throw new ArgumentException("Identification number is required.");

            UserId = userId;
            FullName = fullName;
            CreatedAt = DateTime.UtcNow;
        }

        public void VerifyKyc()
        {
            IsVerified = true;
        }

        public void UpdateIdentification( Guid newIdNumber)
        {
            if (newIdNumber == Guid.Empty) throw new ArgumentException("Identification number is required.");

            IdentificationNumber = newIdNumber;
        }

        public string GetKycSummary()
        {
            return $"KYC for {FullName}, Status: {(IsVerified ? "Verified" : "Pending")}";
        }

        public bool IsKycVerified()
        {
            return IsVerified;
        }
    }
}
