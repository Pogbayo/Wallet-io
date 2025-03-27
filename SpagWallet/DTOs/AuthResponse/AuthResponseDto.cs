﻿
namespace SpagWallet.Application.DTOs.AuthResponse
{

    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
