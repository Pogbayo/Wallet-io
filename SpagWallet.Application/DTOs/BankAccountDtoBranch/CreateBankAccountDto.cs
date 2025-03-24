using SpagWallet.Domain.Enums.BankAccountEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpagWallet.Application.DTOs.BankAccountDtoBranch
{
   public class CreateBankAccountDto
    {
        public Guid UserId { get; set; }
        public AccountType AccountType { get; set; }
        public decimal InitialDeposit { get; set; }
    }
}
