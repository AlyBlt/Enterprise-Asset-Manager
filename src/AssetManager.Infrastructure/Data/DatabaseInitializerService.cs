using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore; 
using System.Threading;
using System.Threading.Tasks;
using AssetManager.Domain.Entities;
using AssetManager.Domain.Enums;

namespace AssetManager.Infrastructure.Data
{
    internal class DatabaseInitializerService(IServiceProvider serviceProvider) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AssetManagerDbContext>();

            // 1. Veritabanını oluştur/güncelle
            // SADECE ilişkisel bir DB (SQL Server gibi) kullanılıyorsa migration yap
            if (context.Database.IsRelational())
            {
                await context.Database.MigrateAsync(cancellationToken);
            }
            else
            {
                // InMemory veya Relational olmayan DB'lerde sadece şemayı oluştur
                await context.Database.EnsureCreatedAsync(cancellationToken);
            }

            // 2. Departmanları Hazırla (Eksikse ekle, varsa çek)
            if (!await context.Departments.AnyAsync(cancellationToken))
            {
                context.Departments.AddRange(
                    new DepartmentEntity { Name = "IT Infrastructure", Description = "Tech & Security" },
                    new DepartmentEntity { Name = "Human Resources", Description = "People Operations" },
                    new DepartmentEntity { Name = "Sales", Description = "Global Sales Team" },
                    new DepartmentEntity { Name = "Finance", Description = "Accounting" }
                );
                await context.SaveChangesAsync(cancellationToken);
            }

            // Değişkenleri dışarıda kullanabilmek için DB'den tekrar çekiyoruz
            var itDept = await context.Departments.FirstOrDefaultAsync(d => d.Name == "IT Infrastructure", cancellationToken);
            var hrDept = await context.Departments.FirstOrDefaultAsync(d => d.Name == "Human Resources", cancellationToken);
            var salesDept = await context.Departments.FirstOrDefaultAsync(d => d.Name == "Sales", cancellationToken);

            // 3. Kullanıcıları Hazırla
            if (!await context.Users.AnyAsync(cancellationToken))
            {
                var admin = new AppUserEntity
                {
                    Username = "admin",
                    FullName = "System Admin",
                    Email = "admin@enterprise.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = Roles.Admin,
                    DepartmentId = itDept?.Id // Artık güvenle erişebiliriz
                };

                var editor = new AppUserEntity
                {
                    Username = "jdoe",
                    FullName = "John Doe",
                    Email = "j.doe@enterprise.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("editor123"),
                    Role = Roles.Editor,
                    DepartmentId = hrDept?.Id
                };

                var guest = new AppUserEntity
                {
                    Username = "awinehouse",
                    FullName = "Amy Winehouse",
                    Email = "amy.w@enterprise.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                    Role = Roles.Guest,
                    DepartmentId = salesDept?.Id
                };

                context.Users.AddRange(admin, editor, guest);
                await context.SaveChangesAsync(cancellationToken);
            }

            // 4. Assetleri Hazırla
            if (!await context.Assets.AnyAsync(cancellationToken))
            {
                // Kullanıcıları ID'leri ile birlikte tekrar çekelim
                var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin", cancellationToken);
                var editorUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "jdoe", cancellationToken);

                context.Assets.AddRange(
                    new AssetEntity
                    {
                        Name = "MacBook Pro M3 16\"",
                        Category = "Laptop",
                        SerialNumber = "AAPL-M3-001",
                        Value = 2750.00m,
                        Status = AssetStatus.Assigned,
                        AssignedUserId = adminUser?.Id,
                        Description = "High-end development machine."
                    },
                    new AssetEntity
                    {
                        Name = "Dell XPS 15",
                        Category = "Laptop",
                        SerialNumber = "DELL-XPS-442",
                        Value = 2300.00m,
                        Status = AssetStatus.Assigned,
                        AssignedUserId = editorUser?.Id,
                        Description = "Standard issue for HR management."
                    },
                    new AssetEntity
                    {
                        Name = "HP LaserJet Enterprise",
                        Category = "Printer",
                        SerialNumber = "HP-PRNT-99",
                        Value = 3500.00m,
                        Status = AssetStatus.Active,
                        Description = "Common area printer - 2nd Floor."
                    },
                    new AssetEntity
                    {
                        Name = "Logitech MX Master 3S",
                        Category = "Peripherals",
                        SerialNumber = "LOGI-MX-55",
                        Value = 80.00m,
                        Status = AssetStatus.InStock,
                        Description = "Spare mouse in IT storage."
                    },
                    new AssetEntity
                    {
                        Name = "Samsung 27\" Curved Monitor",
                        Category = "Monitor",
                        SerialNumber = "SAM-27-CURV",
                        Value = 1500.00m,
                        Status = AssetStatus.InStock,
                        Description = "Ready for new recruits."
                    },
                    new AssetEntity
                    {
                        Name = "iPad Pro 12.9\"",
                        Category = "Tablet",
                        SerialNumber = "AAPL-TAB-11",
                        Value = 650.00m,
                        Status = AssetStatus.InRepair,
                        Description = "Screen replacement in progress."
                    },
                    new AssetEntity
                    {
                        Name = "Jabra Evolve 75 Headset",
                        Category = "Audio",
                        SerialNumber = "JAB-HSET-02",
                        Value = 750.00m,
                        Status = AssetStatus.Lost,
                        Description = "Reported missing during office relocation."
                    },
                    new AssetEntity
                    {
                        Name = "ThinkPad T470",
                        Category = "Laptop",
                        SerialNumber = "LNV-T470-OLD",
                        Value = 2000.00m,
                        Status = AssetStatus.Retired,
                        Description = "Legacy hardware, parts salvaged."
                    }
                );
                await context.SaveChangesAsync(cancellationToken);
            }
        }


        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

