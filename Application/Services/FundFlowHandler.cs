using Application.Common.Models;
using Application.Interfaces.RepoInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Entities;
using SpagWallet.Application.DTOs.TransferDtoBranch;
using SpagWallet.Domain.Enums.TransactionEnums;
using Transaction = SpagWallet.Domain.Entities.Transaction;

namespace Application.Services
{
    public class FundFlowHandler : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ICurrentUserService _currentUserService;
        public Guid currentuserId { get; }
        public FundFlowHandler(
            ITransactionRepository transactionRepository,
            IAuditLogRepository auditLogRepository,
            ICurrentUserService currentUserService
            )
        {
            _auditLogRepository = auditLogRepository;
            _transactionRepository = transactionRepository;
            _currentUserService = currentUserService;
            currentuserId = currentUserService.GetUserId();
        }
        public async Task<IEnumerable<GetTransactionDetailsDto>> GetAllTransactionsAsync()
        {
            var transactionRecords = await _transactionRepository.GetAllAsync();
            if (!transactionRecords.Any())
                throw new ArgumentException("Transaction list is empty");

            var transactionsDto = transactionRecords.Select(transaction => new GetTransactionDetailsDto
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
               action: "Retrieved transactions",
               performedBy: currentuserId,
               details: "Transaction list was retrieved."
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return transactionsDto;
        }

        public async Task<GetTransactionDetailsDto?> GetTransactionByIdAsync(Guid transactionId)
        {
            var transactionRecord = await _transactionRepository.GetByIdAsync(transactionId);
            if (transactionRecord == null)
                throw new ArgumentException($"Transaction with id{transactionId} does not exist");

            var auditLog = new AuditLog(
               action: "Finding transaction by its id",
               performedBy: currentuserId,
               details: "Transaction retrieved."
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return new GetTransactionDetailsDto
            {  
                Id = transactionRecord.Id,
                WalletId = transactionRecord.WalletId,
                BankAccountId = transactionRecord.BankAccountId,
                Amount = transactionRecord.Amount,
                Reference = transactionRecord.Reference,
                Type = transactionRecord.Type,
                Status = transactionRecord.Status,
                Source = transactionRecord.Source
             };
        }

        public async Task<TransactionReceiptDto?> ProcessReceiptAsync(Transaction transaction)
        {
            if (transaction is null)
                throw new ArgumentException($"Transaction does not exist");

            if (transaction.Source == default)
                return null;

            TransactionReceiptDto? transactionReceipt = null;

            if (transaction.Source ==  TransactionSource.Wallet)
            {
               transactionReceipt = new TransactionReceiptDto
                {
                    Id = transaction.Id,
                    Amount = transaction.Amount,
                    WalletId = transaction.WalletId,
                    BankAccountId = null,
                    Source = transaction.Source,
                    Reference = transaction.Reference,
                    Type = transaction.Type,
                    CreatedAt = transaction.CreatedAt,
                    Status = transaction.Status,
                };
            }
            else if(transaction.Source == TransactionSource.BankAccount)
            {
                 transactionReceipt = new TransactionReceiptDto
                {
                    Id = transaction.Id,
                    Amount = transaction.Amount,
                    WalletId = null,
                    BankAccountId = transaction.BankAccountId,
                    Source = transaction.Source,
                    Reference = transaction.Reference,
                    Type = transaction.Type,
                    CreatedAt = transaction.CreatedAt,
                    Status = transaction.Status,
                };
            }

            await _transactionRepository.AddAsync(transaction);

            var auditLog = new AuditLog(
              action: "Transaction receipt generation",
              performedBy: currentuserId,
              details: "Transaction receipt was generated."
            );

            await _auditLogRepository.AddLogAsync(auditLog);

            if (transactionReceipt is null)
                throw new ArgumentException("Invalid transaction source.");

            return transactionReceipt;
        }

        public async Task<TransactionReceiptDto> AddAsync(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentException("Invalid transaction details.");
            }
            var transactionRecord = await _transactionRepository.AddAsync(transaction);
            
            var auditLog = new AuditLog(
              action: "Transaction added successfully",
              performedBy: currentuserId,
              details: "Transactiion added."
            );

            await _auditLogRepository.AddLogAsync(auditLog);

            await ProcessReceiptAsync(transactionRecord);

            return new TransactionReceiptDto
            {
                Id = transactionRecord.Id,
                WalletId = transactionRecord.WalletId,
                BankAccountId = transactionRecord.BankAccountId,
                Amount = transactionRecord.Amount,
                Reference = transactionRecord.Reference,
                Type = transactionRecord.Type,
                Status = transactionRecord.Status,
                Source = transactionRecord.Source
            }; 
        }
    }
}
