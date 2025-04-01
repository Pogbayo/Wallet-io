using SpagWallet.Application.DTOs.BankAccountDtoBranch;
using SpagWallet.Application.DTOs.TransferDtoBranch;
using SpagWallet.Domain.Entities;

namespace Application.Interfaces.ServiceInterfaces
{
    public interface IBankAccountService
    {
        Task<BankAccountDto?> CreateAsync(BankAccount CreateBankAccountDto);
        Task<BankAccountDto?> GetBankAccountByIdAsync(Guid bankAccountId);
        Task<BankAccountDto?> GetBankAccountByUserIdAsync(Guid userId);
        Task<bool> UpdateBalanceAsync(Guid bankAccountId, decimal amount);
        Task<BankAccountDto?> UpdateAccountType(Guid bankAccountId, UpdateBankAccountDto updateDto);
        Task<bool> DeleteBankAccountAsync(Guid bankaccountid);
        Task<bool> AddFundsAsync(Guid bankaccountid, decimal amount);
        Task<bool> WithdrawFundsAsync(Guid bankaccountid, decimal amount);
        //Task<IEnumerable<GetTransactionDetailsDto>> GetTransactionsAsync(Guid bankaccountid);
        Task<bool> TransferFundsAsync(Guid senderbankaccountId, Guid receiverbankaccountId, decimal amount);
    }
}
