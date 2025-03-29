 using Application.DTOs.UserDtoBranch;
using Application.Interfaces.RepoInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using SpagWallet.Application.DTOs.UserDtoBranch;
using SpagWallet.Domain.Entities;
using SpagWallet.Domain.Enums.UserEnums;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Application.Common.Models;
using Microsoft.Extensions.Options;


namespace SpagWallet.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtSettings _jwtSettings;
        public UserService(
            IUserRepository userRepository,
            IAuditLogRepository auditLogRepository,
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtSettings> jwtOptions)
        {
            _userRepository = userRepository;
            _auditLogRepository = auditLogRepository;
            _httpContextAccessor = httpContextAccessor;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            bool isDeleted = await _userRepository.DeleteUserAsync(userId);

            {
                var auditLog = new AuditLog(
                    action: "Deleted User",
                    performedBy: userId,
                    details: $"User with ID {userId} was deleted."
                );

                await _auditLogRepository.AddLogAsync(auditLog);
            
            }
            return isDeleted;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var performedBy = GetCurrentUserId();

            var users = await _userRepository.GetAllUsersAsync();
            if (!users.Any())
                return new List<UserDto>();

            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            });

            var auditLog = new AuditLog(
                action: "Retrieved Users",
                performedBy:performedBy , 
                details: "User list was retrieved."
              );

            await _auditLogRepository.AddLogAsync(auditLog);

            return userDtos;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email does not exist");

            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user is null)
                return null;

            var userDto = new UserDto
               {
               Id = user.Id,
               FirstName = user.FirstName,
               Email = user.Email,
               Role = user.Role,
               CreatedAt = user.CreatedAt
               };

            return userDto;
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Email does not exist");

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user is null)
                return null;

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return userDto;
        }

        private bool verifyPassword(string loginPassword, string existingPassword)
        {
            if (loginPassword != existingPassword)
            {
                return false;
            }
            return true;
        }

        public async Task<string?> LoginUserAsync(LoginDto loginData)
        {
            if (string.IsNullOrEmpty(loginData.Email) && string.IsNullOrEmpty(loginData.Password))
                throw new ArgumentException("Invalid Email orpassword");

            var user = await _userRepository.GetUserByEmailAsync(loginData.Email);
            if (user == null || !verifyPassword(loginData.Password, user.PasswordHash))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                     new Claim(ClaimTypes.Email, user.Email),
                     new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<UserDto?> RegisterUserAsync(CreateUserDto userdata)
        { 
            if (userdata == null)
               throw new ArgumentException("User data not complete");

            var existingUser = await _userRepository.GetUserByEmailAsync(userdata.Email);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists");

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = userdata.FirstName,
                LastName = userdata.LastName,
                Email = userdata.Email,
                PasswordHash = userdata.PasswordHash,
                Role = UserRoleEnum.User,
                DateOfBirth = userdata.DateOfBirth,
                TransferPin = userdata.TransferPin,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddUserAsync(newUser);

            if (createdUser == null)
                return null;

            return new UserDto
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                Email = createdUser.Email,
                Role = createdUser.Role,
                CreatedAt = createdUser.CreatedAt
            };
        }

        public async Task<UserDto?> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null) return null;

            // Update only non-null values
            if (!string.IsNullOrWhiteSpace(updateUserDto.FirstName))
                existingUser.FirstName = updateUserDto.FirstName;

            if (!string.IsNullOrWhiteSpace(updateUserDto.LastName))
                existingUser.LastName = updateUserDto.LastName;

            if (!string.IsNullOrWhiteSpace(updateUserDto.Email))
                existingUser.Email = updateUserDto.Email;

            if (updateUserDto.DateOfBirth.HasValue)
                existingUser.DateOfBirth = updateUserDto.DateOfBirth.Value;

            var updatedUser = await _userRepository.UpdateUserAsync(userId,existingUser);
            if (updatedUser == null) return null;

            return new UserDto
            {
                Id = updatedUser.Id,
                FirstName = updatedUser.FirstName,
                Email = updatedUser.Email,
                Role = updatedUser.Role,
                CreatedAt = updatedUser.CreatedAt
            };
        }

    }
}
