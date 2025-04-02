using API.Common;
using Application.DTOs.UserDtoBranch;
using Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpagWallet.Application.DTOs.UserDtoBranch;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _identityService;

        public UserController(IUserService IdentityService)
        {
            _identityService = IdentityService;
        }

        [HttpGet("get-all-users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<List<UserDto>>>>
            GetAllUsers()
        {
            var users = await _identityService.GetAllUsersAsync();

            if (users == null || !users.Any())
                return NotFoundResponse<List<UserDto>>(
                    new List<string> { "Error getting users" },
                    "users not fetched successfully"
                );

            return Success
                (users.ToList(),
                "Users retrieved successfully.");
        }

        [HttpGet("get-user-by-id/{userId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<ApiResponse<UserDto>>>
            GetUserById(Guid userId)
        {
            var user = await _identityService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFoundResponse<UserDto>(
                   new List<string> { "User not found" },
                   "No user exists with the provided ID"
                );

            return Success
                (user, "User retrieved successfully");
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult<ApiResponse<UserDto>>>
            SignUp([FromBody] CreateUserDto userdata)
        {
            var registeredUser = await _identityService.RegisterUserAsync(userdata);

            if (registeredUser == null)
            {
                return BadRequest(Failure<UserDto>(
                    new List<string> { "User registration failed" },
                    "Unable to register user"
                ));
            }

            return CreatedAtAction(
                 nameof(GetUserById),
                 new { userId = registeredUser.Id },
                 Success(registeredUser, "User registration successful")
            );
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<ApiResponse<string?>>>
            SignIn([FromBody] LoginDto loginData)
        {
            var loggedInUser = await _identityService.LoginUserAsync(loginData);
            if (string.IsNullOrEmpty(loggedInUser))
            {
                return Failure<string?>(
                   new List<string> { "Invalid email or password" },
                   "Authentication failed"
                );
            }
            return Success<string?>(loggedInUser, "User logged in successfully");
        }

        [HttpDelete("delete-user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>>
            DeleteUser(Guid userId)
        {
            bool result = await _identityService.DeleteUserAsync(userId);
            if (!result)
            {
                return NotFoundResponse<bool>
                   (new List<string>
                   { "Error deleting user" },
                 "Unable to delete user");
            }
            return Success(result, "User deleted successfully");
        }
    }
}
