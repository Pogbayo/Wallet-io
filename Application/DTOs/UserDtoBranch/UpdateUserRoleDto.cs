
using SpagWallet.Domain.Enums.UserEnums;

namespace SpagWallet.Application.DTOs.UserDtoBranch
{
    public class UpdateUserRoleDto
    {
        public UserRoleEnum Role { get; set; } = UserRoleEnum.User;
    }
}
