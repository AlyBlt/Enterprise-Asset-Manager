using AssetManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? DepartmentName { get; set; } // İlişkili tablodan sadece isim alacağız
        public int? DepartmentId { get; set; }
    }
}
