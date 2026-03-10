using System.ComponentModel.DataAnnotations;

namespace AssetManager.Web.Models.Asset;

public class AssetCreateViewModel
{
    [Required(ErrorMessage = "Asset name is required.")]
    [Display(Name = "Asset Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    [Display(Name = "Category")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Serial number is required.")]
    [Display(Name = "Serial Number")]
    public string SerialNumber { get; set; } = string.Empty;
}