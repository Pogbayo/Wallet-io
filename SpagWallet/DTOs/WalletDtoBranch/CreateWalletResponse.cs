
namespace SpagWallet.Application.DTOs.WalletDtoBranch
{
    public class CreateWalletResponse
    {
        public Guid WalletId { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }

}
