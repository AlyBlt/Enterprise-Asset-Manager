using AssetManager.Application.Interfaces.Repositories;
using AssetManager.Application.Interfaces.Services;
using AssetManager.Application.Services;
using AssetManager.Infrastructure.Data;
using AssetManager.Infrastructure.Repositories;
using AssetManager.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AssetManagerDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // IHostedService kaydı (Database Migration/Seed için)
            services.AddHostedService<DatabaseInitializerService>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, TokenService>();


            return services;
        }
    }
}
