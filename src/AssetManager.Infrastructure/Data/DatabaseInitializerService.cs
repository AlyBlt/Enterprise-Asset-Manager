using AssetManager.Core.Entities;
using AssetManager.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore; 
using System.Threading;
using System.Threading.Tasks;

namespace AssetManager.Infrastructure.Data
{
    internal class DatabaseInitializerService(IServiceProvider serviceProvider) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AssetManagerDbContext>();

            // 1. Veritabanını oluştur/güncelle
            await context.Database.MigrateAsync(cancellationToken);

            // 2. Eğer Departman yoksa, temel yapıyı kur
            if (!await context.Departments.AnyAsync(cancellationToken))
            {
                // Departmanları oluşturuyoruz
                var itDept = new DepartmentEntity { Name = "IT Department", Description = "Information Technology & Infrastructure" };
                var hrDept = new DepartmentEntity { Name = "Human Resources", Description = "People and Culture" };
                var financeDept = new DepartmentEntity { Name = "Finance", Description = "Accounting and Financial Planning" };

                context.Departments.AddRange(itDept, hrDept, financeDept);
                await context.SaveChangesAsync(cancellationToken);

                // 3. Eğer Kullanıcı yoksa, Admin ve örnek personelleri ekle
                if (!await context.Users.AnyAsync(cancellationToken))
                {
                    var adminUser = new AppUserEntity
                    {
                        Username = "admin",
                        FullName = "System Admin",
                        Email = "admin@enterprise.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                        Role = Roles.Admin,
                        DepartmentId = itDept.Id
                    };

                    var staffUser = new AppUserEntity
                    {
                        Username = "jdoe",
                        FullName = "John Doe",
                        Email = "j.doe@enterprise.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("editor123"),
                        Role = Roles.Editor,
                        DepartmentId = hrDept.Id
                    };

                    var staffUser2 = new AppUserEntity
                    {
                        Username = "awinehouse",
                        FullName = "Amy Winehouse",
                        Email = "j.doe@enterprise.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                        Role = Roles.Guest,
                        DepartmentId = hrDept.Id
                    };


                    context.Users.AddRange(adminUser, staffUser, staffUser2);
                    await context.SaveChangesAsync(cancellationToken);

                    // 4. Eğer Varlık (Asset) yoksa, örnek envanter ekle
                    if (!await context.Assets.AnyAsync(cancellationToken))
                    {
                        context.Assets.AddRange(
                            new AssetEntity
                            {
                                Name = "MacBook Pro M3",
                                Category = "Laptop",
                                SerialNumber = "AAPL-123456",
                                Value = 85000.00m,
                                AssignedUserId = adminUser.Id,
                                Status = AssetStatus.Assigned 
                            },
                            new AssetEntity
                            {
                                Name = "Dell UltraSharp 27\"",
                                Category = "Monitor",
                                SerialNumber = "DELL-987654",
                                Value = 12500.00m,
                                AssignedUserId = adminUser.Id,
                                Status = AssetStatus.Assigned 
                            },
                            new AssetEntity
                            {
                                Name = "Logitech MX Master 3S",
                                Category = "Peripherals",
                                SerialNumber = "LOGI-456789",
                                Value = 3500.00m,
                                AssignedUserId = staffUser.Id,
                                Status = AssetStatus.Assigned 
                            }
                        );
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
