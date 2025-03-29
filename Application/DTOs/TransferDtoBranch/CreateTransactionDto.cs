using SpagWallet.Domain.Enums.TransactionEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpagWallet.Application.DTOs.TransferDtoBranch
{
    public class CreateTransactionDto
    {
        public  Guid? WalletId { get; set; }
        public Guid? BankAccountId { get; set; }
        public decimal Amount { get; set; }  
        public required string Reference { get; set; } 
        public required TransactionType Type { get; set; }
        public TransactionSource Source { get; set; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}
