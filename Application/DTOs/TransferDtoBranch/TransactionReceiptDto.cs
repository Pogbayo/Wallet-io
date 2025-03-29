using SpagWallet.Domain.Enums.TransactionEnums;

namespace SpagWallet.Application.DTOs.TransferDtoBranch
{
    public class TransactionReceiptDto
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public TransactionSource Source { get; set; }
        public string? Reference { get; set; }
        public TransactionType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
