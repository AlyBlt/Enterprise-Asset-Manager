using System.ComponentModel.DataAnnotations;

namespace AssetManager.Web.Models.Department
{
    public class DepartmentCreateViewModel
    {
        [Required(ErrorMessage = "Department name is required.")]
        [Display(Name = "Department Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
    }
}
