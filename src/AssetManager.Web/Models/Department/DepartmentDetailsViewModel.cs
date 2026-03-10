namespace AssetManager.Web.Models.Department
{
    public class DepartmentDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserCount { get; set; }
        public string PageTitle { get; set; } = "Department Details";
    }
}
