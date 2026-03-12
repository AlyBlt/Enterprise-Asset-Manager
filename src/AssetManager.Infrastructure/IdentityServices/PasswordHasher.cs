using AssetManager.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Infrastructure.IdentityServices
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password);

        public bool VerifyPassword(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
