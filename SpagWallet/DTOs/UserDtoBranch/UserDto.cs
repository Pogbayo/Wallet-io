using SpagWallet.Domain.Enums.UserEnums;

namespace SpagWallet.Application.DTOs.UserDtoBranch
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string Email { get; set; }
        public UserRoleEnum Role { get; set; } = UserRoleEnum.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
