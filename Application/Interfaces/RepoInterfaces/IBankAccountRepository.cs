using SpagWallet.Application.DTOs.BankAccountDtoBranch;
using SpagWallet.Domain.Entities;

namespace Application.Interfaces.RepoInterfaces
{
   public interface IBankAccountRepository
    {
        Task<List<Guid>> GetAllAsync();
        Task<BankAccountDto> CreateAsync(BankAccount CreateBankAccountDto);
        Task<BankAccountDto?> GetByIdAsync(Guid bankAccountId);
        Task<BankAccountDto?> GetByUserIdAsync(Guid userId);
        Task<BankAccountDto?> GetByWalletIdAsync(Guid walletId);
        Task<bool> UpdateBalanceAsync(Guid bankAccountId, decimal amount);
        Task<BankAccountDto> UpdateAccountType(Guid bankaccountId, UpdateBankAccountDto updateDto);
    }
}
