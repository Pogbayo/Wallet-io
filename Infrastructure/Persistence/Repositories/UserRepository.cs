using Application.Interfaces.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using SpagWallet.Domain.Entities;
using SpagWallet.Domain.Enums.UserEnums;
using SpagWallet.Infrastructure.Persistence.Data;

namespace SpagWallet.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _userTable;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            _userTable = context.Users;
        }

        public async Task<User?> AddUserAsync(User user)
        {
            await _userTable.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _userTable.FindAsync(userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userTable.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userTable.ToListAsync();
        }

        public async Task<User?> UpdateUserAsync(Guid userId, User newUser)
        {
            var existingUser = await _userTable.FindAsync(userId);
            if (existingUser == null) return null;

            _context.Entry(existingUser).CurrentValues.SetValues(newUser);
            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> UpdateUserRoleAsync(Guid userId, UserRoleEnum newRole)
        {
            var user = await _userTable.FindAsync(userId);
            if (user == null) return false;

            user.Role = newRole;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _userTable.FindAsync(userId);
            if (user == null) return false;

            _userTable.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User?> GetUserWithWalletAsync(Guid userId)
        {
            return await _userTable
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
        public async Task<User?> GetUserWithBankAccountAsync(Guid userId)
        {
            return await _userTable
                .Include(u => u.BankAccount)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
