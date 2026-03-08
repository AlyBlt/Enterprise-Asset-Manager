using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
