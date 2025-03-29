using SpagWallet.Domain.Entities;
using SpagWallet.Domain.Enums.UserEnums;

namespace Application.Interfaces.RepoInterfaces
{
    public interface IUserRepository
    {
        Task<User?> AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> UpdateUserAsync(Guid userId, User user);
        Task<bool> UpdateUserRoleAsync(Guid userId, UserRoleEnum newRole);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<User?> GetUserWithWalletAsync(Guid userId);
        Task<User?> GetUserWithBankAccountAsync(Guid userId);
    }
}
