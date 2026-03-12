using AssetManager.Application.Interfaces.Services;
using AssetManager.Application.Mappings;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AssetManager.Application.Services;
using MediatR;
using AssetManager.Application.Common.Behaviors;

namespace AssetManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            // 1. AutoMapper Ayarı
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // 2. MediatR Ayarı (Tüm Handler'ları otomatik bulur)
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(assembly);
                // Yazdığımız ValidationBehavior'ı araya sokuyoruz
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });
            // 3. FluentValidation Ayarı (Tüm Validator'ları otomatik bulur)
            services.AddValidatorsFromAssembly(assembly);

            services.AddScoped<IAuditLogService, AuditLogService>();
          
           
            return services;
        }
    }
}