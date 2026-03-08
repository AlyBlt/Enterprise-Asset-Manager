using AssetManager.Application.Interfaces.Services;
using AssetManager.Application.Mappings;
using AssetManager.Application.Services;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AssetManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IDepartmentService, DepartmentService>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}