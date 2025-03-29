using SpagWallet.Application.DTOs.TransferDtoBranch;


namespace SpagWallet.Application.DTOs.BankAccountDtoBranch
{
    public class BankAccountTransactionsDto
    {
        public Guid BankAccountId { get; set; }
        public List<GetTransactionDetailsDto> Transactions { get; set; } = new();
    }
}
