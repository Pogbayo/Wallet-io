using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpagWallet.Application.DTOs.KycDtoBranch
{
    public class CreateKycDto
    {
        public required Guid UserId { get; set; }
        public required string FullName { get; set; }
        public string? IdentificationType { get; set; }
        public required string IdentificationNumber { get; set; }
    }

}
