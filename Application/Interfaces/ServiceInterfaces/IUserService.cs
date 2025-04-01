using Application.DTOs.UserDtoBranch;
using SpagWallet.Application.DTOs.UserDtoBranch;

namespace Application.Interfaces.ServiceInterfaces
{
    public interface IUserService
    {
        Task<UserDto?> RegisterUserAsync(CreateUserDto userdata);
        Task<UserDto?> GetUserByIdAsync(Guid userId);
        Task<string?> LoginUserAsync(LoginDto loginData);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
