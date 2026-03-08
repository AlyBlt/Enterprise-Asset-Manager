using AssetManager.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Core.Entities
{
    public class AppUserEntity : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Güvenlik için şifreyi açık tutmuyoruz
        public Roles Role { get; set; } = Roles.Guest;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // İlişki: Bir kullanıcının bir departmanı olur
        public int? DepartmentId { get; set; }
        public DepartmentEntity? Department { get; set; }
    }
}
