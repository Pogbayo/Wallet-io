using Microsoft.EntityFrameworkCore;
using SpagWallet.Application.DTOs.BankAccountDtoBranch;
using SpagWallet.Application.DTOs.UserDtoBranch;
using SpagWallet.Application.DTOs.WalletDtoBranch;
using SpagWallet.Application.Interfaces;
using SpagWallet.Domain.Entities;
using SpagWallet.Domain.Enums.UserEnums;
using SpagWallet.Infrastructure.Persistence.Data;

namespace SpagWallet.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
           return await _context.Users.ToListAsync();
        }

        public async Task<UserBankDetails?> GetUserBankAsync(Guid userId)
        {
            var user = await _context.Users
                 .Include(u => u.BankAccount)
                 .FirstOrDefaultAsync(u => u.Id == userId);

            if (user?.BankAccount == null)
            {
                return null;
            }

            return new UserBankDetails
            {
                Id = user.Id,
                BankAccount = user.Wallet != null ? new BankAccountDto
                {
                    Id = user.BankAccount.Id,
                    AccountNumber = user.BankAccount.AccountNumber,
                    Balance = user.BankAccount.Balance,
                    CreatedAt = user.BankAccount.CreatedAt
                } : null
            };
        }


        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }


        public async Task<UserWalletDetails?> GetUserWalletAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                return null;
            }
            return new UserWalletDetails
            {
                Id = user.Id,
                Wallet = user.Wallet != null ? new WalletDto
                {
                    Id = user.Wallet.Id,
                    Balance = user.Wallet.Balance,
                    Currency = user.Wallet.Currency,
                    WalletNumber = user.Wallet.WalletNumber,
                    IsLocked = user.Wallet.IsLocked,
                    CreatedAt = user.Wallet.CreatedAt
                } : null
            };
        }

        public async Task<User?> UpdateUserAsync(Guid userId, User newUser)
        {
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null)
                return null;

            _context.Entry(existingUser).CurrentValues.SetValues(newUser);
            await _context.SaveChangesAsync();
            return existingUser;
        }


        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(w => w.Email == email);
            if (existingUser == null)
                return null;

            return new UserDto
            {
                Id = existingUser.Id,
                FirstName = existingUser.FirstName,
                Email = existingUser.Email,
                Role = UserRoleEnum.User,
                CreatedAt = existingUser.CreatedAt
            };
        }

        public async Task<bool> UpdateUserRoleAsync(Guid userId, UserRoleEnum newRole)
        {
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null)
                return false;
            _context.Entry(existingUser).CurrentValues.SetValues(newRole);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null)
            {
                return false;
            }
            _context.Users.Remove(user);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
