﻿
namespace SpagWallet.Application.DTOs.AuthenticationDto
{
    public class SignUpDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
