

using SpagWallet.Domain.Enums.UserEnums;

namespace SpagWallet.Application.DTOs.UserDtoBranch
{
    public class UpdateUserDto
    {
        public  string? FirstName { get; set; }
        public  string? LastName { get; set; }
        public  string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserRoleEnum Role { get; set; } = UserRoleEnum.User;

    }
}
