using AssetManager.Application.DTOs.User;

namespace AssetManager.Web.Models.User
{
    public class UserListViewModel
    {
        // API'den gelecek ham kullanıcı listesi (DTO)
        public IEnumerable<UserDto> Users { get; set; } = [];
        public string PageTitle { get; set; } = "User Management";
    }
}
