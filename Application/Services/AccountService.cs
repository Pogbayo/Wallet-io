
using Application.Interfaces.RepoInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Entities;
using SpagWallet.Application.DTOs.BankAccountDtoBranch;
using SpagWallet.Domain.Entities;

namespace Application.Services
{
    public class AccountService : IBankAccountService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ICurrentUserService _currentUSerService;
        public Guid currentuserid { get; } 

        public AccountService(
            IAuditLogRepository auditLogRepository, 
            IBankAccountRepository bankAccountRepository,
            ICurrentUserService currentUSerService
            )
        {
            _auditLogRepository = auditLogRepository;
            _bankAccountRepository = bankAccountRepository;
            _currentUSerService = currentUSerService;
            this.currentuserid = _currentUSerService.GetUserId();
        }

        public async Task<BankAccountDto?> GetBankAccountByIdAsync(Guid bankAccountId)
        {
            if (bankAccountId == Guid.Empty)
                throw new ArgumentException("Invalid bankAccountid");

            var bankAccountRecord = await _bankAccountRepository.GetByIdAsync(bankAccountId);

            if (bankAccountRecord == null)
                throw new ArgumentException("Bank account does not exist");

            var auditLog = new AuditLog(
                 action: "Fetching bank account details",
                 performedBy: currentuserid,
                 details: $"Account with {bankAccountId} got fetched"
            );

            await _auditLogRepository.AddLogAsync(auditLog);

            return new BankAccountDto
            {
                Id = bankAccountRecord.Id,
                AccountNumber = bankAccountRecord.AccountNumber,
                BankName = bankAccountRecord.BankName,
                AccountType = bankAccountRecord.AccountType,
                Balance = bankAccountRecord.Balance,
                CreatedAt = bankAccountRecord.CreatedAt
            };
            
        }

        public async Task<BankAccountDto?> GetBankAccountByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Invalid bankAccountid");

            var bankAccountRecord = await _bankAccountRepository.GetByUserIdAsync(userId);

            if (bankAccountRecord == null)
                throw new ArgumentException("Bank account does not exist");

            var auditLog = new AuditLog(
                 action: "Fetching bank account details",
                 performedBy: currentuserid,
                 details: $"Account with {bankAccountRecord.UserId} got fetched"
            );

            await _auditLogRepository.AddLogAsync(auditLog);

            return new BankAccountDto
            {
                Id = bankAccountRecord.Id,
                AccountNumber = bankAccountRecord.AccountNumber,
                BankName = bankAccountRecord.BankName,
                AccountType = bankAccountRecord.AccountType,
                Balance = bankAccountRecord.Balance,
                CreatedAt = bankAccountRecord.CreatedAt
            };

        }

        public async Task<bool> UpdateBalanceAsync(Guid bankAccountId, decimal amount)
        {
            if (bankAccountId == Guid.Empty)
                throw new ArgumentException("Invalid bankAccountid");

            var bankAccountRecord = await _bankAccountRepository.GetByIdAsync(bankAccountId);

            if (bankAccountRecord == null)
                throw new ArgumentException("Bank account does not exist");

            if(amount<0)
            {
                bankAccountRecord.Withdraw(amount);
            }
            else if(amount>0)
            {
                bankAccountRecord.Credit(amount);
            }

            await _bankAccountRepository.UpdateBalanceAsync(bankAccountRecord.Id, amount);

            var auditLog = new AuditLog(
                     action: "Fetching bank account details",
                     performedBy: currentuserid,
                     details: $"Account with {bankAccountId} got fetched"
                );

            await _auditLogRepository.AddLogAsync(auditLog);

            return true;
        }

        public async Task<BankAccountDto?> UpdateAccountType(Guid bankAccountId, UpdateBankAccountDto updateDto)
        {
            if (bankAccountId == Guid.Empty)
                throw new ArgumentException("Invalid bankAccountid");

            var bankAccountRecord = await _bankAccountRepository.GetByIdAsync(bankAccountId);

            if (bankAccountRecord == null)
                throw new ArgumentException("Bank account does not exist");

            bankAccountRecord.AccountType = updateDto.AccountType ?? bankAccountRecord.AccountType;

            var auditLog = new AuditLog(
                     action: "Updating Account type",
                     performedBy: currentuserid,
                     details: $"Account with {bankAccountId} got updated"
                );

            await _auditLogRepository.AddLogAsync(auditLog);

            return new BankAccountDto
            {
                Id = bankAccountRecord.Id,
                AccountNumber = bankAccountRecord.AccountNumber,
                BankName = bankAccountRecord.BankName,
                AccountType = bankAccountRecord.AccountType,
                Balance = bankAccountRecord.Balance,
                CreatedAt = bankAccountRecord.CreatedAt
            };
        }

        public async Task<bool> DeleteBankAccountAsync(Guid bankaccountid)
        {
            if (bankaccountid == Guid.Empty)
                throw new ArgumentException("Invalid bankAccountid");

            var bankAccountRecord = await _bankAccountRepository.GetByIdAsync(bankaccountid);

            if (bankAccountRecord == null)
                throw new ArgumentException("Bank account does not exist");

            var auditLog = new AuditLog(
                 action: "Deleting bank account.",
                 performedBy: currentuserid,
                 details: $"Account with {bankaccountid} got deleted"
            );

            await _auditLogRepository.AddLogAsync(auditLog);

            return bankAccountRecord != null;
        }

        public async Task<BankAccountDto?> CreateAsync(BankAccount bankAccountdata)
        {
             if (bankAccountdata == null)
                throw new ArgumentException("Bank details incomplete.");

            if (bankAccountdata.User == null)
            {
                throw new ArgumentException("A user object must be associated with a bank account instance");
            }

            await _bankAccountRepository.CreateAsync(bankAccountdata);

            var auditLog = new AuditLog(
                action: "Creating bank account.",
                performedBy: currentuserid,
                details: $"Account with {bankAccountdata.UserId} got created"
           );

            await _auditLogRepository.AddLogAsync(auditLog);

            return new BankAccountDto
            {
                Id = bankAccountdata.Id,
                AccountNumber = bankAccountdata.AccountNumber,
                BankName = bankAccountdata.BankName,
                AccountType = bankAccountdata.AccountType,
                Balance = bankAccountdata.Balance,
                CreatedAt = bankAccountdata.CreatedAt
            };
        }

        public async Task<bool> AddFundsAsync(Guid bankaccountid, decimal amount)
        {
            var bankAccount = await _bankAccountRepository.GetByIdAsync(bankaccountid);
            if (bankAccount == null) return false;

            bool success = bankAccount.Credit(amount);
            if (!success) return false;

            var auditLog = new AuditLog(
                 action: "Funding bank account",
                 performedBy: currentuserid,
                 details: $"Bank account {bankaccountid} funded with {amount}"
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return await _bankAccountRepository.UpdateBalanceAsync(bankaccountid, amount);
        }

        public async Task<bool> WithdrawFundsAsync(Guid bankaccountid, decimal amount)
        {
            var bankAccount = await _bankAccountRepository.GetByIdAsync(bankaccountid);
            if (bankAccount == null) return false;

            bool success = bankAccount.Withdraw(amount);
            if (!success) return false;

            var auditLog = new AuditLog(
                  action: "withdrawing from wallet",
                  performedBy: currentuserid,
                  details: $"{amount} withdrawn from {bankAccount.Id}."
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return await _bankAccountRepository.UpdateBalanceAsync(bankaccountid, amount);
        }

        public async Task<bool> TransferFundsAsync(Guid senderbankaccountid, Guid receiverbankaccountId, decimal amount)
        {
            var senderWallet = await _bankAccountRepository.GetByIdAsync(senderbankaccountid);
            var receiverWallet = await _bankAccountRepository.GetByIdAsync(receiverbankaccountId);

            if (senderWallet == null || receiverWallet == null) return false;

            senderWallet.Withdraw(amount);
            receiverWallet.Credit(amount);

            var auditLog = new AuditLog(
              action: "Transferring funds",
              performedBy: senderWallet.UserId,
              details: $"{senderWallet.UserId} sent {amount} to {receiverWallet.UserId}."
            );
            await _auditLogRepository.AddLogAsync(auditLog);

            return true;
        }

        //public async Task<IEnumerable<GetTransactionDetailsDto>> GetTransactionsAsync(Guid walletId)
        //{
        //    var list = await _bankAccountRepository.(walletId);
        //    if (list == null)
        //    {
        //        throw new ArgumentException("List is empty");
        //    }

        //    var transactionsDto = list.Select(transaction => new GetTransactionDetailsDto
        //    {
        //        Id = transaction.Id,
        //        WalletId = transaction.WalletId,
        //        BankAccountId = transaction.BankAccountId,
        //        Amount = transaction.Amount,
        //        Reference = transaction.Reference,
        //        Type = transaction.Type,
        //        Status = transaction.Status,
        //        Source = transaction.Source
        //    });

        //    var auditLog = new AuditLog(
        //     action: "Fetching wallet transactions",
        //     performedBy: currentuserId,
        //     details: $"Wallet transactions fetched."
        //     );
        //    await _auditLogRepository.AddLogAsync(auditLog);

        //    return transactionsDto;
        //}
        //row new NotImplementedException();
        //}
    }
}
