 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpagWallet.Domain.Entities
{
    public class Kyc
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public required virtual User User { get; init; }
        public string FullName { get; set; } = string.Empty;
        public string? IdentificationType { get; set; }
        public string IdentificationNumber { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        private Kyc() { }

        public Kyc(Guid userId, string fullName, DateTime dateOfBirth, string? idType, string idNumber)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full name is required.");
            if (dateOfBirth > DateTime.UtcNow) throw new ArgumentException("Date of birth cannot be in the future.");
            if (string.IsNullOrWhiteSpace(idNumber)) throw new ArgumentException("Identification number is required.");

            UserId = userId;
            FullName = fullName;
            IdentificationType = idType;
            IdentificationNumber = idNumber;
            CreatedAt = DateTime.UtcNow;
        }

        public void VerifyKyc()
        {
            IsVerified = true;
        }

        public void UpdateIdentification(string newIdType, string newIdNumber)
        {
            if (string.IsNullOrWhiteSpace(newIdType)) throw new ArgumentException("Identification type is required.");
            if (string.IsNullOrWhiteSpace(newIdNumber)) throw new ArgumentException("Identification number is required.");

            IdentificationType = newIdType;
            IdentificationNumber = newIdNumber;
        }

        public string GetKycSummary()
        {
            return $"KYC for {FullName}, Status: {(IsVerified ? "Verified" : "Pending")}, ID Type: {IdentificationType ?? "N/A"}";
        }

        public bool IsKycVerified()
        {
            return IsVerified;
        }
    }
}
