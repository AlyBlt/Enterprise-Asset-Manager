using AssetManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(AppUserEntity user);
    }
}
