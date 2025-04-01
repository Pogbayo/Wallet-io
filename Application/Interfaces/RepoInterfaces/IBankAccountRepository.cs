using SpagWallet.Application.DTOs.BankAccountDtoBranch;
using SpagWallet.Domain.Entities;

namespace Application.Interfaces.RepoInterfaces
{
   public interface IBankAccountRepository
    {
        Task<List<Guid>> GetAllAsync();
        Task<BankAccount> CreateAsync(BankAccount CreateBankAccountDto);
        Task<BankAccount> GetByIdAsync(Guid bankAccountId);
        Task<BankAccount> GetByUserIdAsync(Guid userId);
        Task<BankAccount> GetByWalletIdAsync(Guid walletId);
        Task<bool> UpdateBalanceAsync(Guid bankAccountId, decimal amount);
        Task<BankAccount> UpdateAccountTypeAsync(Guid bankaccountId, UpdateBankAccountDto updateDto);
        Task<bool> DeleteAsync(Guid bankaccoutid);
        Task<bool> SaveAsync();
        Task<bool> AddFundsAsync(Guid bankaccountId, decimal amount);
        Task<bool> WithdrawFundsAsync(Guid bankaccountId, decimal amount);
        Task<bool> TransferFundsAsync(Guid senderbankaccountid, Guid receiverbankaccountIid, decimal amount);
    }
}

