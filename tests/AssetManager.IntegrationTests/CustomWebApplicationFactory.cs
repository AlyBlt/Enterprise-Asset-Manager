using AssetManager.Domain.Entities;
using AssetManager.Domain.Enums;
using AssetManager.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.Common;

namespace AssetManager.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. DbContext ve EF ile ilgili HER ŞEYİ (Internal Provider dahil) kökten kazı
            var efDescriptors = services.Where(d =>
                d.ServiceType.FullName.Contains("EntityFrameworkCore") ||
                d.ServiceType == typeof(AssetManagerDbContext) ||
                d.ServiceType == typeof(DbContextOptions<AssetManagerDbContext>)).ToList();

            foreach (var d in efDescriptors) services.Remove(d);

            // 2. DatabaseInitializerService'i kaldır (Production'daki çalışmasın)
            var initializer = services.SingleOrDefault(d => d.ImplementationType == typeof(DatabaseInitializerService));
            if (initializer != null) services.Remove(initializer);

            // 3. SQLite Bağlantısını oluştur ve Singleton yap
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            services.AddSingleton<DbConnection>(connection);

            // 4. DbContext'i TERTEMİZ bir sayfada SQLite olarak kur
            services.AddDbContext<AssetManagerDbContext>(options =>
            {
                options.UseSqlite(connection);
                // Uyarıları kapat
                options.ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning));
                options.ConfigureWarnings(x => x.Ignore(RelationalEventId.PendingModelChangesWarning));
            });

            // 5. Şemayı oluştur ve Seed Datayı elle tetikle
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AssetManagerDbContext>();

            // SQLite 'Relational' olduğu için burada asla hata almazsın
            db.Database.EnsureCreated();

            // Önce her şeyi sil (Tertemiz bir başlangıç için)
            db.Users.RemoveRange(db.Users);
            db.Departments.RemoveRange(db.Departments);
            db.SaveChanges();

            // Departmanları ekle
            var itDept = new DepartmentEntity { Name = "IT Infrastructure", Description = "Tech" };
            var hrDept = new DepartmentEntity { Name = "Human Resources", Description = "HR" };
            db.Departments.AddRange(itDept, hrDept);
            db.SaveChanges();

            // Kullanıcıları ekle
            db.Users.AddRange(
                new AppUserEntity
                {
                    Username = "admin",
                    FullName = "System Admin",
                    Email = "admin@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), // Şifre tam olarak bu!
                    Role = Roles.Admin,
                    DepartmentId = itDept.Id
                },
                new AppUserEntity
                {
                    Username = "jdoe",
                    FullName = "John Doe",
                    Email = "jdoe@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("editor123"),
                    Role = Roles.Editor,
                    DepartmentId = hrDept.Id
                },
                new AppUserEntity
                {
                    Username = "awinehouse",
                    FullName = "Amy Winehouse",
                    Email = "amy@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                    Role = Roles.Guest,
                    DepartmentId = hrDept.Id
                }
            );
            db.SaveChanges();
        });
    }
}