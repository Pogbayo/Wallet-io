
namespace SpagWallet.Application.DTOs.WalletDtoBranch
{
   public class WalletDto
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; } 
        public string Currency { get; set; } = "NGN";
        public Guid? WalletNumber { get; set; }
        public string? WalletPinHash { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
