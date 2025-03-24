using SpagWallet.Application.DTOs.UserDtoBranch;
using SpagWallet.Domain.Entities;
using SpagWallet.Domain.Enums.UserEnums;

namespace SpagWallet.Application.Interfaces
{
  public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> UpdateUserAsync(Guid userId, User user);
        Task<bool> UpdateUserRoleAsync(Guid userId, UserRoleEnum newRole);
        Task<UserWalletDetails?> GetUserWalletAsync(Guid userId);
        Task<UserBankDetails?> GetUserBankAsync(Guid userId);
        Task<UserDto?> GetUserByEmailAsync(string email); 
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
