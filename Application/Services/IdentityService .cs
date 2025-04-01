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
    public class IdentityService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtSettings _jwtSettings;
        private readonly ICurrentUserService _currentUserService;
        public Guid currentuserId { get; }
        public IdentityService(
            IUserRepository userRepository,
            IAuditLogRepository auditLogRepository,
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtSettings> jwtOptions,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _auditLogRepository = auditLogRepository;
            _httpContextAccessor = httpContextAccessor;
            _jwtSettings = jwtOptions.Value;
            _currentUserService = currentUserService;
            currentuserId = currentUserService.GetUserId();
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
                    performedBy: currentuserId,
                    details: $"User with ID {userId} was deleted."
                );

                await _auditLogRepository.AddLogAsync(auditLog);
            
            }
            return isDeleted;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
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
                performedBy: currentuserId, 
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

            var auditLog = new AuditLog(
              action: "Retrieved User",
              performedBy: currentuserId,
              details: $"User fetched by {currentuserId}."
            );

            await _auditLogRepository.AddLogAsync(auditLog);

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

            var auditLog = new AuditLog(
                  action: "Retrieved User",
                  performedBy: currentuserId,
                  details: $"User fetched by {currentuserId}."
             );

            await _auditLogRepository.AddLogAsync(auditLog);

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
            var auditLog = new AuditLog(
                  action: "Sign in",
                  performedBy: currentuserId,
                  details: $"{user.Id} logged in"
             );

            await _auditLogRepository.AddLogAsync(auditLog);

            return tokenHandler.WriteToken(token);
        }


        public async Task<UserDto?> RegisterUserAsync(CreateUserDto userdata)
        {
            if (userdata == null)
               throw new ArgumentException("User data not complete");

            var existingUser = await _userRepository.GetUserByEmailAsync(userdata.Email);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists");

            var userEntity = new User
            {
                Id = Guid.NewGuid(),
                FirstName = userdata.FirstName,
                LastName = userdata.LastName,
                TransferPin = userdata.TransferPin,
                PasswordHash = userdata.PasswordHash,
                Email = userdata.Email,
                Role = UserRoleEnum.User,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddUserAsync(userEntity);

            if (createdUser == null)
                return null;

            var auditLog = new AuditLog(
                  action: "User registration",
                  performedBy: currentuserId,
                  details: $"{userEntity.Id} just registered."
             );

            await _auditLogRepository.AddLogAsync(auditLog);

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

            string oldRole = existingUser.Role.ToString();

            if (!string.IsNullOrWhiteSpace(updateUserDto.FirstName))
                existingUser.FirstName = updateUserDto.FirstName;

            if (!string.IsNullOrWhiteSpace(updateUserDto.LastName))
                existingUser.LastName = updateUserDto.LastName;

            if (!string.IsNullOrWhiteSpace(updateUserDto.Email))
                existingUser.Email = updateUserDto.Email;

            if (updateUserDto.DateOfBirth.HasValue)
                existingUser.DateOfBirth = updateUserDto.DateOfBirth.Value;

            if (updateUserDto.Role != existingUser.Role)
            {
                oldRole = existingUser.Role.ToString();
                existingUser.Role = updateUserDto.Role;
            }

            var updatedUser = await _userRepository.UpdateUserAsync(userId, existingUser);
            if (updatedUser == null) return null;

            var auditLog = new AuditLog(
                action: "User update",
                performedBy: currentuserId,
                details: $"User with ID {userId} updated by {currentuserId}. Role changed from {oldRole} to {updatedUser.Role}."
            );

            await _auditLogRepository.AddLogAsync(auditLog);

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

