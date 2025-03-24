using SpagWallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpagWallet.Application.Interfaces
{
    public interface IKycRepository
    {
        Task<Kyc?> GetKycByUserIdAsync(Guid userId);
        Task<bool> UpdateKycStatusAsync(Guid userId, bool isVerified);
        Task AddKycAsync(Kyc kycData);
        Task DeleteKycAsync(Guid userId);
    }
}
