using SpagWallet.Application.DTOs.BankAccountDtoBranch;

namespace Application.Interfaces.ServiceInterfaces
{
    public interface IBankAccountService
    {
        Task<BankAccountDto?> GetBankAccountByIdAsync(Guid bankAccountId);
        Task<BankAccountDto?> GetBankAccountByUserIdAsync(Guid userId);
        Task<bool> UpdateBalanceAsync(Guid bankAccountId, decimal amount);
        Task<BankAccountDto> UpdateAccountType(Guid bankAccountId, UpdateBankAccountDto updateDto);
    }
}
