
namespace SpagWallet.Application.DTOs.UserDtoBranch
{
   public class CreateUserDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string TransferPin { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
