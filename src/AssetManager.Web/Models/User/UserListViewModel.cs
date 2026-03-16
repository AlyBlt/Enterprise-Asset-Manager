using AssetManager.Application.DTOs.Department;
using AssetManager.Application.DTOs.User;

namespace AssetManager.Web.Models.User
{
    public class UserListViewModel
    {
        // API'den gelecek ham kullanıcı listesi (DTO)
        public IEnumerable<UserDto> Users { get; set; } = [];
        public IEnumerable<DepartmentResponseDto> Departments { get; set; } = [];
        public string PageTitle { get; set; } = "User Management";
    }
}
