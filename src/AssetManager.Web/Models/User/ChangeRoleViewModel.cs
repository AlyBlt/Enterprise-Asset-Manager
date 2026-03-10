namespace AssetManager.Web.Models.User
{
    public class ChangeRoleViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string CurrentRole { get; set; } = string.Empty;
        public string NewRole { get; set; } = string.Empty; // Admin, Editor, User, Guest
    }
}
