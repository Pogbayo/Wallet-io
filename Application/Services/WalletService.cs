using Application.Interfaces.RepoInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Entities;
using SpagWallet.Application.DTOs.TransferDtoBranch;
using SpagWallet.Application.DTOs.WalletDtoBranch;
using SpagWallet.Domain.Entities;
using System.Transactions;

namespace Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ICurrentUserService _currentUserService;
        public Guid currentuserId { get; }

        public WalletService(
            IWalletRepository walletRepository,
            IUserRepository userRepository,
            IBankAccountRepository bankAccountRepository,
            IAuditLogRepository auditLogRepository,
            ICurrentUserService currentUserService
            )
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _bankAccountRepository = bankAccountRepository;
            _auditLogRepository = auditLogRepository;
            _currentUserService = currentUserService;
            currentuserId = _currentUserService.GetUserId();
        }

        public async Task<WalletDto?> CreateWalletAsync(CreateWalletDto walletdata)
        {
            var user = await _userRepository.GetUserByIdAsync(walletdata.UserId);
            if (user == null)
                throw new ArgumentException("User not found.");

            if (walletdata is null)
                throw new ArgumentException("Incorrect wallet details");

            var wallet = await _walletRepository.GetByIdAsync(walletdata.Id);
            if (wallet != null)
                throw new ArgumentException("Wallet already exists");

            var bankAccount = await _bankAccountRepository.GetByUserIdAsync(walletdata.UserId);
            if (bankAccount == null)
                throw new ArgumentException("Bank account not found.");

            var newWallet = new Wallet(walletdata.UserId, walletdata.WalletPin);

            await _walletRepository.CreateAsync(newWallet);

            var auditLog = new AuditLog(
              action: "Wallet creation",
              performedBy: currentuserId,
              details: $"Wallet created for {user.Id}."
            );

            await _auditLogRepository.AddLogAsync(auditLog);

            return new WalletDto
            { 
                Id = newWallet.Id,
                Balance = newWallet.Balance,
                Currency = newWallet.Currency,
                WalletNumber = newWallet.WalletNumber,
                WalletPinHash = newWallet.WalletPinHash,
                IsLocked = newWallet.IsLocked,
                CreatedAt = newWallet.CreatedAt
            };
        }

        public async Task<bool> DeleteWalletAsync(Guid walletId)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet is null)
                throw new ArgumentException($"Wallet with {walletId} does not exist.");

            bool result = await _walletRepository.DeleteAsync(walletId);

            var auditLog = new AuditLog(
                action: "Wallet Deletion",
                performedBy: currentuserId,
                details: $"Wallet {walletId} deleted by {currentuserId}."
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return result;
        }

        public async Task<WalletDto?> GetWalletByIdAsync(Guid walletId)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet is null)
                throw new ArgumentException($"Wallet with {walletId} does not exist.");

            var auditLog = new AuditLog(
             action: "Finding Wallet by Id",
             performedBy: currentuserId,
             details: $"Wallet fetched for {wallet.UserId}."
           );

            await _auditLogRepository.AddLogAsync(auditLog);

            return new WalletDto
            {
                Id = wallet.Id,
                Balance = wallet.Balance,
                Currency = wallet.Currency,
                WalletNumber = wallet.WalletNumber,
                WalletPinHash = wallet.WalletPinHash,
                IsLocked = wallet.IsLocked,
                CreatedAt = wallet.CreatedAt
            };
        }

        public async Task<WalletDto?> GetWalletByUserIdAsync(Guid userId)
        {
            var wallet = await _walletRepository.GetByUserIdAsync(userId);
            if (wallet is null)
                throw new ArgumentException($"Wallet with {userId} does not exist.");

            var auditLog = new AuditLog(
             action: "Finding Wallet by userID",
             performedBy: currentuserId,
             details: $"Wallet fetched for {wallet.UserId}."
           );

            await _auditLogRepository.AddLogAsync(auditLog);

            return new WalletDto
            {
                Id = wallet.Id,
                Balance = wallet.Balance,
                Currency = wallet.Currency,
                WalletNumber = wallet.WalletNumber,
                WalletPinHash = wallet.WalletPinHash,
                IsLocked = wallet.IsLocked,
                CreatedAt = wallet.CreatedAt
            };
        }

        public async Task<bool> AddFundsAsync(Guid walletId, decimal amount)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null) return false;

            bool success = wallet.Credit(amount);
            if (!success) return false;

            var auditLog = new AuditLog(
            action: "Funding wallet",
            performedBy: currentuserId,
            details: $"Wallet funded for {wallet.UserId}."
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return await _walletRepository.UpdateAsync(wallet);
        }

        public async Task<bool> WithdrawFundsAsync(Guid walletId, decimal amount)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null) return false;

            bool success = wallet.Withdraw(amount);
            if (!success) return false;

            var auditLog = new AuditLog(
            action: "withdrawing from wallet",
            performedBy: currentuserId,
            details: $"{amount} withdrawn from {wallet.Id}."
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return await _walletRepository.UpdateAsync(wallet);
        }

        public async Task<bool> TransferFundsAsync(Guid senderWalletId, Guid receiverWalletId, decimal amount)
        {
            var senderWallet = await _walletRepository.GetByIdAsync(senderWalletId);
            var receiverWallet = await _walletRepository.GetByIdAsync(receiverWalletId);

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

        public async Task<IEnumerable<GetTransactionDetailsDto>> GetTransactionsAsync(Guid walletId)
        {
            var list = await _walletRepository.GetTransactionsAsync(walletId);
            if (list == null)
            {
                throw new ArgumentException("List is empty");
            }

            var transactionsDto = list.Select(transaction => new GetTransactionDetailsDto
            {
                Id = transaction.Id,
                WalletId = transaction.WalletId,
                BankAccountId = transaction.BankAccountId,
                Amount = transaction.Amount,
                Reference = transaction.Reference,
                Type = transaction.Type,
                Status = transaction.Status,
                Source = transaction.Source
            });

            var auditLog = new AuditLog(
             action: "Fetching wallet transactions",
             performedBy: currentuserId,
             details: $"Wallet transactions fetched."
             );
            await _auditLogRepository.AddLogAsync(auditLog);

            return transactionsDto;
        }

        public async Task<IEnumerable<WalletDto?>?> GetWallets()
        {
            var walletList = await _walletRepository.GetAllWalletsAsync();
            if (!walletList.Any())
                return null;

            var walletsDto = walletList.Select(wallet => new WalletDto
            {
                Id = wallet.Id,
                Balance = wallet.Balance,
                Currency = wallet.Currency,
                WalletNumber = wallet.WalletNumber,
                WalletPinHash = wallet.WalletPinHash,
                IsLocked = wallet.IsLocked,
                CreatedAt = wallet.CreatedAt
            });

            var auditLog = new AuditLog(
            action: "Fetching all wallets.",
            performedBy: currentuserId,
            details: $"All wallets fetched."
            );
            await _auditLogRepository.AddLogAsync(auditLog);

            return walletsDto;
        }

       
    }
}

